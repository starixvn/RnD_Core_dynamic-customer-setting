'use strict';

(function () {
    var injectParams = ['$window'];
    var commonService = function ($window) {
        this.scrollToTop = function () {
            $window.scrollTo(0, 0);
        };

        this.inArray = function (value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        };

        var OptionsBuilder = function (step, factor) {
            this.generateOptions = function (min, max, value) {
                var options = [];
                min = Math.max(0, min);
                max = Math.max(0, max);
                while (max >= min) {
                    options.push(max);
                    max -= step;
                    max = Math.round(max * factor) / factor;
                }

                if (angular.isDefined(value)) {
                    var selectIndex = -1;
                    angular.forEach(options, function (option, index) {
                        if (selectIndex === -1 && option === value) {
                            selectIndex = index;
                        }
                    });

                    if (selectIndex === -1) {
                        if (value < min) {
                            options.push(value);
                            selectIndex = options.length - 1;
                        } else if (value > max) {
                            options.unshift(value);
                            selectIndex = 0;
                        }
                    }
                }

                return options;
            };
        };

        var SelectBuilder = function (step, factor) {
            var optionsBuilder = new OptionsBuilder(step, factor);
            this.build = function (min, max, value) {
                var options = optionsBuilder.generateOptions(min, max, value);
                return {
                    valid: true,
                    disabled: false,
                    value: value,
                    options: options
                };
            };
        };

        this.correctValue = function (min, max, value) {
            return Math.max(min, Math.min(max, value));
        };

        this.copyValue = function (source, destination, field, generator) {
            destination[field].value = source[field].value;
            destination[field] = generator();
            var isCopied = source[field].value === destination[field].value;
            destination[field].valid = isCopied;
            return isCopied;
        };

        this.createPositionTakingSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(1, 1);
            return selectBuilder.build(min, max, value);
        };

        this.createDiscountSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(0.0005, 10000);
            return selectBuilder.build(min, max, value);
        };
    };

    commonService.$inject = injectParams;
    angular.module('customerSettingApp').service('commonService', commonService);
})();