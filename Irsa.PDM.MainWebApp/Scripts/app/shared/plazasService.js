angular.module('irsa.pdm.service.plazas', [])
       .factory('plazasService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Plazas';

               return result;
           }]);