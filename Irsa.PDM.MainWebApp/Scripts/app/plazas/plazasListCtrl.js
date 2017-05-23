angular.module('irsa.pdm.plazas.ctrl.list', [])
       .controller('listCtrl', [
           '$scope',
           'plazasService',
           'baseNavigationService',
           'listBootstraperService',
           function ($scope, plazasService, baseNavigationService, listBootstraperService) {
               $scope.onInitEnd = function () {                          
               };

               listBootstraperService.init($scope, {
                   service: plazasService,
                   navigation: baseNavigationService,
                   columns: [                       
                       { field: 'nombre', displayName: 'Nombre'},
                       { field: 'descripcion', displayName: 'Descripción' },                       
                       { field: 'cuit', displayName: 'Acciones', width: 80, cellTemplate: '<div class="ng-grid-icon-container"><a href="javascript:void(0)" class="btn btn-rounded btn-xs btn-icon btn-default" ng-click="edit(row.entity.id)"><i class="fa fa-pencil"></i></a></div>' }
                   ]
               });

               $scope.export = function() {
                   html2canvas(document.getElementById('pepe'), {
                       onrendered: function (canvas) {
                           var data = canvas.toDataURL();
                           var docDefinition = {
                               content: [{
                                   image: data,
                                   width: 500,
                               }]
                           };
                           pdfMake.createPdf(docDefinition).download("pepe.pdf");
                       }
                   });
               };
           }]);