angular.module('irsa.pdm.campanias.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'campaniasService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, campaniasService, baseNavigationService, editBootstraperService) {
               $scope.pautas = {};
               $scope.currentPauta = null;
               $scope.resultModal = { hasErrors: false, messages: [] };

               //#region base 

               $scope.onInitEnd = function () {
                   $scope.entity.pautas.forEach(function (pauta) {
                       $scope.pautas[pauta.id.toString()] = {
                           data: [],
                           filter: {
                               currentPage: 1,
                               pageSize: 10,
                               pautaId: pauta.id,
                               campaniaCodigo: $scope.entity.codigo
                           }
                       };

                       $scope.findItems(pauta.id);
                   });
               };

               editBootstraperService.init($scope, $routeParams, {
                   service: campaniasService,
                   navigation: baseNavigationService
               });

               //#endregion

               $scope.confirmRechazoCampania = function () {
                   $scope.currentPauta = null;
                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $scope.motivo = null;
                   $('#rechazoModal').modal('show');
               };

               $scope.confirmRechazoPauta = function (pauta) {
                   $scope.currentPauta = pauta;
                   $scope.resultModal = { hasErrors: false, messages: [] };
                   $scope.motivo = null;
                   $('#rechazoModal').modal('show');
               };

               $scope.changeEstadoCampania = function (estado) {
                   $scope.resultModal = $scope.result = { hasErrors: false, messages: [] };

                   if (!$scope.motivo && estado == 'Rechazada') {
                       $scope.resultModal = { hasErrors: true, messages: ['Ingrese el motivo'] };
                       return;
                   }

                   if (estado == 'Aprobada' && !$scope.isValidAprobacion()) {                       
                       return;
                   }

                   campaniasService.changeEstadoCampania($scope.entity, estado, $scope.motivo).then(function (response) {
                       $scope.resultModal = $scope.result = response.data.result;

                       if ($scope.resultModal.hasErrors) return;

                       $scope.entity.estado = estado;

                       $scope.entity.pautas.forEach(function (item) {
                           item.estado = estado;
                       });

                       $scope.entity.pautas = angular.copy($scope.entity.pautas);
                       $scope.motivo = null;
                       $('#rechazoModal').modal('hide');
                       $('#aprobacionModal').modal('hide');
                   }, function () { throw 'Error on changeEstadoCampania'; });
               };


               $scope.confirmAprobacion = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };

                   $scope.campania = $scope.entity;
                   $scope.campania.idSapDistribucion = null;
                   $scope.campania.centro = null;
                   $scope.campania.almacen = null;
                   $scope.campania.orden = null;
                   $scope.campania.centroDestino = null;
                   $scope.campania.almacenDestino = null;

                   $('#aprobacionModal').modal('show');
               };

               $scope.isValidAprobacion = function () {
                   $scope.resultModal = { hasErrors: false, messages: [] };

                   if (!$scope.campania.centro) {
                       $scope.resultModal.messages.push('Ingrese el id centro');
                   }

                   if (!$scope.campania.almacen) {
                       $scope.resultModal.messages.push('Ingrese el id almacen');
                   }

                   $scope.resultModal.hasErrors = $scope.resultModal.messages.length;
                   return !$scope.resultModal.hasErrors;
               };

               $scope.changeEstadoPauta = function (pauta, estado) {
                   $scope.resultModal = $scope.result = { hasErrors: false, messages: [] };

                   if (!$scope.motivo && estado == 'Rechazada') {
                       $scope.resultModal = { hasErrors: true, messages: ['Ingrese el motivo'] };
                       return;
                   }

                   campaniasService.changeEstadoPauta(pauta.id, estado, $scope.motivo).then(function (response) {
                       $scope.resultModal = $scope.result = response.data.result;

                       if ($scope.resultModal.hasErrors) return;

                       $scope.entity.estado = response.data.data;
                       pauta.estado = estado;
                       $scope.entity.pautas = angular.copy($scope.entity.pautas);
                       $scope.motivo = null;
                       $scope.currentPauta = null;
                       $('#rechazoModal').modal('hide');
                   }, function () { throw 'Error on changeEstadoPauta'; });
               };

               $scope.isFirstPage = function (pautaId) {
                   return $scope.pautas[pautaId.toString()].filter.currentPage == 1;
               };

               $scope.isLastPage = function (pautaId) {
                   return $scope.pautas[pautaId.toString()].filter.currentPage == $scope.pautas[pautaId.toString()].filter.pageCount;
               };

               $scope.goToNextPage = function (pautaId) {
                   if ($scope.isLastPage(pautaId)) return;
                   $scope.pautas[pautaId.toString()].filter.currentPage++;
                   $scope.findItems(pautaId);
               };

               $scope.goToPreviousPage = function (pautaId) {
                   if ($scope.isFirstPage(pautaId)) return;
                   $scope.pautas[pautaId.toString()].filter.currentPage--;
                   $scope.findItems(pautaId);
               };

               $scope.findItems = function (pautaId) {
                   campaniasService.getItemsByFilter($scope.pautas[pautaId.toString()].filter).then(function (response) {
                       $scope.pautas[pautaId.toString()].data = response.data.data;
                       $scope.pautas[pautaId.toString()].filter.count = response.data.count;
                       $scope.pautas[pautaId.toString()].filter.pageCount = Math.ceil($scope.pautas[pautaId.toString()].filter.count / $scope.pautas[pautaId.toString()].filter.pageSize);
                   });
               };

               $scope.openTarifaModal = function(tarifa) {
                   $scope.currentTarifa = tarifa;
                   $('#tarifaModal').modal('show');
               };
           }]);