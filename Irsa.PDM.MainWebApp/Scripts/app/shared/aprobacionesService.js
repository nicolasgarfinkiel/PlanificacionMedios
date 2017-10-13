angular.module('irsa.pdm.service.aprobaciones', [])
       .factory('aprobacionesService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   changeEstadoPauta: function (pautaId, estado, motivo) {
                       return $http({
                           method: 'POST',
                           url: '/Aprobaciones/ChangeEstadoPauta',
                           data: { pautaId: pautaId, estado: estado, motivo: motivo }
                       });
                   }
               }, baseService);
               result.controller = 'Aprobaciones';

               return result;
           }]);