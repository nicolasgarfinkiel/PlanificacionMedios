angular.module('irsa.pdm.service.vehiculos', [])
       .factory('vehiculosService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Vehiculos';

               return result;
           }]);