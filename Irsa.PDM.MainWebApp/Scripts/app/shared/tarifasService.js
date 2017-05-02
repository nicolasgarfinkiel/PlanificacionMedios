angular.module('irsa.pdm.service.tarifas', [])
       .factory('tarifasService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   setValues: function (filter, importe, oc) {
                       return $http({
                           method: 'POST',
                           url: '/Tarifas/SetValues',
                           data: {filter: filter, importe: importe, oc: oc}
                       });
                   }
               }, baseService);
               result.controller = 'Tarifas';

               return result;
           }]);