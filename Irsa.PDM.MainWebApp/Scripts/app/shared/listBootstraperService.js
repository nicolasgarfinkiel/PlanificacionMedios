angular.module('cresud.cdp.service.bootstraper.list', [])
       .factory('listBootstraperService', [
           function () {
               var scope = {};
               var data = {};

               var bootstrapper = {
                   list: [],
                   columns: [],
                   filter: {},
                   result: { data: null, hasErrors: false, messages: [] },
                   create: function () {
                       data.navigation.goToCreate();
                   },
                   edit: function (id) {
                       data.navigation.goToEdit(id);
                   },
                   totalItems: 0,
                   gridOptions: {
                       data: 'list',
                       columnDefs: 'columns',
                       showFooter: true,
                       useExternalSorting: true,
                       useExternalPagination: true,
                       enablePaging: true,
                       enableRowSelection: false,
                       totalServerItems: 'totalItems',
                       pagingOptions: {
                           pageSizes: [15],
                           pageSize: 15,
                           currentPage: 1
                       }
                   },
                   search: function (reset) {
                       if (reset) {
                           scope.gridOptions.pagingOptions.currentPage = 1;
                       }

                       scope.filter.currentPage = scope.gridOptions.pagingOptions.currentPage;
                       scope.filter.pageSize = scope.gridOptions.pagingOptions.pageSize;

                       data.service.getByFilter(scope.filter)
                           .then(function (response) {
                               scope.result = response.data.result;

                               if (!scope.result.hasErrors) {
                                   scope.list = response.data.data;
                                   scope.totalItems = response.data.count;
                               }
                               
                           }, function () { throw 'Error on getByFilter'; });
                   },                   
                   getDataListInit: function () {
                       data.service.getDataListInit().then(function (response) {
                           scope.result = response.data.result;                           
                           scope.data = response.data.data.data;
                           scope.usuario = response.data.data.usuario;
                           scope.filter.idGrupoEmpresa = scope.usuario.currentEmpresa.grupoEmpresa.id;
                           scope.filter.empresaId = scope.usuario.currentEmpresa.id;

                           if (scope.onInitEnd) scope.onInitEnd();
                           scope.search();
                       }, function () { throw 'Error on getDataListInit'; });
                   },
                   init: function () {
                       scope.columns = data.columns;

                       scope.$watch('gridOptions.pagingOptions', function (newVal, oldVal) {
                           if (newVal == oldVal || newVal.currentPage == oldVal.currentPage) return;
                           scope.search();
                       }, true);

                       scope.$watch('filter.multiColumnSearchText', function (newValue, oldValue) {
                           if (typeof newValue == 'undefined' || newValue == oldValue) return;

                           scope.gridOptions.pagingOptions.currentPage = 1;
                           scope.search();
                       });

                       scope.getDataListInit();
                   }
               };

               return {
                   init: function (s, d) {
                       scope = s;
                       data = d;

                       for (var prop in bootstrapper) {
                           scope[prop] = scope[prop] || bootstrapper[prop];
                       }

                       scope.init();
                   }
               };
           }]);