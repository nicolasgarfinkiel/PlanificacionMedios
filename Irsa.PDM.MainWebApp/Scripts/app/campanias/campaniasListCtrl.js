angular.module('irsa.pdm.campanias.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'campaniasService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, campaniasService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;

               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: campaniasService,
                   navigation: baseNavigationService,
                   columns: [                       
                       { field: 'nombre', displayName: 'Nombre' },
                       { field: 'estado', displayName: 'Estado' },
                       { field: 'createDate', displayName: 'Fecha de alta', width: 100 },
                       { field: 'cuit', displayName: 'Administrar', width: 100, cellTemplate: '<div class="ng-grid-icon-container">' + '<a title="Administrar" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="navigationService.goToEdit(row.entity.id)" ><i ng-class="{\'fa fa-share-square-o\': row.entity.estado == \'Pendiente\', \'fa fa-search\': row.entity.estado != \'Pendiente\' }"></i></a></div>' }
                   ]
               });


               $scope.clearFilter = function () {
                   $scope.filter = {};
                   $scope.search();
               };

           }]);