angular.module('irsa.pdm.vehiculos.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'vehiculosService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, vehiculosService, baseNavigationService, listBootstraperService) {
               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: vehiculosService,
                   navigation: baseNavigationService,
                   columns: [
                       { field: 'codigo', displayName: 'Código', width: 60 },
                       { field: 'nombre', displayName: 'Nombre'},                                              
                       { field: 'cuit', displayName: 'Acciones', width: 80, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="edit(row.entity.id)" ng-if="usuario.roles.indexOf(\'vehiculos_edit\') >= 0"><i class="fa fa-pencil"></i></a></div>' }
                   ]
               });
           }]);