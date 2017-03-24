angular.module('cresud.cdp.service.general', [])
       .factory('generalService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   getClienteById: function (id) {
                       return $http({
                           method: 'POST',
                           url: '/General/GetClienteById',
                           data: { id: id }
                       });
                   },
                   getClientesByFilter: function (filter) {
                       return $http({
                           method: 'POST',
                           url: '/General/GetClientesByFilter',
                           data: { filter: filter }
                       });
                   },
                   getClientesConProveedorByFilter: function (filter) {
                       return $http({
                           method: 'POST',
                           url: '/General/GetClientesConProveedorByFilter',
                           data: { filter: filter }
                       });
                   },
                   getProveedoresByFilter: function (filter) {
                       return $http({
                           method: 'POST',
                           url: '/General/GetProveedoresByFilter',
                           data: { filter: filter }
                       });
                   }
               }, baseService);
               result.controller = 'General';

               return result;
           }]);