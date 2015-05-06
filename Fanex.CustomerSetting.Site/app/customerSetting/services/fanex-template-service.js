'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDataService'];
    var fanexTemplateService = function (appSettings, dataService) {
        var levelFolders = {};
        levelFolders[appSettings.settingLevel.Product] = 'no-group';
        levelFolders[appSettings.settingLevel.SportType] = 'no-group';
        levelFolders[appSettings.settingLevel.BetType] = 'no-group';
        levelFolders[appSettings.settingLevel.SportTypeBetType] = 'sport-group';

        var positionTakingFiles = {};
        positionTakingFiles[appSettings.formType.CompanyEditSuper] = 'fanex-pt-company-edit-super-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMaster] = 'fanex-pt-super-edit-master-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditAgent] = 'fanex-pt-master-edit-agent-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMember] = 'fanex-pt-super-edit-member-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditMember] = 'fanex-pt-master-edit-member-container.htm';
        positionTakingFiles[appSettings.formType.AgentEditMember] = 'fanex-pt-agent-edit-member-container.htm';

        var discountFiles = {};
        discountFiles[appSettings.formType.CompanyEditSuper] = 'fanex-discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMaster] = 'fanex-discount-container.htm';
        discountFiles[appSettings.formType.MasterEditAgent] = 'fanex-discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMember] = 'fanex-discount-super-edit-member-container.htm';
        discountFiles[appSettings.formType.MasterEditMember] = 'fanex-discount-master-edit-member-container.htm';
        discountFiles[appSettings.formType.AgentEditMember] = 'fanex-discount-container.htm';

        this.resolveStatus = function () {
            var queryData = dataService.queryData;
            var settingContext = queryData.SettingContext;
            var levelFolder = levelFolders[settingContext.StatusLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/fanex-status-container.htm'
            : undefined;
        };

        this.resolveDiscount = function () {
            var queryData = dataService.queryData;
            var settingContext = queryData.SettingContext;
            var requestContext = queryData.RequestContext;
            var levelFolder = levelFolders[settingContext.DiscountLevel];
            var fileName = discountFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };

        this.resolveBetLimit = function () {
            var queryData = dataService.queryData;
            var settingContext = queryData.SettingContext;
            var levelFolder = levelFolders[settingContext.BetLimitLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/fanex-bet-limit-container.htm'
            : undefined;
        };

        this.resolvePositionTaking = function () {
            var queryData = dataService.queryData;
            var settingContext = queryData.SettingContext;
            var requestContext = queryData.RequestContext;
            var levelFolder = levelFolders[settingContext.PositionTakingLevel];
            var fileName = positionTakingFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };
    };

    fanexTemplateService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexTemplateService', fanexTemplateService);
})();