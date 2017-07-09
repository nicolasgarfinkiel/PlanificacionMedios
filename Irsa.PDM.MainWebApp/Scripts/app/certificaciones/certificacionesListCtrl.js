angular.module('irsa.pdm.certificaciones.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'certificacionesService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, certificacionesService, baseNavigationService, listBootstraperService) {
               $scope.navigationService = baseNavigationService;

               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: certificacionesService,
                   navigation: baseNavigationService,
                   columns: [                       
                       { field: 'nombre', displayName: 'Nombre' },
                       { field: 'estado', displayName: 'Estado' },
                       { field: 'createDate', displayName: 'Fecha de alta', width: 100 },
                       { field: 'cuit', displayName: 'Administrar', width: 100, cellTemplate: '<div class="ng-grid-icon-container">' + '<a title="Administrar" href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="navigationService.goToEdit(row.entity.id)" ><i ng-class="{\'fa fa-share-square-o\': row.entity.estado == \'Pendiente\', \'fa fa-search\': row.entity.estado != \'Pendiente\' }"></i></a></div>' }
                   ]
               });


           }]);