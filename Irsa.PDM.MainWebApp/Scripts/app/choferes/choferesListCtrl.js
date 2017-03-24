angular.module('cresud.cdp.choferes.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'choferesService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, choferesService, baseNavigationService, listBootstraperService) {
               $scope.onInitEnd = function () {
                   var displayName = null;

                   switch ($scope.usuario.currentEmpresa.grupoEmpresa.paisDescripcion.toUpperCase()) {
                       case 'PARAGUAY':
                           displayName = 'RUC';
                           break;
                       case 'BOLIVIA':
                           displayName = 'NIT';
                           break;
                       default:
                           displayName = 'CUIT';
                   }

                   $scope.columns[1].displayName = displayName;                   
               };

               listBootstraperService.init($scope, {
                   service: choferesService,
                   navigation: baseNavigationService,
                   columns: [
                       { field: 'nombre', displayName: 'Apellido Nombre / Descripción', cellTemplate: '<div class="" style="padding: 4px;">{{row.entity.apellido}} {{row.entity.nombre}}</div>' },
                       { field: 'cuit', displayName: 'Cuit', width: 100 },
                       { field: 'esChoferTransportista ? "Si" : "No" ', displayName: 'Transportista', width: 110 },
                       { field: 'createDate', displayName: 'Fecha creación', width: 120 },
                       { field: 'createdBy', displayName: 'Usuario creación' },
                       { field: 'cuit', displayName: 'Acciones', width: 80, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="edit(row.entity.id)"><i class="fa fa-pencil"></i></a></div>' }
                   ]
               });
           }]);