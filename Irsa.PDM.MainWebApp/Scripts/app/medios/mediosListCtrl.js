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
                       { field: 'nombre', displayName: 'Apellido Nombre / Descripción', cellTemplate: '<div class="" style="padding: 4px;">{{row.entity.apellido}} {{row.entity.nombre}}</div>' },
                       { field: 'cuit', displayName: 'Cuit', width: 100 },
                       { field: 'esChoferTransportista ? "Si" : "No" ', displayName: 'Transportista', width: 110 },
                       { field: 'createDate', displayName: 'Fecha creación', width: 120 },
                       { field: 'createdBy', displayName: 'Usuario creación' },
                       { field: 'cuit', displayName: 'Acciones', width: 80, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="edit(row.entity.id)"><i class="fa fa-pencil"></i></a></div>' }
                   ]
               });
           }]);