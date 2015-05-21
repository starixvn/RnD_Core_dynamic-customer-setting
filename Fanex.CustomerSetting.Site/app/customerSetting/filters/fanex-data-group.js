(function () {
    'use strict';
    var injectParams = [];
    var fanexDataGroup = function () {
        function DataGroup(sportType, sportTypeName) {
            this.SportType = sportType;
            this.SportTypeName = sportTypeName;
            this.Items = [];
            this.addItem = function (item) {
                this.Items.push(item);
            };
        }

        return function (items) {
            var dataGroups = [];
            var currentSportType = -1;
            var dataGroup = null;
            angular.forEach(items, function (item) {
                var sportType = item.SportType;
                var sportTypeName = item.SportTypeName;
                if (dataGroup === null || sportType !== currentSportType) {
                    currentSportType = sportType;
                    dataGroup = new DataGroup(sportType, sportTypeName);
                    dataGroups.push(dataGroup);
                }
                dataGroup.addItem(item);
            });

            return dataGroups;
        };
    };

    fanexDataGroup.$inject = injectParams;
    angular.module('customerSettingApp').filter('fanexDataGroup', fanexDataGroup);
})();