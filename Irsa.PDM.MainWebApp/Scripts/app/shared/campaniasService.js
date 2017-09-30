angular.module('irsa.pdm.service.campanias', [])
       .factory('campaniasService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   changeEstadoPauta: function (pautaId, estado, motivo) {
                       return $http({
                           method: 'POST',
                           url: '/Campanias/ChangeEstadoPauta',
                           data: { pautaId: pautaId, estado: estado, motivo: motivo }
                       });
                   },
                   changeEstadoCampania: function (campania, estado, motivo) {
                       return $http({
                           method: 'POST',
                           url: '/Campanias/ChangeEstadoCampania',
                           data: { campania: campania, estado: estado, motivo: motivo }
                       });
                   },
                   getItemsByFilter: function (filter) {
                       return $http({
                           method: 'POST',
                           url: '/Campanias/GetItemsByFilter',
                           data: { filter: filter }
                       });
                   },
                   getPautasByFilter: function (filter) {
                       return $http({
                           method: 'POST',
                           url: '/Campanias/GetPautasByFilter',
                           data: { filter: filter }
                       });
                   }
               }, baseService);
               result.controller = 'Campanias';

               return result;
           }]);