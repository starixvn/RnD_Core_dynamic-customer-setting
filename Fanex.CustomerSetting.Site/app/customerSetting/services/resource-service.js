'use strict';

(function () {
    var injectParams = ['resources', 'appSettings'];
    var resourceService = function (resources, appSettings) {
        var resource = {};

        this.setLanguage = function (language) {
            if (angular.isUndefined(language)) {
                language = appSettings.defaultLanguage.toLowerCase();
            } else {
                language = language.toLowerCase();
            }

            var selectResource = resources[language];
            if (angular.isUndefined(selectResource)) {
                selectResource = resources[appSettings.defaultLanguage.toLowerCase()];
            }
            resource = selectResource;
        };

        this.getString = function (key) {
            var value = resource[key];
            if (angular.isDefined(value)) {
                value = value.replace(/&apos;|&#x0*27;/g, "'");
            } else {
                value = key;
            }
            return value;
        };

        this.getFormatString = function (key) {
            var input = this.getString(key);
            var args = Array.prototype.slice.call(arguments, 1);
            return input.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined' ? args[number] : match;
            });
        };
    };

    resourceService.$inject = injectParams;
    angular.module('customerSettingApp').service('resourceService', resourceService);
})();