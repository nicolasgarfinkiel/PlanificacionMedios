angular.module('irsa.pdm.service.campanias', [])
       .factory('campaniasService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   changeEstadoCampania: function (id, estado, motivo) {
                       return $http({
                           method: 'POST',
                           url: '/Campanias/ChangeEstadoCampania',
                           data: { id: id, estado: estado, motivo: motivo }
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