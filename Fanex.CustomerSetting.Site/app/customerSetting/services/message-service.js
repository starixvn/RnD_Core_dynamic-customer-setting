'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', '$sce', 'appSettings'];
    var messageService = function ($rootScope, commonService, $sce, appSettings) {
        var messageClasses = {};
        messageClasses[appSettings.messageType.Success] = 'success';
        messageClasses[appSettings.messageType.Warning] = 'warning';
        messageClasses[appSettings.messageType.Error] = 'error';

        var show = function (message, messageType) {
            $rootScope.hasMessage = true;
            $rootScope.messageClass = messageClasses[messageType];
            $rootScope.message = $sce.trustAsHtml(message);
            commonService.scrollToTop();
        };

        this.showSuccess = function (message) {
            show(message || 'Undefined success', appSettings.messageType.Success);
        };

        this.showWarning = function (message) {
            show(message || 'Undefined warning', appSettings.messageType.Warning);
        };

        this.showError = function (message) {
            show(message || 'Undefined error', appSettings.messageType.Error);
        };

        this.hide = function () {
            $rootScope.hasMessage = false;
            $rootScope.message = undefined;
        };
    };

    messageService.$inject = injectParams;
    angular.module('customerSettingApp').service('messageService', messageService);
})();