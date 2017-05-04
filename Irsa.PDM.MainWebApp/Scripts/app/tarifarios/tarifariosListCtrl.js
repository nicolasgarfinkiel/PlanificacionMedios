angular.module('irsa.pdm.tarifarios.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'tarifariosService',
           'vehiculosService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, tarifariosService, vehiculosService, baseNavigationService, listBootstraperService) {
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
                   //   { field: 'cuit', displayName: 'Editar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="setEdit(row.entity)"><i class="fa fa-pencil"></i></a></div>' },
                       { field: 'cuit', displayName: 'Admin', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="navigationService.goToEdit(row.entity.id)" ><i class="fa fa-share-square-o"></i></a></div>' }
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
                   $scope.loading = false;
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
                   $scope.resultModal.hasErrors = false;
                   $scope.resultModal.messages = [];                   

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
                   $scope.resultModal.hasErrors = false;
                   $scope.resultModal.messages = [];
                   $scope.entity.fechaDesde = null;
                 
                   tarifariosService.getFechaDesde($scope.entity.vehiculo.id).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $scope.entity.fechaDesde = response.data.data;                           
                           return;
                       }

                       $scope.resultModal = response.data.result;
                   }, function () { throw 'Error on getFechaDesde'; });
               };

               $scope.$watch('entity.vehiculo', function (newValue) {
                   if (!newValue || $scope.loading) return;

                   $scope.getFechaDesde();
               });

               //#region Select UI

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
           }]);