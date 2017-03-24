angular.module('cresud.cdp.navigation.base', [])
       .factory('baseNavigationService', [
          '$location',
           function ($location) {
               return {
                   goToList: function () {
                       $location.path('/');
                   },
                   goToCreate: function () {
                       $location.path('/create');
                   },
                   goToEdit: function (id) {
                       $location.path('/edit/' + id);
                   }                  
               };
           }]);