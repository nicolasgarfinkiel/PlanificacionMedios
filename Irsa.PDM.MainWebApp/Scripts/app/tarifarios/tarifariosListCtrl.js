angular.module('irsa.pdm.tarifarios.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           '$timeout',
           'tarifariosService',
           'tarifasService',
           'proveedoresService',
           'vehiculosService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, $timeout, tarifariosService, tarifasService, proveedoresService, vehiculosService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;

               //#region Base

               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: tarifariosService,
                   navigation: baseNavigationService,
                   columns: [                       
                       { field: 'id', displayName: 'Id', width: 50 },
                       { field: 'vehiculo.nombre', displayName: 'Vehículo' },
                       { field: 'fechaDesde', displayName: 'Fecha desde' },
                       { field: 'fechaHasta', displayName: 'Fecha hasta' },
                       { field: 'estado', displayName: 'Estado' },
                       { field: 'cuit', displayName: 'Editar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.estado == \'Editable\'" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="setEdit(row.entity)"><i class="fa fa-pencil"></i></a></div>' },
                       { field: 'cuit', displayName: 'Eliminar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.estado == \'Editable\'" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="remove(row.entity)"><i class="fa fa-remove"></i></a></div>' },
                       { field: 'cuit', displayName: 'Admin', width: 70, cellTemplate: '<div class="ng-grid-icon-container">' +'<a title="Administrar" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="navigationService.goToEdit(row.entity.id)" ><i ng-class="{\'fa fa-share-square-o\': row.entity.estado == \'Editable\', \'fa fa-search\': row.entity.estado != \'Editable\' }"></i></a></div>'}
                   ]
               });
               
               //#endregion

               $scope.resultModal = { hasErrors: false, messages: [] };

               $scope.clearFilter = function() {
                   $scope.filter = {};
                   $scope.search();
               };

               $scope.setCreate = function () {                   
                   $scope.resultModal.hasErrors = false;
                   $scope.operation = 'Alta';
                   $scope.entity = {};
                   $('#tarifarioModal').modal('show');
                 //  $scope.getFechaDesde();                   
               };

               $scope.setEdit = function (entity) {
                   $scope.loading = true;
                   $scope.resultModal.hasErrors = false;
                   $scope.operation = 'Edición';
                   $scope.entity = angular.copy(entity);                  
                   $('#tarifarioModal').modal('show');
                   $timeout(function () { $scope.loading = false; }, 200);
               };

               $scope.remove = function (entity) {
                   if (!confirm('Desea llevar a cabo la operación?')) return;
                   $scope.result.hasErrors = false;
                                      
                   tarifariosService.deleteEntity(entity.id).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $scope.search();                           
                           return;
                       }

                       $scope.result = response.data.result;
                   }, function () { throw 'Error on deleteEntity'; });
               };

               $scope.save = function () {
                   if (!$scope.isValid()) return;

                   if ($scope.entity.id) {
                       tarifariosService.updateEntity($scope.entity).then(function (response) {
                           if (!response.data.result.hasErrors) {
                               $scope.search();
                               $('#tarifarioModal').modal('hide');
                               return;
                           }

                           $scope.resultModal = response.data.result;
                       }, function () { throw 'Error on updateUsuarioCliente'; });

                   } else {                       
                       tarifariosService.createEntity($scope.entity).then(function (response) {
                           if (!response.data.result.hasErrors) {
                               $scope.search();                               
                               $('#tarifarioModal').modal('hide');
                               return;
                           }

                           $scope.resultModal = response.data.result;
                       }, function () { throw 'Error on createCliente'; });
                   }
               };

               $scope.isValid = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };

                   if (!$scope.entity.vehiculo) {
                       $scope.resultModal.messages.push('Seleccione un vehículo');
                   }

                   if (!$scope.entity.fechaDesde) {
                       $scope.resultModal.messages.push('Error al completar la fecha desde');
                   }

                   if (!$scope.entity.fechaHasta) {
                       $scope.resultModal.messages.push('Ingrese la fecha hasta');
                   }                                      

                   $scope.resultModal.hasErrors = $scope.resultModal.messages.length;
                   return !$scope.resultModal.hasErrors;
               };

               $scope.getFechaDesde = function () {
                   if (!$scope.entity.vehiculo.id) return;

                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $scope.entity.fechaDesde = null;
                 
                   tarifariosService.getFechaDesde($scope.entity.vehiculo.id).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $scope.entity.fechaDesde = response.data.data;                           
                           return;
                       }

                       $scope.resultModal = response.data.result;
                   }, function () { throw 'Error on getFechaDesde'; });
               };

               $scope.openProveedoresModal = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $scope.tarifaProveedor = {};
                   $('#proveedoresModal').modal('show');
               };

               $scope.setTarifasProveedor = function() {
                   if (!$scope.isValidTarifaProveedor()) return;

                   tarifasService.setValuesByProveedor($scope.tarifaProveedor).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $('#proveedoresModal').modal('hide');

                           toastr.options = {
                               "debug": false,
                               "newestOnTop": false,                               
                               "closeButton": true,                               
                           };

                           toastr.info(response.data.data);

                           return;
                       }

                       $scope.resultModal = response.data.result;
                   }, function () { throw 'Error on getFechaDesde'; });
                   
               };

               $scope.isValidTarifaProveedor = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };

                   if (!$scope.tarifaProveedor.proveedor) {
                       $scope.resultModal.messages.push('Seleccione un proveedor');
                   }

                   if (!$scope.tarifaProveedor.importe) {
                       $scope.resultModal.messages.push('Ingrese el importe');
                   }

                   if ($scope.tarifaProveedor.importe && isNaN($scope.tarifaProveedor.importe)) {
                       $scope.resultModal.messages.push('El importe debe ser numérico');
                   }

                   $scope.resultModal.hasErrors = $scope.resultModal.messages.length;
                   return !$scope.resultModal.hasErrors;
                   
               };

               $scope.$watch('entity.vehiculo', function (newValue) {
                   if (!newValue || $scope.loading) return;

                   $scope.getFechaDesde();
               });

               //#region Select UI Vehiculos

               $scope.selectList = [];
               $scope.currentPage = 0;
               $scope.pageCount = 0;
               $scope.filterVehiculos = { pageSize: 20 };

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

                   $scope.filterVehiculos.currentPage = $scope.currentPage;
                   $scope.filterVehiculos.multiColumnSearchText = $select.search;

                   vehiculosService.getByFilter($scope.filterVehiculos).then(function (response) {
                       $scope.selectList = $scope.selectList.concat(response.data.data);
                       $scope.pageCount = Math.ceil(response.data.count / 20);
                   }, function () { throw 'Error on getByFilter'; });
               };

               //#endregion         

               //#region Select UI Proveedores

               $scope.filterProveedores = { pageSize: 20 };

               $scope.getSelectSourceProveedor = function ($select, $event) {
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

                   $scope.filterProveedores.currentPage = $scope.currentPage;
                   $scope.filterProveedores.multiColumnSearchText = $select.search;

                   proveedoresService.getByFilter($scope.filterProveedores).then(function (response) {
                       $scope.selectList = $scope.selectList.concat(response.data.data);
                       $scope.pageCount = Math.ceil(response.data.count / 20);
                   }, function () { throw 'Error on getByFilter'; });
               };

               //#endregion     
           }]);