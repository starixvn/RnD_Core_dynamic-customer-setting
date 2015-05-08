'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];

    // this service contains data parsed from response after select and update.
    var fanexDataService = function (appSettings) {
        return {
            productName: undefined,
            queryData: undefined,
            commandType: appSettings.settingType.All,
            commandData: undefined
        };
    };

    fanexDataService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexDataService', fanexDataService);
})();