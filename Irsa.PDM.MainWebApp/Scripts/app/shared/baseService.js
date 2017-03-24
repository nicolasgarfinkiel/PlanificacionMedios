angular.module('cresud.cdp.service.base', [])
       .factory('baseService', [
           '$http',
           function ($http) {
              
               return {
                   controller: 'Home',
                   getDataListInit: function () {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/GetDataListInit'
                       });
                   },
                   getDataEditInit: function () {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/GetDataEditInit'
                       });
                   },                   
                   getById: function (id) {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/GetById',
                           data: { id: id }
                       });
                   },
                   getByFilter: function (filter) {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/GetByFilter',
                           data: { filter: filter }
                       });
                   },
                   createEntity: function (entity) {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/CreateEntity',
                           data: { entity: entity }
                       });
                   },
                   updateEntity: function (entity) {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/UpdateEntity',
                           data: { entity: entity }
                       });
                   },
                   
               };
           }]);