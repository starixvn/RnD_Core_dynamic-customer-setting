'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];

    // this service contains data parsed from response after select and update.
    var fanexDataService = function (appSettings) {
        this.productName = undefined;
        this.queryData = undefined;
        this.commandType = appSettings.settingType.All;
        this.commandData = undefined;
    };

    fanexDataService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexDataService', fanexDataService);
})();