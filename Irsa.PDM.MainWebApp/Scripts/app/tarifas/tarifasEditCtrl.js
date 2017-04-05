angular.module('irsa.pdm.tarifas.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'tarifasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, tarifasService, baseNavigationService, editBootstraperService) {               
               //#region base

               $scope.onInitEnd = function () {                   
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: tarifasService,
                   navigation: baseNavigationService
               });
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };                   

                   //if (!$scope.entity.nombre) {
                   //    $scope.result.messages.push('Ingrese el nombre');
                   //}                  
               

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };

               //#endregion


               //#region filter

               $scope.filter = {
                   mediosAux: [],
                   medios: [],
                   plazasAux: [],
                   plazas: [],
                   vehiculosAux: [],
                   vehiculos: [],
                   diasAux: [],
                   dias: []
               };
               $scope.dias = ['Lunes', 'Martes', 'Miercoles', 'Juves', 'Viernes', 'Sabado', 'Domingo'];

               $scope.toggleCheck = function (entity, id) {
                   if ($scope.filter[entity + 'Aux'].indexOf(id) === -1) {
                       $scope.filter[entity + 'Aux'].push(id);
                   } else {
                       $scope.filter[entity + 'Aux'].splice($scope.filter[entity + 'Aux'].indexOf(id), 1);
                   }
               };

               $scope.setAux = function(entity) {
                   $scope.filter[entity + 'Aux'] = angular.copy($scope.filter[entity]);
               };

               $scope.applyFilter = function (entity) {
                   $scope.filter[entity] = angular.copy($scope.filter[entity + 'Aux']);
                   $scope.findTarifas();
               };

               $scope.clearFilter = function (entity, $event) {
                   $scope.filter[entity + 'Aux'] = [];

                   $event.stopPropagation();
               };

               $scope.setAuxHoras = function (entity) {
                   $scope.filter[entity + 'Aux'] = $scope.filter[entity];                   
               };

               $scope.applyFilterHoras = function (entity) {
                   $scope.filter[entity] = $scope.filter[entity + 'Aux'];                   
               };

               $scope.clearFilterHoras = function (entity, $event) {
                   $scope.filter[entity + 'Aux'] = null;

                   $event.stopPropagation();
               };

               //#endregion

               $scope.findTarifas = function() {
                   
               };
           }]);