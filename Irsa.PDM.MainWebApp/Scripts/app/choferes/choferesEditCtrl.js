angular.module('cresud.cdp.choferes.ctrl.edit', [])
       .controller('editCtrl', [
           '$scope',
           '$routeParams',
           'choferesService',
           'baseNavigationService',
           'editBootstraperService',
           function ($scope, $routeParams, choferesService, baseNavigationService, editBootstraperService) {

               $scope.onInitEnd = function () {
                   $scope.operation = !$routeParams.id ? 'Nuevo chofer' : 'Edición de chofer';
                   $scope.esParaguay = $scope.usuario.currentEmpresa.grupoEmpresa.paisDescripcion.toUpperCase() == 'PARAGUAY';
                   $scope.esGrupoCresud = $scope.usuario.currentEmpresa.grupoEmpresa.id == 1;
               };

               editBootstraperService.init($scope, $routeParams,  {
                   service: choferesService,
                   navigation: baseNavigationService
               });
              
               $scope.isValid = function() {
                   $scope.result = { hasErrors: false, messages: [] };
                   var patenteOldRegex = /^[A-ZÑ]{3}\d{3}$/;
                   var patenteNewRegex = /^[A-ZÑ]{2}\d{3}[A-ZÑ]{2}$/;

                   if (!$scope.entity.nombre) {
                       $scope.result.messages.push($scope.entity.esChoferTransportista ? 'Ingrese la descripción' : 'Ingrese el nombre');
                   }                   

                   if (!$scope.entity.esChoferTransportista && !$scope.entity.apellido) {
                       $scope.result.messages.push('Ingrese el apellido');
                   }

                   if ($scope.esGrupoCresud && !$scope.entity.cuit) {
                       $scope.result.messages.push('Ingrese el cuit');
                   }

                   if ($scope.esGrupoCresud && $scope.entity.cuit && !$scope.isValidCuit($scope.entity.cuit)) {
                       $scope.result.messages.push('Cuit inválido');
                   }

                   if (!$scope.entity.esChoferTransportista && $scope.esParaguay && !$scope.entity.domicilio) {
                       $scope.result.messages.push('Ingrese el domicilio');
                   }

                   if (!$scope.entity.esChoferTransportista && $scope.esParaguay && !$scope.entity.marca) {
                       $scope.result.messages.push('Ingrese la marca');
                   }

                   if (!$scope.entity.esChoferTransportista && $scope.esGrupoCresud && !$scope.entity.camion) {
                       $scope.result.messages.push('Ingrese la patente del camión');
                   }

                   if (!$scope.entity.esChoferTransportista && 
                        $scope.esGrupoCresud &&
                        $scope.entity.camion &&
                        !($scope.entity.camion.match(patenteOldRegex) || $scope.entity.camion.match(patenteNewRegex))) {
                        $scope.result.messages.push('Formato de patente de camión inválido. Formato corrercto ej: AAA111 o AA111AA');
                   }
                                                      
                   if (!$scope.entity.esChoferTransportista && $scope.esGrupoCresud && !$scope.entity.acoplado) {
                       $scope.result.messages.push('Ingrese la patente del acoplado');
                   }

                   if (!$scope.entity.esChoferTransportista &&
                        $scope.esGrupoCresud &&
                        $scope.entity.acoplado &&
                        !($scope.entity.acoplado.match(patenteOldRegex) || $scope.entity.acoplado.match(patenteNewRegex))) {
                        $scope.result.messages.push('Formato de patente de acoplado inválido. Formato corrercto ej: AAA111 o AA111AA');
                   }

                   $scope.result.hasErrors = $scope.result.messages.length;
                   return !$scope.result.hasErrors;
               };

               $scope.isValidCuit = function (cuit) {

                   if (cuit.length != 11) {
                       return false;
                   }

                   var acumulado = 0;
                   var digitos = cuit.split("");
                   var digito = digitos.pop();

                   for (var i = 0; i < digitos.length; i++) {
                       acumulado += digitos[9 - i] * (2 + (i % 6));
                   }

                   var verif = 11 - (acumulado % 11);
                   if (verif == 11) {
                       verif = 0;
                   } else if (verif == 10) {
                       verif = 9;
                   }

                   return digito == verif;
               };
           }]);