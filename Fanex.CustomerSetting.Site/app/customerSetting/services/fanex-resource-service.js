'use strict';

(function () {
    var injectParams = ['fanexResources', 'fanexAppSettings', 'fanexDataService'];
    var fanexResourceService = function (resources, appSettings, dataService) {
        var resource = {};

        function setLanguage(language) {
            var selectResource = resources[language.toLowerCase()];
            if (angular.isUndefined(selectResource)) {
                selectResource = resources[appSettings.defaultLanguage.toLowerCase()];
            }
            resource = selectResource;
        }

        function getString(key) {
            var value = resource[key];
            if (angular.isDefined(value)) {
                value = value.replace(/&apos;|&#x0*27;/g, "'");
            } else {
                value = key;
            }
            return value;
        }

        function getFormatString(key) {
            var input = getString(key);
            var args = Array.prototype.slice.call(arguments, 1);
            return input.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined' ? args[number] : match;
            });
        }

        function getDisplayName(settingLevel, data) {
            var liveName = data.IsLive === true ? ' (' + getString('Live') + ')' : '';
            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return dataService.productName;
                case appSettings.settingLevel.SportType:
                    return data.SportTypeName;
                case appSettings.settingLevel.BetType:
                    return data.BetTypeName + liveName;
                case appSettings.settingLevel.SportTypeBetType:
                    return data.BetTypeName + liveName;
            }
        }

        setLanguage(appSettings.defaultLanguage);
        return {
            setLanguage: setLanguage,
            getString: getString,
            getFormatString: getFormatString,
            getDisplayName: getDisplayName
        };
    };

    fanexResourceService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexResourceService', fanexResourceService);
})();