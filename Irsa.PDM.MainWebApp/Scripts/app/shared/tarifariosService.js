angular.module('irsa.pdm.service.tarifarios', [])
       .factory('tarifariosService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   getFechaDesde: function () {
                       return $http({
                           method: 'POST',
                           url: '/Tarifarios/getFechaDesde'
                       });
                   },
               }, baseService);
               result.controller = 'Tarifarios';

               return result;
           }]);