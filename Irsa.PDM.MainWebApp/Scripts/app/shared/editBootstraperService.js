angular.module('cresud.cdp.service.bootstraper.edit',
    [        
    ]).factory('editBootstraperService', [
           function () {
               var scope = {};
               var data = {};
               var $routeParams = {};

               var bootstrapper = {
                   entity: {},
                   operation: null,
                   result: { hasErrors: false, messages: [] },                   
                   list: function () {
                       data.navigation.goToList();
                   },
                   isValid: function() {
                       return true;
                   },
                   save: function () {
                       if (!scope.isValid()) return;

                       if (scope.entity.id) {
                           data.service.updateEntity(scope.entity).then(function (response) {
                               if (!response.data.result.hasErrors)
                                   data.navigation.goToList();

                               scope.result = response.data.result;
                           }, function () { throw 'Error on update'; });
                       } else {
                           data.service.createEntity(scope.entity).then(function (response) {
                               if (!response.data.result.hasErrors)
                                   data.navigation.goToList();

                               scope.result = response.data.result;
                           }, function () { throw 'Error on create'; });
                       }
                   },

                   //#region Init
                   getDataEditInit: function (entity) {
                       data.service.getDataEditInit().then(function (response) {
                           scope.result = response.data.result;
                           scope.data = response.data.data.data;
                           scope.usuario = response.data.data.usuario;
                           scope.entity = entity;

                           if (!entity.id) {
                               scope.entity.grupoEmpresaId = scope.usuario.currentEmpresa.grupoEmpresa.id;
                               scope.entity.empresaId = scope.usuario.currentEmpresa.id;
                           }

                           if (scope.onInitEnd) scope.onInitEnd();
                       }, function () { throw 'Error on getDataEditInit'; });
                   },

                   init: function () {
                       scope.entity = data.entity;                       

                       if (angular.isUndefined($routeParams.id)) {
                           scope.getDataEditInit({});
                           scope.operation = 'Alta';
                       } else {
                           scope.operation = 'Edición';
                           data.service.getById($routeParams.id).then(function (response) {
                               scope.getDataEditInit(response.data.data);
                           }, function () { throw 'Error on get'; });
                       }                       
                   }
               };

               return {
                   init: function (s, rp, d) {
                       scope = s;
                       data = d;
                       $routeParams = rp;

                       for (var prop in bootstrapper) {
                           scope[prop] = scope[prop] || bootstrapper[prop];
                       }

                       scope.init();
                   }
               };
           }]);