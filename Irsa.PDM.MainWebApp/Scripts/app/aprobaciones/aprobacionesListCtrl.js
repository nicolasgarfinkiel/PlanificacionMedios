angular.module('irsa.pdm.aprobaciones.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'aprobacionesService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, aprobacionesService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;
               $scope.resultModal = { hasErrors: false, messages: [] };

               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: aprobacionesService,
                   navigation: baseNavigationService,
                   columns: [                       
                       { field: 'nombre', displayName: 'Campaña' },
                       { field: 'estado', displayName: 'Proveedor' },
                       { field: 'createDate', displayName: 'Fecha de alta', width: 100 },
                       { field: 'createDate', displayName: 'Estado consumo', width: 100 },
                       { field: 'createDate', displayName: 'Estado provisión', width: 100 },
                       { field: 'createDate', displayName: 'Estado certificacion', width: 100 },
                       { field: 'cuit', displayName: 'Aprobar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a ng-if="row.entity.estado == \'Pendiente\'" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="confirmAprobacion(row.entity)"><i class="fa fa-thumbs-o-up"></i></a></div>' }                       
                   ]
               });

               $scope.clearFilter = function () {
                   $scope.filter = {};
                   $scope.search();
               };

               //$scope.changeEstadoCampania = function () {
               //    $scope.resultModal = $scope.result = { hasErrors: false, messages: [] };
                 
               //    if (!$scope.isValidAprobacion()) {
               //        return;
               //    }

               //    campaniasService.changeEstadoCampania($scope.campania, 'Aprobada', $scope.motivo).then(function (response) {
               //        $scope.resultModal = $scope.result = response.data.result;

               //        if ($scope.resultModal.hasErrors) return;

               //        $scope.search();
               //        $('#aprobacionModal').modal('hide');
               //    }, function () { throw 'Error on changeEstadoCampania'; });
               //};

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