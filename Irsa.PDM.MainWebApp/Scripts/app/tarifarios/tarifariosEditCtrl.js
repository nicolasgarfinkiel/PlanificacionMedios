angular.module('irsa.pdm.tarifarios.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'tarifariosService',
           'tarifasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, tarifariosService, tarifasService, baseNavigationService, editBootstraperService) {
               $scope.dias = ['Lunes', 'Martes', 'Miercoles', 'Juves', 'Viernes', 'Sabado', 'Domingo'];
               $scope.tarifaInit = { idTarifario: $routeParams.id, lunes: true, martes: true, miercoles: true, jueves: true, viernes: true, sabado: true, domingo: true };
               $scope.tarifas = [];               

               //#region base

               $scope.onInitEnd = function () {
                   $scope.findTarifas();
               };

               editBootstraperService.init($scope, $routeParams, {
                   service: tarifariosService,
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
                   dias: [],
                   currentPage: 1,
                   pageSize: 50,
                   tarifarioId: $routeParams.id

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
                   $scope.findTarifas();
               };

               $scope.clearFilterHoras = function (entity, $event) {
                   $scope.filter[entity + 'Aux'] = null;

                   $event.stopPropagation();
               };

               $scope.getById = function (entity, id) {
                   var result = null;

                   for (var i = 0; i < $scope.data[entity].length; i++) {
                       if ($scope.data[entity][i].id == id) {
                           result = $scope.data[entity][i];
                           break;
                       }
                   }

                   return result;
               };

               $scope.$watch('filter.multiColumnSearchText', function (newValue, oldValue) {
                   if (typeof newValue == 'undefined' || newValue == oldValue) return;
                   $scope.findTarifas();
               });

               //#endregion                      

               $scope.findTarifas = function () {
                   tarifasService.getByFilter($scope.filter).then(function (response) {
                       $scope.tarifas = response.data.data.length ? response.data.data : [angular.copy($scope.tarifaInit)];
                   });
               };

               $scope.addTarifa = function (current) {
                   $scope.validateAndSave(function () {
                       var index = $scope.tarifas.indexOf(current);
                       var tarifa = angular.copy($scope.tarifaInit);
                       $scope.tarifas.splice(index + 1, 0, tarifa);
                       $scope.currentTarifa = null;
                       $scope.setEditable(tarifa);
                   });
               };

               $scope.deleteTarifa = function (tarifa) {
                   tarifasService.deleteEntity(tarifa.id).then(function (response) {
                       $scope.result = response.data.result;

                       var index = $scope.tarifas.indexOf(tarifa);
                       $scope.tarifas.splice(index, 1);

                       if (tarifa == $scope.currentTarifa) {
                           $scope.currentTarifa = null;
                       }

                   }, function () { throw 'Error on deleteEntity'; });
               };

               $scope.setEditable = function (tarifa) {
                   tarifa.edited = false;
                   $scope.validateAndSave(function () {
                       if (tarifa == $scope.currentTarifa) {
                           $scope.currentTarifa = null;
                           return;
                       }

                       $scope.currentTarifa = tarifa;
                       $scope.currentTarifa.editable = !$scope.currentTarifa.editable;

                       //if ($scope.currentTarifa.medio) {
                       //    $scope.currentTarifa.medio = $scope.getById('medios', $scope.currentTarifa.medio.id);
                       //    $scope.currentTarifa.plaza = $scope.getById('plazas', $scope.currentTarifa.plaza.id);
                       //    $scope.currentTarifa.vehiculo = $scope.getById('vehiculos', $scope.currentTarifa.vehiculo.id);
                       //}
                   });
               };

               $scope.validateAndSave = function (callback) {
                   if ($scope.currentTarifa == null) {
                       callback();
                       return;
                   }

                   if (!$scope.isValidTarifa($scope.currentTarifa)) return;

                   if ($scope.currentTarifa.id) {
                       tarifasService.updateEntity($scope.currentTarifa).then(function (response) {
                           $scope.result = response.data.result;
                           $scope.currentTarifa.editable = false;
                           callback();
                       }, function () { throw 'Error on update'; });
                   } else {
                       tarifasService.createEntity($scope.currentTarifa).then(function (response) {
                           $scope.currentTarifa.id = response.data.data.id;
                           $scope.result = response.data.result;
                           $scope.currentTarifa.editable = false;
                           callback();
                       }, function () { throw 'Error on create'; });
                   }
               };

               $scope.isValidTarifa = function (tarifa) {
                   //tarifa.invalidMedio = !tarifa.medio;
                   //tarifa.invalidPlaza = !tarifa.plaza;
                   //tarifa.invalidVehiculo = !tarifa.vehiculo;
                   //tarifa.invalidDias = !tarifa.lunes && !tarifa.martes && !tarifa.miercoles && !tarifa.jueves && !tarifa.viernes && !tarifa.sabado && !tarifa.domingo;
                   //tarifa.invalidHoraDesde = tarifa.horaDesde === null || tarifa.horaDesde === '' || typeof tarifa.horaDesde == 'undefined';
                   //tarifa.invalidHoraHasta = tarifa.horaHasta === null || tarifa.horaHasta === '' || typeof tarifa.horaHasta == 'undefined';
                   //tarifa.invalidDescripcion = !tarifa.descripcion;
                   tarifa.invalidTarifa = !tarifa.importe || isNaN(tarifa.importe);
                 

                   return !(tarifa.invalidTarifa);
               };

               $scope.filterVehiculos = function (medio) {
                   $scope.vehiculosFiltered = [];

                   if ($scope.currentTarifa.vehiculo && $scope.currentTarifa.vehiculo.medio.id != medio.id) {
                       $scope.currentTarifa.vehiculo = null;
                   }

                   if (!medio) return;

                   $scope.data.vehiculos.forEach(function (vehiculo) {
                       if (vehiculo.medio.id == medio.id) {
                           $scope.vehiculosFiltered.push(vehiculo);
                       }
                   });
               };

               $scope.setStaticValues = function (medio) {
                   if (!medio || medio.tipoEspacio != 'Estatico') return;

                   $scope.currentTarifa.lunes =
                   $scope.currentTarifa.martes =
                   $scope.currentTarifa.miercoles =
                   $scope.currentTarifa.jueves =
                   $scope.currentTarifa.viernes =
                   $scope.currentTarifa.sabado =
                   $scope.currentTarifa.domingo = true;

                   $scope.currentTarifa.horaDesde = 0;
                   $scope.currentTarifa.horaHasta = 2359;

               };

               $scope.$watch('currentTarifa.medio', function (newValue, oldValue) {
                   if (!$scope.currentTarifa) return;
                   $scope.filterVehiculos(newValue);
                   $scope.setStaticValues(newValue);

               });
           }]);