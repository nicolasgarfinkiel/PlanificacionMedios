angular.module('cresud.cdp.directive.loading', [])
    .directive('loading', ['$http', function ($http) {
        return {
            restrict: 'E',
            replace: true,
            template: '<div class="loading" style="display: none;"></div>',
            link: function (scope, element, attrs) {
                var container = $("body")[0];
                
                scope.$watch('activeCalls', function (newVal, oldVal) {
                    if (newVal == 0) {
                        scope.unblockUI();
                    }
                    else {
                        scope.blockUI();
                    }
                });
               
                scope.blockUI = function() {                 
                    $(container).block(
                                {
                                    message: element,
                                    css: {
                                        width: 100,
                                        height: 80,
                                        border: 0,
                                        backgroundColor: '',
                                        zIndex: 9999999
                                    },
                                    overlayCSS: { opacity: 0.0 }
                                });
                };

                scope.unblockUI = function () {
                    setTimeout(function () { $(container).unblock(); }, 300);
                };
            }
        };
    }]);
      