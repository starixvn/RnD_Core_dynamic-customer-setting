(function () {
    'use strict';
    var injectParams = ['$compile', '$templateCache', 'fanexBlockUiService'];
    var fanexBlockUiDirective = function ($compile, $templateCache, blockUiService) {
        return {
            restrict: 'A',
            compile: function () {
                return function (scope, element) {
                    var template = $templateCache.get('templates/fanex-block-ui.htm');
                    var clone = angular.element(template);
                    element.append(clone);
                    var newScope = scope.$new();
                    var link = $compile(clone);
                    link(newScope);
                    blockUiService.setup(newScope);
                };
            }
        };
    };

    fanexBlockUiDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBlockUi', fanexBlockUiDirective);
})();