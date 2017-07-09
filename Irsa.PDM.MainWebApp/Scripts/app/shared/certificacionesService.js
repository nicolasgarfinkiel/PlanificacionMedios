angular.module('irsa.pdm.service.certificaciones', [])
       .factory('certificacionesService', [
           '$http',
           'baseService',
           function ($http, baseService) {
               var result = angular.extend({
                   //changeEstadoPauta: function (pautaId, estado, motivo) {
                   //    return $http({
                   //        method: 'POST',
                   //        url: '/Campanias/ChangeEstadoPauta',
                   //        data: { pautaId: pautaId, estado: estado, motivo: motivo }
                   //    });
                   //},                  
               }, baseService);
               result.controller = 'Certificaciones';

               return result;
           }]);