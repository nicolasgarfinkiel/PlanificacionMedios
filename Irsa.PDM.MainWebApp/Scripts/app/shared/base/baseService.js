angular.module('cresud.cdp.service.base', [])
       .factory('baseService', [
           '$http',           
           function ($http) {
               return {
                   controller: 'base',
                   getUser: function () {
                       return $http({
                           method: 'POST',
                           url: '/Account/GetUser'
                       });
                   },
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
                   createEntity: function (dto) {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/CreateEntity',
                           data: { dto: dto }
                       });
                   },
                   updateEntity: function (dto) {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/UpdateEntity',
                           data: { dto: dto }
                       });
                   },
                   deleteEntity: function (id) {
                       return $http({
                           method: 'POST',
                           url: '/' + this.controller + '/deleteEntity',
                           data: { id: id }
                       });
                   },
                           
               };                                                        
       }]);