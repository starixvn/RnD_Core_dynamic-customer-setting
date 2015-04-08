'use strict';

(function () {
    var injectParams = ['appSettings', 'controllerService'];
    var templateService = function (appSettings, controllerService) {
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

        var discountFiles = {};
        discountFiles[appSettings.formType.CompanyEditSuper] = 'discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMaster] = 'discount-container.htm';
        discountFiles[appSettings.formType.MasterEditAgent] = 'discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMember] = 'super-edit-member-discount-container.htm';
        discountFiles[appSettings.formType.MasterEditMember] = 'master-edit-member-discount-container.htm';
        discountFiles[appSettings.formType.AgentEditMember] = 'discount-container.htm';

        this.resolveView = function () {
            return 'templates/customer-setting-view.htm';
        };

        this.resolveStatus = function () {
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.StatusLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/status-container.htm'
            : undefined;
        };

        this.resolveDiscount = function () {
            var requestContext = controllerService.getRequestContext();
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.DiscountLevel];
            var fileName = discountFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };

        this.resolveBetLimit = function () {
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.BetLimitLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/betlimit-container.htm'
            : undefined;
        };

        this.resolvePositionTaking = function () {
            var requestContext = controllerService.getRequestContext();
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.PositionTakingLevel];
            var fileName = positionTakingFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };
    };

    templateService.$inject = injectParams;
    angular.module('customerSettingApp').service('templateService', templateService);
})();