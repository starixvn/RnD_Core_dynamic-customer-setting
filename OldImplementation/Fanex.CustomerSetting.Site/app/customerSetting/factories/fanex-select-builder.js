(function () {
    'use strict';
    var injectParams = [];
    var fanexSelectBuilder = function () {
        function OptionsBuilder(step, factor) {
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
        }

        function SelectBuilder(step, factor) {
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
        }

        return SelectBuilder;
    };

    fanexSelectBuilder.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexSelectBuilder', fanexSelectBuilder);
})();