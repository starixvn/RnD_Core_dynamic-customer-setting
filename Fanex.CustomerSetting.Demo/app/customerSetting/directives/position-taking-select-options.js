'use strict';

(function () {
    var injectParams = ['$parse'];
    var positionTakingSelectOptionsDirective = function ($parse) {
        var optionTemplate = angular.element(document.createElement('option'));
        return {
            restrict: 'A',
            require: ['select', '?ngModel'],
            link: function (scope, selectElement, attrs, ctrls) {
                //var selectCtrl = ctrls[0];
                var ngModelCtrl = ctrls[1];
                if (!ngModelCtrl) {
                    return;
                }

                if (angular.isDefined(attrs.ngOptions)) {
                    throw "Don't use ng-options in this directive";
                }

                scope.$watchCollection(attrs.positionTakingSelectOptions, function (newValues, oldValues) {
                    if (newValues !== oldValues) {
                        ngModelCtrl.$render();
                    }
                });

                var valueFn = $parse(attrs.positionTakingSelectOptions);
                function render() {
                    selectElement.children().remove();
                    var modelValue = ngModelCtrl.$modelValue;
                    var options = valueFn(scope);
                    angular.forEach(options, function (option) {
                        var value = option;
                        var label = value / 100;
                        var selected = modelValue === option;
                        var optionElement = optionTemplate.clone();
                        optionElement.val(value);
                        optionElement.prop('selected', selected);
                        optionElement.attr('selected', selected);
                        optionElement.text(label);
                        selectElement.append(optionElement);
                    });
                }
                ngModelCtrl.$render = render;
            }
        };
    };

    positionTakingSelectOptionsDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('positionTakingSelectOptions', positionTakingSelectOptionsDirective);
})();