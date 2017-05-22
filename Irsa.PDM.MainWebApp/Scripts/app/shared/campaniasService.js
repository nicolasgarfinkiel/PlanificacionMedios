angular.module('irsa.pdm.service.campanias', [])
       .factory('campaniasService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Campanias';

               return result;
           }]);