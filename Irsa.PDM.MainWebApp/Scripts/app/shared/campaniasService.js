angular.module('irsa.pdm.service.campanias', [])
       .factory('campaniasService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   changeEstadoPauta: function (pautaId, estado) {
                       return $http({
                           method: 'POST',
                           url: '/Campanias/ChangeEstadoPauta',
                           data: { pautaId: pautaId, estado: estado }
                       });
                   },
                   getItemsByFilter: function (filter) {
                       return $http({
                           method: 'POST',
                           url: '/Campanias/GetItemsByFilter',
                           data: { filter: filter }
                       });
                   }
               }, baseService);
               result.controller = 'Campanias';

               return result;
           }]);