angular.module('irsa.pdm.campanias.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'campaniasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, campaniasService, baseNavigationService, editBootstraperService) {
               $scope.pautas = {};

               //#region base 

               $scope.onInitEnd = function () {
                   $scope.entity.pautas.forEach(function (pauta) {
                       $scope.pautas[pauta.id.toString()] = {
                           data: [],
                           filter: {                               
                               currentPage: 1,
                               pageSize: 10,
                               pautaId: pauta.id
                           }
                       };

                       $scope.findItems(pauta.id);
                   });
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: campaniasService,
                   navigation: baseNavigationService
               });

               //#endregion

               $scope.changeEstadoPauta = function(pauta, estado) {
                   if (!confirm('Desea continuar con la operación?')) return;

                   campaniasService.changeEstadoPauta(pauta.id, estado).then(function (response) {
                       $scope.result = response.data.result;
                       pauta.estado = estado;
                       $scope.entity.pautas = angular.copy($scope.entity.pautas);                       
                   }, function () { throw 'Error on changeEstadoPauta'; });
               };

               $scope.isFirstPage = function (pautaId) {
                   return $scope.pautas[pautaId.toString()].filter.currentPage == 1;
               };

               $scope.isLastPage = function (pautaId) {
                   return $scope.pautas[pautaId.toString()].filter.currentPage == $scope.pautas[pautaId.toString()].filter.pageCount;
               };

               $scope.goToNextPage = function (pautaId) {
                   if ($scope.isLastPage(pautaId)) return;
                   $scope.pautas[pautaId.toString()].filter.currentPage++;
                   $scope.findItems(pautaId);
               };

               $scope.goToPreviousPage = function (pautaId) {
                   if ($scope.isFirstPage(pautaId)) return;
                   $scope.pautas[pautaId.toString()].filter.currentPage--;
                   $scope.findItems(pautaId);
               };

               $scope.findItems = function (pautaId) {
                   campaniasService.getItemsByFilter($scope.pautas[pautaId.toString()].filter).then(function (response) {
                       $scope.pautas[pautaId.toString()].data = response.data.data;
                       $scope.pautas[pautaId.toString()].filter.count = response.data.count;
                       $scope.pautas[pautaId.toString()].filter.pageCount = Math.ceil($scope.pautas[pautaId.toString()].filter.count / $scope.pautas[pautaId.toString()].filter.pageSize);
                   });
               };
           }]);