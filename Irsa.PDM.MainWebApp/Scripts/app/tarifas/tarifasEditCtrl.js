angular.module('irsa.pdm.tarifas.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'tarifasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, tarifasService, baseNavigationService, editBootstraperService) {
               $scope.dias = ['Lunes', 'Martes', 'Miercoles', 'Juves', 'Viernes', 'Sabado', 'Domingo'];
               $scope.tarifaInit = { lunes: true, martes: true, miercoles: true, jueves: true, viernes: true, sabado: true, domingo: true };
               $scope.tarifas = [angular.copy($scope.tarifaInit)];

               //#region base

               $scope.onInitEnd = function () {
               };

               editBootstraperService.init($scope, $routeParams, {
                   service: tarifasService,
                   navigation: baseNavigationService
               });

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

               $scope.toggleCheck = function (entity, id) {
                   if ($scope.filter[entity + 'Aux'].indexOf(id) === -1) {
                       $scope.filter[entity + 'Aux'].push(id);
                   } else {
                       $scope.filter[entity + 'Aux'].splice($scope.filter[entity + 'Aux'].indexOf(id), 1);
                   }
               };

               $scope.setAux = function (entity) {
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

               $scope.findTarifas = function () {

               };

               $scope.addTarifa = function (current) {
                   $scope.validateAndSave(function() {
                       var index = $scope.tarifas.indexOf(current);
                       var tarifa = angular.copy($scope.tarifaInit);
                       $scope.tarifas.splice(index + 1, 0, tarifa);
                       $scope.currentTarifa = null;
                       $scope.setEditable(tarifa);
                   });                   
               };

               $scope.deleteTarifa = function (tarifa) {
                   if (tarifa == $scope.currentTarifa) {
                       var index = $scope.tarifas.indexOf(tarifa);
                       $scope.tarifas.splice(index, 1);
                       $scope.currentTarifa = null;
                   } else {
                       $scope.validateAndSave(function () {
                           var index = $scope.tarifas.indexOf(tarifa);
                           $scope.tarifas.splice(index, 1);
                           $scope.currentTarifa = null;
                       });
                   }                   
               };

               $scope.setEditable = function (tarifa) {                   
                   $scope.validateAndSave(function () {                                          
                       if (tarifa == $scope.currentTarifa) {
                           $scope.currentTarifa = null;
                           return;
                       }

                       $scope.currentTarifa = tarifa;
                       $scope.currentTarifa.editable = !$scope.currentTarifa.editable;                       
                   });                   
               }

               $scope.validateAndSave = function (callback) {
                   if ($scope.currentTarifa == null) {
                       callback();
                       return;
                   }

                   if (!$scope.isValidTarifa($scope.currentTarifa)) return;

                   //TODO: save
                   $scope.currentTarifa.editable = false;
                 //  $scope.currentTarifa = null;
                   callback();
               }

               $scope.isValidTarifa = function (tarifa) {
                   tarifa.invalidMedio = !tarifa.medio;
                   tarifa.invalidPlaza = !tarifa.plaza;
                   tarifa.invalidVehiculo = !tarifa.vehiculo;
                   tarifa.invalidDias = !tarifa.lunes && !tarifa.martes && !tarifa.miercoles && !tarifa.jueves && !tarifa.viernes && !tarifa.sabado && !tarifa.domingo;
                   tarifa.invalidHoraDesde = !tarifa.horaDesde;
                   tarifa.invalidHoraHasta = !tarifa.horaHasta;
                   tarifa.invalidTarifa = !tarifa.importe || isNaN(tarifa.importe);
                   tarifa.invalidDescripcion = !tarifa.descripcion;

                   return !(tarifa.invalidMedio ||
                       tarifa.invalidPlaza ||
                       tarifa.invalidVehiculo ||
                       tarifa.invalidDias ||
                       tarifa.invalidHoraDesde ||
                       tarifa.invalidHoraHasta ||
                       tarifa.invalidTarifa ||
                       tarifa.invalidDescripcion);
               }
             
           }]);