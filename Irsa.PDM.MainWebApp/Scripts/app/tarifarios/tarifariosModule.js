angular.module('irsa.pdm.tarifarios', [
    'irsa.pdm.tarifarios.ctrl.list',
    'irsa.pdm.tarifarios.ctrl.edit',
    'irsa.pdm.service.tarifarios',
    'irsa.pdm.service.tarifas',
    'irsa.pdm.service.vehiculos',
    'irsa.pdm.service.proveedores',
    'irsa.pdm.navigation.base',    
    'irsa.pdm.service.base',
    'irsa.pdm.service.bootstraper.list',
    'irsa.pdm.service.bootstraper.edit',    
    'ngRoute',
    'ngGrid',
    '$strap.directives',
    'irsa.pdm.directive.loading',    
    'irsa.pdm.directive.debounce',
    'irsa.pdm.directive.int',
    'fcsa-number',
    'ui.select'
]).config([
    '$routeProvider',
    '$locationProvider',
    "$httpProvider",
    function ($routeProvider, $locationProvider, $httpProvider) {

        $routeProvider.when('/', {
            templateUrl: 'tarifarios/list',
            controller: 'listCtrl'
        });

        $routeProvider.when('/create', {
            templateUrl: 'tarifarios/edit',
            controller: 'editCtrl'
        });

        $routeProvider.when('/edit/:id', {
            templateUrl: 'tarifarios/edit',
            controller: 'editCtrl'
        });

        $routeProvider.otherwise({
            redirectTo: '/'
        });

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
               

        var regexIso8601 = /^\d{4}(-\d\d(-\d\d(T\d\d:\d\d(:\d\d)?(\.\d+)?(([+-]\d\d:\d\d)|Z)?)?)?)?$/i;

        $httpProvider.defaults.transformResponse.push(function (responseData) {
            convertDateStringsToDates(responseData);
            return responseData;
        });

        function convertDateStringsToDates(input) {
            // Ignore things that aren't objects.
            if (typeof input !== "object") return input;


            for (var key in input) {
                if (!input.hasOwnProperty(key)) continue;

                var value = input[key];
                var match;
                if (typeof value === "string" && value.length > 4 && (match = value.match(regexIso8601))) {
                    input[key] = moment(value).format('DD/MM/YYYY');
                } else if (typeof value === "object") {
                    // Recurse into object
                    convertDateStringsToDates(value);
                }
            }
        };

    }
]).run(['$rootScope', function ($rootScope) {
    $rootScope.$on('$routeChangeSuccess', function () {
        setTimeout(function() {            
        }, 200);
    });
}]);

      