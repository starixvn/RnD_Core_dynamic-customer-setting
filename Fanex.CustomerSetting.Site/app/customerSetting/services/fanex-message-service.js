'use strict';

(function () {
    var injectParams = ['$timeout', '$sce', 'fanexCommonService', 'fanexAppSettings', 'fanexResourceService'];
    var fanexMessageService = function ($timeout, $sce, commonService, appSettings, resourceService) {
        var messageScope;
        var messageClasses = {};
        messageClasses[appSettings.messageType.Success] = 'success';
        messageClasses[appSettings.messageType.Warning] = 'warning';
        messageClasses[appSettings.messageType.Error] = 'error';

        var Message = function (messageType, messageContent) {
            this.MessageType = messageType;
            this.MessageContent = messageContent;
        };

        function setScopeMessages(messages) {
            if (angular.isDefined(messageScope)) {
                messageScope.messages = messages;
            }
        }

        function generateMesage(result) {
            var messageType = appSettings.messageType.Error;
            switch (result.ErrCode) {
                case appSettings.errorCode.Success:
                    messageType = appSettings.messageType.Success;
                    break;

                case appSettings.errorCode.NoChange:
                    messageType = appSettings.messageType.Warning;
                    break;
            }

            var messageContent = result.ErrMsg || 'Undefined messsage';
            return new Message(messageType, messageContent);
        }

        function showMessage(message) {
            var htmlMessages = [];
            htmlMessages.push({
                messageClass: messageClasses[message.MessageType],
                messageContent: $sce.trustAsHtml(message.MessageContent)
            });
            commonService.scrollToTop();

            $timeout(function () {
                setScopeMessages(htmlMessages);
            });
        }

        function showMessages(messages) {
            var htmlMessages = [];
            angular.forEach(messages, function (message) {
                htmlMessages.push({
                    messageClass: messageClasses[message.MessageType],
                    messageContent: $sce.trustAsHtml(message.MessageContent)
                });
            });
            commonService.scrollToTop();

            $timeout(function () {
                setScopeMessages(htmlMessages);
            });
        }

        this.setup = function (scope) {
            messageScope = scope;
        };

        this.showMessage = function (messageType, messageContent) {
            var message = new Message(messageType, messageContent);
            showMessage(message);
        };

        this.showResult = function (result) {
            var message = generateMesage(result);
            showMessage(message);
        };

        this.showResults = function (results) {
            // if having any successful or error messages -> remove any no-change messages
            var messages = [];
            angular.forEach(results, function (result) {
                if (result.ErrCode !== appSettings.errorCode.NoChange) {
                    var message = generateMesage(result);
                    messages.push(message);
                }
            });

            if (messages.length === 0) {
                this.showNoChange();
            } else {
                showMessages(messages);
            }
        };

        this.showSuccess = function (messageContent) {
            this.showMessage(appSettings.messageType.Success, messageContent || 'Undefined success message');
        };

        this.showWarning = function (messageContent) {
            this.showMessage(appSettings.messageType.Warning, messageContent || 'Undefined warning message');
        };

        this.showNoChange = function () {
            this.showWarning(resourceService.getString('NoChangeToUpdate'));
        };

        this.showError = function (messageContent) {
            this.showMessage(appSettings.messageType.Error, messageContent || 'Undefined error message');
        };

        this.hide = function () {
            setScopeMessages(undefined);
        };
    };

    fanexMessageService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexMessageService', fanexMessageService);
})();