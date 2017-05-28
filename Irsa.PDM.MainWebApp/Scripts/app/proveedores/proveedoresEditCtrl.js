angular.module('irsa.pdm.proveedores.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'proveedoresService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, proveedoresService, baseNavigationService, editBootstraperService) {

               $scope.onInitEnd = function () {
                   $scope.entity.vehiculos = $scope.entity.vehiculos || [];
                   $scope.operation = !$routeParams.id ? 'Nuevo proveedor' : 'Edición de proveedor';                   
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: proveedoresService,
                   navigation: baseNavigationService
               });
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };                   

                   if (!$scope.entity.nombre) {
                       $scope.result.messages.push('Ingrese el nombre');
                   }                  

                   if (!$scope.entity.vehiculos.length) {
                       $scope.result.messages.push('Seleccione uno o más vehículos');
                   }

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };             
           }]);