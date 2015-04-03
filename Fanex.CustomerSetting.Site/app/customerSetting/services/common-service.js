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

        this.generateOptions = function (min, max, step, factor) {
            var options = [];
            min = Math.max(0, min);
            max = Math.max(0, max);
            while (max >= min) {
                options.push(max);
                max -= step;
                max = Math.round(max * factor) / factor;
            }
            return options;
        };

        var self = this;
        var PositionTakingSelect = function (min, max, value) {
            var options = self.generateOptions(min, max, 1, 1);
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

            this.options = options;
            this.value = this.options[selectIndex];
            this.min = min;
            this.max = max;
        };

        this.createPositionTakingSelect = function (min, max, value) {
            return new PositionTakingSelect(min, max, value);
        };
    };

    commonService.$inject = injectParams;
    angular.module('customerSettingApp').service('commonService', commonService);
})();