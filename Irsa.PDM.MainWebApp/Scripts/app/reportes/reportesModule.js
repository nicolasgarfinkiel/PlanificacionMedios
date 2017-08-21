angular.module('irsa.pdm.reportes', [    
    'irsa.pdm.service.campanias',    
    'irsa.pdm.service.base',    
    'ngRoute',
    'ngGrid',
    '$strap.directives',
    'irsa.pdm.directive.loading',    
    'irsa.pdm.directive.debounce',
    'irsa.pdm.directive.int',
    'ui.select'
]).config([
    '$routeProvider',
    '$locationProvider',
    "$httpProvider",
    function ($routeProvider, $locationProvider, $httpProvider) {
        $httpProvider.interceptors.push(function ($q, $rootScope) {
            if ($rootScope.activeCalls == undefined) {
                $rootScope.activeCalls = 0;
            }

            return {
                request: function (config) {
                    $rootScope.activeCalls += 1;
                    return config;
                },
                requestError: function (rejection) {
                    $rootScope.activeCalls -= 1;
                    return rejection;
                },
                response: function (response) {
                    $rootScope.activeCalls -= 1;
                    return response;
                },
                responseError: function (rejection) {
                    $rootScope.activeCalls -= 1;
                    return rejection;
                }
            };
        });                      
    }
]).controller('reportesCtrl', [
    '$scope',
    'campaniasService', 
    function ($scope, campaniasService) {
        $scope.filter = {};
        $scope.pautas = [];
        $scope.pautasCount = 0;

        $scope.clearAndFind = function () {
            $scope.pautas = [];
            $scope.findPautas();
        };

        $scope.findPautas = function () {            
            $scope.filter.currentPage = $scope.gridPautas.pagingOptions.currentPage;
            $scope.filter.pageSize = $scope.gridPautas.pagingOptions.pageSize;
            $scope.filter.campaniaCodigo = $scope.filter.campania ?  $scope.filter.campania.codigo : null;

            campaniasService.getPautasByFilter($scope.filter).then(function (response) {
                $scope.pautas = response.data.data;
                $scope.pautasCount = response.data.count;
            }, function () { throw 'Error on getPautasByFilter'; });
        };

        $scope.gridPautas = {
            data: 'pautas',
            columnDefs: [
                       { field: 'campania.codigo', displayName: 'Código campaña', width: 140 },
                       { field: 'campania.nombre', displayName: 'Nombre' },                   
                       { field: 'codigo', displayName: 'Código pauta', width: 100 },
                       { field: 'estado', displayName: 'Estado pauta' },
                       { field: 'estado', displayName: 'Reporte de pauta', width: 140, cellTemplate: '<div class="ng-grid-icon-container"><form method="POST" action="/Reportes/GetExcelReporteDePauta"><input type="hidden" name="pautaId" value="{{row.entity.id}}" /><button type="submit" style="border: 0; background: 0;"><i class="fa fa-file-excel-o text-success"></i></button></form></div>' },
                       { field: 'estado', displayName: 'Reporte visual de pauta', width: 170, cellTemplate: '<div class="ng-grid-icon-container"><form method="POST" action="/Reportes/GetExcelVisualDePauta"><input type="hidden" name="pautaId" value="{{row.entity.id}}" /><button type="submit" style="border: 0; background: 0;"><i class="fa fa-file-excel-o text-success"></i></button></form></div>' }
            ],
            showFooter: true,
            enablePaging: true,
            multiSelect: false,
            totalServerItems: 'pautasCount',
            pagingOptions: {
                pageSizes: [10],
                pageSize: 10,
                currentPage: 1
            },
            filterOptions: { useExternalFilter: true }
        };


        $scope.$watch('gridPautas.pagingOptions', function (newVal, oldVal) {
            if (newVal == oldVal || newVal.currentPage == oldVal.currentPage) return;
            $scope.findPautas();
        }, true);      

        //#region Select UI campanias

        $scope.selectList = [];
        $scope.currentPageCampanias = 0;
        $scope.pageCountCampanias = 0;
        $scope.filterCampanias = { pageSize: 20 };

        $scope.getSelectSource = function ($select, $event) {
            if ($scope.loading) return;

            if (!$event) {
                $scope.currentPageCampanias = 1;
                $scope.pageCountCampanias = 0;
                $scope.selectList = [];
            } else {
                $event.stopPropagation();
                $event.preventDefault();
                $scope.currentPageCampanias++;
            }

            $scope.filterCampanias.currentPage = $scope.currentPageCampanias;
            $scope.filterCampanias.multiColumnSearchText = $select.search;            

            campaniasService.getByFilter($scope.filterCampanias).then(function (response) {
                $scope.selectList = $scope.selectList.concat(response.data.data);
                $scope.pageCountCampanias = Math.ceil(response.data.count / 20);
            }, function () { throw 'Error on getByFilter'; });
        };

        //#endregion         

    }]);
      