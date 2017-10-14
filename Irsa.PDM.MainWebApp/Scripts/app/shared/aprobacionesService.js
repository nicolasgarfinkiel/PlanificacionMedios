angular.module('irsa.pdm.service.aprobaciones', [])
       .factory('aprobacionesService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   //aprobar: function () {
                   //    return $http({
                   //        method: 'POST',
                   //        url: '/Aprobaciones/aprobar'                          
                   //    });
                   //}
               }, baseService);
               result.controller = 'Aprobaciones';

               return result;
           }]);