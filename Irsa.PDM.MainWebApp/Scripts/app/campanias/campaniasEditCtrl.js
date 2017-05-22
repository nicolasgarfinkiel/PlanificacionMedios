angular.module('irsa.pdm.campanias.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'campaniasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, campaniasService, baseNavigationService, editBootstraperService) {
               $scope.pautas = {};

               //#region base 

               $scope.onInitEnd = function () {
                   $scope.entity.pautas.forEach(function (pauta) {
                       $scope.pautas[pauta.id.toString()] = {
                           data: []
                       };
                   });
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: campaniasService,
                   navigation: baseNavigationService
               });

               //endregion


              
                       
           }]);