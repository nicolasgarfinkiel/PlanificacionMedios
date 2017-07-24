angular.module('irsa.pdm.service.tarifarios', [])
       .factory('tarifariosService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   getFechaDesde: function (vehiculolId) {
                       return $http({
                           method: 'POST',
                           url: '/Tarifarios/getFechaDesde',
                           data: { vehiculolId: vehiculolId }
                       });
                   },
                   aprobar: function (tarifarioId) {
                       return $http({
                           method: 'POST',
                           url: '/Tarifarios/aprobar',
                           data: { tarifarioId: tarifarioId }
                       });
                   }
               }, baseService);
               result.controller = 'Tarifarios';

               return result;
           }]);