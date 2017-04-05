angular.module('irsa.pdm.service.tarifas', [])
       .factory('tarifasService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Tarifas';

               return result;
           }]);