﻿angular.module('irsa.pdm.tarifarios.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           '$timeout',
           'tarifariosService',
           'tarifasService',
           'proveedoresService',
           'vehiculosService',
           'baseNavigationService',
           'listBootstraperService',
           'singleFileService',
           function ($scope, $timeout, tarifariosService, tarifasService, proveedoresService, vehiculosService, baseNavigationService, listBootstraperService, singleFileService) {
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
                       { field: 'estado', displayName: 'Documento', width: 120, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.documento" href="/documents/Tarifarios/{{row.entity.id}}/{{row.entity.documento}}" target="_blank" class="btn btn-rounded btn-xs btn-icon btn-default" ><i style="font-size: 11pt;" class="fa fa-file-pdf-o text-success"></i></a></div>' },
                       { field: 'cuit', displayName: 'Editar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.editable && usuario.roles.indexOf(\'tarifarios_edit\') >= 0" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="setEdit(row.entity)"><i class="fa fa-pencil"></i></a></div>' },
                       { field: 'cuit', displayName: 'Eliminar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.editable && usuario.roles.indexOf(\'tarifarios_delete\') >= 0" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="confirmBaja(row.entity)"><i class="fa fa-remove"></i></a></div>' },
                       { field: 'cuit', displayName: 'Admin', width: 70, cellTemplate: '<div class="ng-grid-icon-container">' + '<a title="Administrar" href="javascript:void(0)" ng-if="usuario.roles.indexOf(\'tarifarios_admin\') >= 0" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="navigationService.goToEdit(row.entity.id)" ><i ng-class="{\'fa fa-share-square-o\': row.entity.editable, \'fa fa-search\': !row.entity.editable }"></i></a></div>' },
                       { field: 'cuit', displayName: 'Aprobar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.estado == \'PendienteAprobacion\' && usuario.roles.indexOf(\'tarifarios_aprobacion\') >= 0" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="confirmAprobacion(row.entity)"><i class="fa fa-thumbs-o-up"></i></a></div>' }
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

                   singleFileService.setUploader({
                       returnDetailedFile: true,
                       onError: $scope.onErrorFileCallBack,
                       onSuccess: $scope.onSuccessFileCallBack,
                       controlId: 'fileupload',
                       urlFile: 'Tarifarios/UploadFile',
                       uploadProgressCallBack: $scope.uploadProgressCallBack
                   });

                   $('#tarifarioModal').modal('show');                 
               };

               $scope.setEdit = function (entity) {
                   $scope.loading = true;
                   $scope.resultModal.hasErrors = false;
                   $scope.operation = 'Edición';
                   $scope.entity = angular.copy(entity);

                   singleFileService.setUploader({
                       returnDetailedFile: true,
                       onError: $scope.onErrorFileCallBack,
                       onSuccess: $scope.onSuccessFileCallBack,
                       controlId: 'fileupload',
                       urlFile: 'Tarifarios/UploadFile',
                       uploadProgressCallBack: $scope.uploadProgressCallBack
                   });

                   $('#tarifarioModal').modal('show');
                   $timeout(function () { $scope.loading = false; }, 200);
               };

               $scope.confirmBaja = function (entity) {
                   $scope.operacion = 'Baja';
                   $scope.currentEntity = entity;
                   $('#modalConfirm').modal('show');
               };

               $scope.confirmAprobacion = function (entity) {
                   $scope.operacion = 'Aprobacion';
                   $scope.currentEntity = entity;
                   $('#modalConfirm').modal('show');
               };

               $scope.confirmOperacion = function () {
                   if ($scope.operacion == 'Baja') {
                       $scope.baja();
                   } else {
                       $scope.aprobar();
                   }                   
               };

               $scope.baja = function () {                   
                   $scope.result.hasErrors = false;

                   tarifariosService.deleteEntity($scope.currentEntity.id).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $scope.search();
                           $('#modalConfirm').modal('hide');
                           return;
                       }

                       $scope.result = response.data.result;
                   }, function () { throw 'Error on deleteEntity'; });
               };

               $scope.aprobar = function () {
                   $scope.result.hasErrors = false;

                   tarifariosService.aprobar($scope.currentEntity.id).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $scope.search();
                           $('#modalConfirm').modal('hide');
                           return;
                       }

                       $scope.result = response.data.result;
                   }, function () { throw 'Error on aprobar'; });
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

                   if (!$scope.entity.tipoOperacion) {
                       $scope.resultModal.messages.push('Seleccione el tipo de operación');
                   }

                   if (!$scope.entity.fechaDesde) {
                       $scope.resultModal.messages.push('Error al completar la fecha desde');
                   }

                   if (!$scope.entity.fechaHasta) {
                       $scope.resultModal.messages.push('Ingrese la fecha hasta');
                   }


                   if (!$scope.entity.numeroProveedorSap) {
                       $scope.resultModal.messages.push('Ingrese el número de medio SAP');
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

               $scope.$watch('entity.vehiculo', function (newValue) {
                   if (!newValue || $scope.loading) return;

                   $scope.getFechaDesde();
               });

               //#region TarifaProveedor

               $scope.openProveedoresModal = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $scope.tarifaProveedor = {};

                   singleFileService.setUploader({
                       returnDetailedFile: true,
                       onError: $scope.onErrorFileCallBack,
                       onSuccess: $scope.onSuccessFileCallBack,
                       controlId: 'fileuploadProveedor',
                       urlFile: 'Tarifarios/UploadFile',
                       uploadProgressCallBack: $scope.uploadProgressCallBack
                   });

                   $('#proveedoresModal').modal('show');
               };

               $scope.setTarifasProveedor = function() {
                   if (!$scope.isValidTarifaProveedor()) return;

                   tarifariosService.createTarifariosProveedor($scope.tarifaProveedor).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $('#proveedoresModal').modal('hide');                                                    
                           window.location.reload();

                           return;
                       }

                       $scope.resultModal = response.data.result;
                   }, function () { throw 'Error on getFechaDesde'; });                   
               };              

               $scope.getFechaDesdeProveedor = function () {
                   if (!$scope.tarifaProveedor || !$scope.tarifaProveedor.proveedor) return;

                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $scope.tarifaProveedor.fechaDesde = null;

                   tarifariosService.getFechaDesdeProveedor($scope.tarifaProveedor.proveedor.id).then(function (response) {
                       if (!response.data.result.hasErrors) {
                           $scope.tarifaProveedor.fechaDesde = response.data.data;
                           return;
                       }

                       $scope.resultModal = response.data.result;
                   }, function () { throw 'Error on getFechaDesdeProveedor'; });
               };

               $scope.isValidTarifaProveedor = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };

                   if (!$scope.tarifaProveedor.proveedor) {
                       $scope.resultModal.messages.push('Seleccione un proveedor');
                   }

                   if (!$scope.tarifaProveedor.tipoOperacion) {
                       $scope.resultModal.messages.push('Seleccione el tipo de operación');
                   }

                   if (!$scope.tarifaProveedor.fechaDesde) {
                       $scope.resultModal.messages.push('Error al completar la fecha desde');
                   }

                   if (!$scope.tarifaProveedor.fechaHasta) {
                       $scope.resultModal.messages.push('Ingrese la fecha hasta');
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

               $scope.$watch('tarifaProveedor.proveedor', function (newValue) {
                   if (!newValue || $scope.loading) return;

                   $scope.getFechaDesdeProveedor();
               });

               //#endregion

               //#region Documento

               $scope.onErrorFileCallBack = function (message) {
                   $scope.$apply(function () {
                       $scope.resultModal.hasErrors = true;
                       $scope.resultModal.messages = [message || 'Se produjo un error al intentar subir el archivo'];
                   });
               };

               $scope.onSuccessFileCallBack = function (file) {
                   $scope.$apply(function () {
                       $scope.resultModal.hasErrors = false;

                       if ($scope.entity) {
                           $scope.entity.documento = file.name;
                       }

                       if ($scope.tarifaProveedor) {
                           $scope.tarifaProveedor.documento = file.name;
                       }
                       
                       $("#progressBar").width('0%');
                   });
               };

               $scope.uploadProgressCallBack = function (percent) {                   
                   $("#progressBar").width(percent + '%');
               };              

               //#endregion

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