angular.module('irsa.pdm.plazas.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'plazasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, plazasService, baseNavigationService, editBootstraperService) {

               $scope.onInitEnd = function () {
                   $scope.operation = !$routeParams.id ? 'Nueva plaza' : 'Edición de plaza';                   
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: plazasService,
                   navigation: baseNavigationService
               });
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };                   

                   if (!$scope.entity.codigo) {
                       $scope.result.messages.push('Ingrese el código');
                   }                  

                   if (!$scope.entity.descripcion) {
                       $scope.result.messages.push('Ingrese la descripción');
                   }

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };             
           }]);