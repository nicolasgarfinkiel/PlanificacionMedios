angular.module('irsa.pdm.medios.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'mediosService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, mediosService, baseNavigationService, editBootstraperService) {

               $scope.onInitEnd = function () {
                   $scope.operation = !$routeParams.id ? 'Nuevo medio' : 'Edición de medio';                   
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: mediosService,
                   navigation: baseNavigationService
               });
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };                   

                   if (!$scope.entity.nombre) {
                       $scope.result.messages.push($scope.entity.esChoferTransportista ? 'Ingrese la descripción' : 'Ingrese el nombre');
                   }                                    

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };             
           }]);