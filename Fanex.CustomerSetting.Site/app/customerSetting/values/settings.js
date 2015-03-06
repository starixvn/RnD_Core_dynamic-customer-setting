'use strict';

(function () {
    angular.module('customerSettingApp').value('appSettings', {
        version: '0.1',
        defaultLanguage: 'en-US',
        defaultMaxBet: 99999999999,
        betLimitMaxLength: 14,
        positionTakingStep: 0.01,
        positionTakingDivider: 100,
        formType: {
            CompanyEditSuper: 1,
            SuperEditMaster: 2,
            MasterEditAgent: 3,
            SuperEditMember: 4,
            MasterEditMember: 5,
            AgentEditMember: 6
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
        messageType: {
            Success: 0,
            Warning: 1,
            Error: 2
        }
    });
})();