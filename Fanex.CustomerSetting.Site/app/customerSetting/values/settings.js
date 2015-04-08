'use strict';

(function () {
    var body = angular.element(document.body);
    var rootUrl = body.attr("data-root-url");
    angular.module('customerSettingApp').value('appSettings', {
        rootUrl: rootUrl,
        version: '0.2',
        defaultLanguage: 'en-US',
        defaultMaxBet: 99999999999,
        betLimitMaxLength: 14,
        positionTakingDivider: 100,
        formType: {
            CompanyEditSuper: 1,
            SuperEditMaster: 2,
            MasterEditAgent: 3,
            AgentEditMember: 4,
            SuperEditMember: 5,
            MasterEditMember: 6
        },
        roleType: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4,
            Company: 5
        },
        settingType: {
            All: 0,
            PositionTaking: 1,
            BetLimit: 2,
            Discount: 3,
            Status: 4
        },
        settingLevel: {
            Product: 1,
            SportType: 2,
            BetType: 3,
            SportTypeBetType: 4
        },
        errorType: {
            Success: 0,
            FormInvalid: 1,
            NoChange: 2
        },
        errCode: {
            Success: 0,
            ErrorValue: -1,
            NoChange: -2,
            AccessDenied: -3
        },
        messageType: {
            Success: 0,
            Warning: 1,
            Error: 2
        }
    });
})();