﻿angular.module('irsa.pdm.medios.ctrl.edit', [])
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
                       $scope.result.messages.push('Ingrese el nombre');
                   }

                   if (!$scope.entity.tipoEspacio) {
                       $scope.result.messages.push('Seleccione el tipo de espacio');
                   }

                   if (!$scope.entity.descripcion) {
                       $scope.result.messages.push('Ingrese la descripción');
                   }

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };             
           }]);