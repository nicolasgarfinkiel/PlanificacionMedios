angular.module('irsa.pdm.service.tarifarios', [])
       .factory('tarifariosService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Tarifarios';

               return result;
           }]);