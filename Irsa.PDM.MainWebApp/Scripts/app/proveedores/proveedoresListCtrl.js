angular.module('irsa.pdm.proveedores.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'proveedoresService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, proveedoresService, baseNavigationService, listBootstraperService) {
               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: proveedoresService,
                   navigation: baseNavigationService,
                   columns: [
                       { field: 'id', displayName: 'Código', width: 60 },
                       { field: 'nombre', displayName: 'Nombre' },
                       { field: 'numeroProveedorSap', displayName: 'Nro.Prov.SAP' },                       
                                              { field: 'cuit', displayName: 'Acciones', width: 80, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="edit(row.entity.id)" ng-if="usuario.roles.indexOf(\'proveedores_edit\') >= 0"><i class="fa fa-pencil"></i></a></div>' }
                   ]
               });
             
           }]);