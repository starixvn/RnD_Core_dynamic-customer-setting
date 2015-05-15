'use strict';

(function () {
    var injectParams = ['$window'];
    var fanexCommonService = function ($window) {
        // scroll to top
        function scrollToTop() {
            $window.scrollTo(0, 0);
        }

        // check if value is in array, return index of value or -1 if not exist
        function inArray(value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        }

        // check if object is array and it's not empty.
        function isArrayAndNotEmpty(object) {
            return angular.isArray(object) && object.length > 0;
        }

        // return array of property values of object.
        function getPropertyValues(object) {
            var propertyValues = [];
            for (var propertyName in object) {
                propertyValues.push(object[propertyName]);
            }

            return propertyValues;
        }

        function copyProperties(source, destination) {
            for (var key in source) {
                destination[key] = source[key];
            }
        }

        return {
            scrollToTop: scrollToTop,
            inArray: inArray,
            isArrayAndNotEmpty: isArrayAndNotEmpty,
            getPropertyValues: getPropertyValues,
            copyProperties: copyProperties
        };
    };

    fanexCommonService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexCommonService', fanexCommonService);
})();