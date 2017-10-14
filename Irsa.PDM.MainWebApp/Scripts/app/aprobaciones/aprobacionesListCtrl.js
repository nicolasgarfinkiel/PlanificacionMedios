angular.module('irsa.pdm.aprobaciones.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'aprobacionesService',
           'campaniasService',
           'proveedoresService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, aprobacionesService, campaniasService, proveedoresService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;
               $scope.resultModal = { hasErrors: false, messages: [] };

               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: aprobacionesService,
                   navigation: baseNavigationService,
                   columns: [
                       { field: 'id', displayName: 'Id', width: 50 },
                       { field: 'campaniaNombre', displayName: 'Campaña' },
                       { field: 'proveedorNombre', displayName: 'Proveedor' },
                       { field: 'createDate', displayName: 'Fecha de alta', width: 100 },
                       { field: 'montoTotal', displayName: 'Monto total', width: 180 },
                       { field: 'estadoConsumo', displayName: 'Estado consumo', width: 160 },
                       { field: 'estadoProvision', displayName: 'Estado provisión', width: 160 },
                       { field: 'estadoCertificacion', displayName: 'Estado certificacion', width: 160 },
                       { field: 'cuit', displayName: 'Detalle', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.estado == \'Pendiente\'" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="confirmAprobacion(row.entity)"><i class="fa fa-thumbs-o-up"></i></a></div>' }                       
                   ]
               });

               $scope.clearFilter = function () {
                   $scope.filter = {};
                   $scope.search();
               };            

               //#region Select UI

               $scope.sources = {                                    
                   'campania': {
                       service: campaniasService,
                       method: 'getByFilter',
                       filter: { pageSize: 20 },
                   },
                   'proveedor': {
                       service: proveedoresService,
                       method: 'getByFilter',
                       filter: { pageSize: 20 },
                   }
               };

               $scope.selectList = [];
               $scope.currentPage = 0;
               $scope.pageCount = 0;

               $scope.getSelectSource = function ($select, $event) {
                   if ($scope.loading) return;

                   var source = $scope.sources[$select.$element.attr('name')];

                   if (!$event) {
                       $scope.currentPage = 1;
                       $scope.pageCount = 0;
                       $scope.selectList = [];
                   } else {
                       $event.stopPropagation();
                       $event.preventDefault();
                       $scope.currentPage++;
                   }

                   source.filter.currentPage = $scope.currentPage;
                   source.filter.multiColumnSearchText = $select.search;

                   source.service[source.method](source.filter).then(function (response) {
                       $scope.selectList = $scope.selectList.concat(response.data.data);
                       $scope.pageCount = Math.ceil(response.data.count / 20);
                   }, function () { throw 'Error on getSelectByFilter'; });
               };

               //#endregion
           }]);