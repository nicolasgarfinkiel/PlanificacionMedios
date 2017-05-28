angular.module('irsa.pdm.service.proveedores', [])
       .factory('proveedoresService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Proveedores';

               return result;
           }]);