angular.module('cresud.cdp.service.choferes', [])
       .factory('choferesService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({}, baseService);
               result.controller = 'Choferes';

               return result;
           }]);