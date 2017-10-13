angular.module('irsa.pdm.certificaciones.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'certificacionesService',
           'campaniasService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, certificacionesService, campaniasService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;
               $scope.certificaciones = [];

               $scope.openModalPauta = function (certificacion) {
                   $scope.currentCertificacion = certificacion;

                   $scope.filterPauta = {
                       currentPage: 1,
                       pageSize: 10,
                       pautaCodigo: certificacion.pautaCodigo,
                       campaniaCodigo: certificacion.campaniaCodigo
                   };
                   
                   $scope.findItems();
                   $('#pautaModal').modal('show');
               };

               //#region Filters

               $scope.filter = {                 
                   currentPage: 1,
                   pageSize: 10                   
               };

               $scope.isFirstPage = function () {
                   return $scope.filter.currentPage == 1;
               };

               $scope.isLastPage = function () {
                   return $scope.filter.currentPage == $scope.filter.pageCount;
               };

               $scope.goToNextPage = function () {
                   if ($scope.isLastPage()) return;
                   $scope.filter.currentPage++;
                   $scope.find();
               };

               $scope.goToPreviousPage = function () {
                   if ($scope.isFirstPage()) return;
                   $scope.filter.currentPage--;
                   $scope.find();
               };

           
               $scope.find = function () {
                   certificacionesService.getByFilter($scope.filter).then(function (response) {
                       $scope.certificaciones = response.data.data;
                       $scope.filter.count = response.data.count;
                       $scope.filter.pageCount = Math.ceil($scope.filter.count / $scope.filter.pageSize);
                   });
               };

               $scope.clearFilter = function () {
                   $scope.filter = {
                       currentPage: 1,
                       pageSize: 10
                   };

                   $scope.find();
               };
               

               //#endregion

               //#region PautaDetail

               $scope.pautaItemList = [];

               $scope.filterPauta = {
                   currentPage: 1,
                   pageSize: 10
               };

               $scope.isFirstPagePauta = function () {
                   return $scope.filterPauta.currentPage == 1;
               };

               $scope.isLastPagePauta = function () {
                   return $scope.filterPauta.currentPage == $scope.filterPauta.pageCount;
               };

               $scope.goToNextPagePauta = function () {
                   if ($scope.isLastPage()) return;
                   $scope.filterPauta.currentPage++;
                   $scope.findItems();
               };

               $scope.goToPreviousPagePauta = function () {
                   if ($scope.isFirstPage()) return;
                   $scope.filterPauta.currentPage--;
                   $scope.findItems();
               };

               $scope.findItems = function () {
                   campaniasService.getItemsByFilter($scope.filterPauta).then(function (response) {
                       $scope.pautaItemList = response.data.data;
                       $scope.filterPauta.count = response.data.count;
                       $scope.filterPauta.pageCount = Math.ceil($scope.filterPauta.count / $scope.filterPauta.pageSize);
                   });
               };              

               //#endregion

               //#region Init

               certificacionesService.getDataListInit().then(function (response) {
                   $scope.data = response.data.data.data;
                   $scope.find();
               });

               //#endregion
           }]);