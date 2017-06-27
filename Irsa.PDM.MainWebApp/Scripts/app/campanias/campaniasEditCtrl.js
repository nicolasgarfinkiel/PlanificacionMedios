angular.module('irsa.pdm.campanias.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'campaniasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, campaniasService, baseNavigationService, editBootstraperService) {
               $scope.pautas = {};
               $scope.resultModal = { hasErrors: false, messages: [] };

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

               editBootstraperService.init($scope, $routeParams, {
                   service: campaniasService,
                   navigation: baseNavigationService
               });

               //#endregion

               $scope.confirmRechazo = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $scope.motivo = null;
                   $('#rechazoModal').modal('show');
               };

               $scope.changeEstado = function (estado) {
                   $scope.resultModal = $scope.result = { hasErrors: false, messages: [] };

                   if (!$scope.motivo && estado == 'Rechazada') {
                       $scope.resultModal = { hasErrors: true, messages: ['Ingrese el motivo'] };
                       return;
                   }

                   campaniasService.changeEstadoCampania($scope.entity.id, estado, $scope.motivo).then(function (response) {
                       $scope.resultModal = $scope.result = response.data.result;

                       if ($scope.resultModal.hasErrors) return;

                       $scope.entity.estado = estado;

                       $scope.entity.pautas.forEach(function (item) {
                           item.estado = estado;
                       });

                       $scope.entity.pautas = angular.copy($scope.entity.pautas);
                       $scope.motivo = null;
                       $('#rechazoModal').modal('hide');
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

               $scope.openTarifaModal = function(tarifa) {
                   $scope.currentTarifa = tarifa;
                   $('#tarifaModal').modal('show');
               };
           }]);