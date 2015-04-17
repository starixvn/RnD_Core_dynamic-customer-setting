'use strict';

(function () {
    var injectParams = ['$rootScope', 'controllerService', 'appSettings', 'resourceService', 'tooltipService'];
    var customerSettingCopyDirective = function ($rootScope, controllerService, appSettings, resourceService, tooltipService) {
        return {
            restrict: 'A',
            link: function (scope, element, attributes) {
                var isShow = scope.$eval(attributes.ngShow);
                if (!isShow) {
                    return;
                }

                element.on('click', function () {
                    var copy = function () {
                        var count = 0;
                        var scopes = controllerService.getScopes(appSettings.settingType.PositionTaking);
                        var source = scopes[0];
                        for (var i = 1; i < scopes.length; i++) {
                            var scope = scopes[i];
                            var result = scope.copy(source);
                            if (!result) {
                                count++;
                            }
                        }

                        if (count > 0) {
                            tooltipService.show(element, resourceService.getString('DuplicateValuesAlertMsg'), 'left');
                            element.on('mouseleave', function () {
                                tooltipService.hide();
                                element.off('mouseleave');
                            });
                        } else {
                            tooltipService.hide();
                        }
                    };

                    if ($rootScope.$$phase) {
                        scope.$evalAsync(copy);
                    } else {
                        scope.$apply(copy);
                    }
                });
            }
        };
    };

    customerSettingCopyDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingCopy', customerSettingCopyDirective);
})();