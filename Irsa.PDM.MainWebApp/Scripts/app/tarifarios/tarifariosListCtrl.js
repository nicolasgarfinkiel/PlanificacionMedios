angular.module('irsa.pdm.tarifarios.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'tarifariosService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, tarifariosService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;

               //#region Base

               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: tarifariosService,
                   navigation: baseNavigationService,
                   columns: [                       
                       { field: 'id', displayName: 'Id', width: 50 },
                       { field: 'fechaDesde', displayName: 'Fecha desde' },
                       { field: 'fechaHasta', displayName: 'Fecha hasta' },
                       { field: 'cuit', displayName: 'Editar', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="setEdit(row.entity)"><i class="fa fa-pencil"></i></a></div>' },
                       { field: 'cuit', displayName: 'Admin', width: 70, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="navigationService.goToEdit(row.entity.id)"><i class="fa fa-share-square-o"></i></a></div>' }
                   ]
               });
               
               //#endregion

               $scope.resultModal = { hasErrors: false, messages: [] };

               $scope.setCreate = function () {
                   $scope.resultModal.hasErrors = false;
                   $scope.operation = 'Alta';
                   $scope.entity = {};
                   $('#tarifarioModal').modal('show');
               };

               $scope.setEdit = function (entity) {                   
                   $scope.resultModal.hasErrors = false;
                   $scope.operation = 'Edición';
                   $scope.entity = angular.copy(entity);                  
                   $('#tarifarioModal').modal('show');
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

                   if (!$scope.entity.fechaDesde) {
                       $scope.resultModal.messages.push('Ingrese la fecha desde');
                   }

                   if (!$scope.entity.fechaHasta) {
                       $scope.resultModal.messages.push('Ingrese la fecha hasta');
                   }                                      

                   $scope.resultModal.hasErrors = $scope.resultModal.messages.length;
                   return !$scope.resultModal.hasErrors;
               };
           }]);