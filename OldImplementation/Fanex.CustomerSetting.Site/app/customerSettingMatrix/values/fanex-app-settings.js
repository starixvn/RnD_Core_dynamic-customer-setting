(function () {
    'use strict';
    var body = angular.element(document.body);
    var rootUrl = body.attr('data-root-url');
    angular.module('customerSettingMatrixApp').value('fanexAppSettings', {
        rootUrl: rootUrl,
        roleType: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4
        },
        settingLevel: {
            Product: 1,
            SportType: 2,
            BetType: 3,
            SportTypeBetType: 4
        },
        settingType: {
            PositionTaking: 0,
            Commission: 1
        }
    });
})();