angular.module('irsa.pdm.aprobaciones.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           '$timeout',
           'aprobacionesService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, $timeout, aprobacionesService, baseNavigationService, editBootstraperService) {
               $scope.resultModal = { hasErrors: false, messages: [] };
               $scope.nav = baseNavigationService;

               $scope.onInitEnd = function () {
                              
               };

               $scope.gridOptions = {
                   data: 'data.aprobacionesPendientes',
                   columnDefs: [                      
                       { field: 'campaniaNombre', displayName: 'Campaña' },
                       { field: 'proveedorNombre', displayName: 'Proveedor' },                       
                       { field: 'montoTotal', displayName: 'Monto total', cellFilter: 'currency', width: 180 },
                       { field: 'cuit', displayName: 'Detalle', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.estado == \'Pendiente\'" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="confirmAprobacion(row.entity)"><i class="fa fa-thumbs-o-up"></i></a></div>' }
                   ],
                   showFooter: false,
                   useExternalSorting: false,
                   useExternalPagination: false,
                   enablePaging: false,
                   enableRowSelection: false,
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: aprobacionesService,
                   navigation: baseNavigationService
               });
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };                   
                                                 
                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };

               $scope.confirmAprobacion = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $('#modalConfirm').modal('show');
               };

               $scope.aprobar = function () {
                   aprobacionesService.createEntity({}).then(function (response) {
                       $scope.resultModal = response.data.result;

                       if ($scope.resultModal.hasErrors) return;

                       $('#modalConfirm').modal('hide');

                       $timeout(function() {
                           baseNavigationService.goToList();
                       }, 500);
                   });                   
               };

           }]);