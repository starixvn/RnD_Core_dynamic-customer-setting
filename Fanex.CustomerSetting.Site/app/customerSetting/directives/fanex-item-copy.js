(function () {
    'use strict';
    var injectParams = ['fanexSettingTypeHandler'];
    var fanexItemCopyDirective = function (SettingTypeHandler) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                settingType: '=settingType'
            },
            templateUrl: 'templates/fanex-item-copy.htm',
            link: function (scope) {
                scope.copy = function () {
                    var settingTypeHandler = new SettingTypeHandler(scope.settingType);
                    var scopes = settingTypeHandler.getScopes();
                    var source = scopes[0];
                    for (var i = 1; i < scopes.length; i++) {
                        scopes[i].copy(source);
                    }
                };
            }
        };
    };

    fanexItemCopyDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexItemCopy', fanexItemCopyDirective);
})();