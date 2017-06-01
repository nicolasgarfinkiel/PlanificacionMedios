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
                   },
                   setValuesByProveedor: function (tarifaProveedor) {
                       return $http({
                           method: 'POST',
                           url: '/Tarifas/SetValuesByProveedor',
                           data: { tarifaProveedor: tarifaProveedor }
                       });
                   }
               }, baseService);
               result.controller = 'Tarifas';

               return result;
           }]);