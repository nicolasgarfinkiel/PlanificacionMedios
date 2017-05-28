angular.module('irsa.pdm.medios.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'mediosService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, mediosService, baseNavigationService, listBootstraperService) {
               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: mediosService,
                   navigation: baseNavigationService,
                   columns: [
                       { field: 'codigo', displayName: 'Código', width: 60 },
                       { field: 'nombre', displayName: 'Nombre'},                       
                       { field: 'cuit', displayName: 'Acciones', width: 80, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="edit(row.entity.id)"><i class="fa fa-pencil"></i></a></div>' }
                   ]
               });
           }]);