angular.module('irsa.pdm.service.medios', [])
       .factory('mediosService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Medios';

               return result;
           }]);