angular.module('irsa.pdm.vehiculos.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'vehiculosService',
           'baseNavigationService',
           'editBootstraperService',
           'mediosService',
           function ($scope, $routeParams, vehiculosService, baseNavigationService, editBootstraperService, mediosService) {

               $scope.onInitEnd = function () {
                   $scope.operation = !$routeParams.id ? 'Nuevo vehículo' : 'Edición de vehículo';
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: vehiculosService,
                   navigation: baseNavigationService
               });
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };

                   if (!$scope.entity.nombre) {
                       $scope.result.messages.push('Ingrese el nombre');
                   }

                   if (!$scope.entity.medio) {
                       $scope.result.messages.push('Seleccione el medio');
                   }

                   if (!$scope.entity.descripcion) {
                       $scope.result.messages.push('Ingrese la descripción');
                   }

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };

               //#region Select UI

               $scope.selectList = [];
               $scope.currentPage = 0;
               $scope.pageCount = 0;
               $scope.filterMedios = { pageSize: 20 };

               $scope.getSelectSource = function ($select, $event) {
                   if ($scope.loading) return;

                   if (!$event) {
                       $scope.currentPage = 1;
                       $scope.pageCount = 0;
                       $scope.selectList = [];
                   } else {
                       $event.stopPropagation();
                       $event.preventDefault();
                       $scope.currentPage++;
                   }

                   $scope.filterMedios.currentPage = $scope.currentPage;
                   $scope.filterMedios.multiColumnSearchText = $select.search;

                   mediosService.getByFilter($scope.filterMedios).then(function (response) {
                       $scope.selectList = $scope.selectList.concat(response.data.data);
                       $scope.pageCount = Math.ceil(response.data.count / 20);
                   }, function () { throw 'Error on getByFilter'; });
               };
              
               //#endregion              
           }]);