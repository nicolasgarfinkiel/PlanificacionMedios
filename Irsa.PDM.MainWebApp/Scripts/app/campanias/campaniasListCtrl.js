angular.module('irsa.pdm.campanias.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'campaniasService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, campaniasService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;
               $scope.resultModal = { hasErrors: false, messages: [] };

               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: campaniasService,
                   navigation: baseNavigationService,
                   columns: [                       
                       { field: 'nombre', displayName: 'Nombre' },
                       { field: 'estado', displayName: 'Estado' },
                       { field: 'createDate', displayName: 'Fecha de alta', width: 100 },
                       { field: 'cuit', displayName: 'Aprobar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.estado == \'Pendiente\'" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="confirmAprobacion(row.entity)"><i class="fa fa-thumbs-o-up"></i></a></div>' },
                       { field: 'cuit', displayName: 'Administrar', width: 100, cellTemplate: '<div class="ng-grid-icon-container">' + '<a title="Administrar" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="navigationService.goToEdit(row.entity.id)" ><i ng-class="{\'fa fa-share-square-o\': row.entity.estado == \'Pendiente\', \'fa fa-search\': row.entity.estado != \'Pendiente\' }"></i></a></div>' }
                   ]
               });

               $scope.clearFilter = function () {
                   $scope.filter = {};
                   $scope.search();
               };

               $scope.changeEstadoCampania = function () {
                   $scope.resultModal = $scope.result = { hasErrors: false, messages: [] };
                 
                   if (!$scope.isValidAprobacion()) {
                       return;
                   }

                   campaniasService.changeEstadoCampania($scope.campania, 'Aprobada', $scope.motivo).then(function (response) {
                       $scope.resultModal = $scope.result = response.data.result;

                       if ($scope.resultModal.hasErrors) return;

                       $scope.search();
                       $('#aprobacionModal').modal('hide');
                   }, function () { throw 'Error on changeEstadoCampania'; });
               };

               $scope.confirmAprobacion = function(campania) {
                   $scope.campania = campania;
                   $scope.campania.idSapDistribucion = null;
                   $scope.campania.centro = null;
                   $scope.campania.almacen = null;
                   $scope.campania.orden = null;                   
                   $scope.campania.centroDestino = null;                   
                   $scope.campania.almacenDestino = null;

                   $('#aprobacionModal').modal('show');
               };

               $scope.isValidAprobacion = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };

                   if (!$scope.campania.centro) {
                       $scope.resultModal.messages.push('Ingrese el id centro');
                   }

                   if (!$scope.campania.almacen) {
                       $scope.resultModal.messages.push('Ingrese el id almacen');
                   }                 

                   $scope.resultModal.hasErrors = $scope.resultModal.messages.length;
                   return !$scope.resultModal.hasErrors;
               };

           }]);