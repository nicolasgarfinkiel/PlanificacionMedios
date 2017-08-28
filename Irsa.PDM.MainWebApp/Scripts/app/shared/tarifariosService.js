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
                   getFechaDesdeProveedor: function (proveedorId) {
                       return $http({
                           method: 'POST',
                           url: '/Tarifarios/getFechaDesdeProveedor',
                           data: { proveedorId: proveedorId }
                       });
                   },
                   aprobar: function (tarifarioId) {
                       return $http({
                           method: 'POST',
                           url: '/Tarifarios/aprobar',
                           data: { tarifarioId: tarifarioId }
                       });
                   },
                   createTarifariosProveedor: function (tarifarioProveedor) {
                       return $http({
                           method: 'POST',
                           url: '/Tarifarios/createTarifariosProveedor',
                           data: { tarifarioProveedor: tarifarioProveedor }
                       });
                   }
               }, baseService);
               result.controller = 'Tarifarios';

               return result;
           }]);