(function () {
    'use strict';
    var injectParams = [];
    var fanexCommonService = function () {
        function group(array, condition) {
            var results = [];
            var currentItem;
            var items = [];
            angular.forEach(array, function (item) {
                if (angular.isDefined(currentItem) && !condition(currentItem, item)) {
                    results.push(items);
                    items = [];
                }

                items.push(item);
                currentItem = item;
            });

            results.push(items);
            return results;
        }

        return {
            group: group
        };
    };

    fanexCommonService.$inject = injectParams;
    angular.module('customerSettingMatrixApp').service('fanexCommonService', fanexCommonService);
})();