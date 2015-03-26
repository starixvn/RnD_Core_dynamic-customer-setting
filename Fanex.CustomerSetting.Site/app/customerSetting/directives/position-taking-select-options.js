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
                var currentOptions;
                var currentValue;
                function render() {
                    var value = ngModelCtrl.$modelValue;
                    var options = valueFn(scope);
                    var hasChange = false;
                    if (currentValue !== value) {
                        hasChange = true;
                    } else if (angular.isUndefined(currentOptions) || currentOptions.length !== options.length) {
                        hasChange = true;
                    } else {
                        for (var i = 0; i < currentOptions.length; i++) {
                            if (currentOptions[i] !== options[i]) {
                                hasChange = true;
                                break;
                            }
                        }
                    }

                    if (!hasChange) {
                        //console.log('This check is useful');
                        return;
                    }

                    currentOptions = options;
                    currentValue = value;
                    selectElement.children().remove();
                    angular.forEach(currentOptions, function (option) {
                        var value = option;
                        var label = value / 100;
                        var selected = currentValue === option;
                        var optionElement = optionTemplate.clone();
                        optionElement.val(value);
                        optionElement.prop('selected', selected);
                        optionElement.attr('selected', selected);
                        optionElement.text(label);
                        selectElement.append(optionElement);
                    });
                }
                ngModelCtrl.$render = render;
                ngModelCtrl.$parsers.unshift(function (viewValue) {
                    return parseInt(viewValue);
                });
            }
        };
    };

    positionTakingSelectOptionsDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('positionTakingSelectOptions', positionTakingSelectOptionsDirective);
})();