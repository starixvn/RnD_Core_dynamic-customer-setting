'use strict';

(function () {
    var injectParams = ['$window', 'fanexProductConfiguration'];
    var fanexCommonService = function ($window, productConfiguration) {
        // scroll to top
        this.scrollToTop = function () {
            $window.scrollTo(0, 0);
        };

        // check if value is in array, return index of value or -1 if not exist
        this.inArray = function (value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        };

        // check if object is array and it's not empty.
        this.isArrayAndNotEmpty = function (object) {
            return angular.isArray(object) && object.length > 0;
        };

        // return array of property values of object.
        this.getPropertyValues = function (object) {
            var propertyValues = [];
            for (var propertyName in object) {
                propertyValues.push(object[propertyName]);
            }

            return propertyValues;
        };

        // return product setting by product id.
        this.getProductSetting = function (productId) {
            var result = productConfiguration.defaultProductSetting;
            angular.forEach(productConfiguration.productSettings, function (productSetting) {
                if (productSetting.id === productId) {
                    result = productSetting;
                }
            });

            return result;
        };
    };

    fanexCommonService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexCommonService', fanexCommonService);
})();