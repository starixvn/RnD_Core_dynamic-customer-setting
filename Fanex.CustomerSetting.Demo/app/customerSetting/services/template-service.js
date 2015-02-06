'use strict';

(function () {
    var injectParams = ['appSettings'];
    var templateService = function (appSettings) {
        var levelFolders = {};
        levelFolders[appSettings.settingLevel.Product] = 'no-group';
        levelFolders[appSettings.settingLevel.SportType] = 'no-group';
        levelFolders[appSettings.settingLevel.BetType] = 'no-group';
        levelFolders[appSettings.settingLevel.SportTypeBetType] = 'sport-group';

        var positionTakingFiles = {};
        positionTakingFiles[appSettings.formType.CompanyEditSuper] = 'company-edit-super-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMaster] = 'super-edit-master-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditAgent] = 'master-edit-agent-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMember] = 'super-edit-member-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditMember] = 'master-edit-member-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.AgentEditMember] = 'agent-edit-member-position-taking-container.htm';

        this.resolveView = function (productId) {
            return productId > 0
                ? '/app/customerSetting/templates/customer-setting-view.htm'
                : undefined;
        };

        this.resolveStatus = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/status-container.htm'
            : undefined;
        };

        this.resolveDiscount = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/discount-container.htm'
            : undefined;
        };

        this.resolveBetLimit = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/betlimit-container.htm'
            : undefined;
        };

        this.resolvePositionTaking = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            var fileName = positionTakingFiles[formType];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/' + fileName
            : undefined;
        };
    };

    templateService.$inject = injectParams;
    angular.module('customerSettingApp').service('templateService', templateService);
})();