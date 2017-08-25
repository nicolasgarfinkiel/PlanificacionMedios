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

                   var aux = $scope.entity.vehiculos;
                   $scope.entity.vehiculos = [];

                   aux.forEach(function(item) {
                       var vehiculo = $scope.getVehiculoById(item.id);
                       vehiculo.tick = true;
                       $scope.entity.vehiculos.push(vehiculo);
                   });
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: proveedoresService,
                   navigation: baseNavigationService
               });

               $scope.localLang = {
                   selectAll: "Seleccionar todos",
                   selectNone: "Ninguno",
                   reset: "Limpiar",
                   search: "Buscar...",
                   nothingSelected: "Seleccione uno o más vehículos" 
               };

               $scope.getVehiculoById = function (id) {
                   var result = null;

                   for (var i = 0; i < $scope.data.vehiculos.length; i++) {
                       if ($scope.data.vehiculos[i].id == id) {
                           result = $scope.data.vehiculos[i];
                           break;
                       }
                   }

                   return result;
               };
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };                   

                   if (!$scope.entity.nombre) {
                       $scope.result.messages.push('Ingrese el nombre');
                   }

                   if (!$scope.entity.numeroProveedorSap) {
                       $scope.result.messages.push('Ingrese el número de proveedor SAP');
                   }

                   if (!$scope.entity.vehiculos.length) {
                       $scope.result.messages.push('Seleccione uno o más vehículos');
                   }

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };             
           }]);