(function () {
    'use strict';
    var injectParams = ['$rootScope', '$timeout', '$sce', 'fanexCommonService', 'fanexAppSettings', 'fanexResourceService'];
    var fanexMessageService = function ($rootScope, $timeout, $sce, commonService, appSettings, resourceService) {
        var messageScope;
        var messageOptions = {
            RemoveNoChange: true
        };
        var messageClasses = {};
        messageClasses[appSettings.messageType.Success] = 'success';
        messageClasses[appSettings.messageType.Warning] = 'warning';
        messageClasses[appSettings.messageType.Error] = 'error';

        var Message = function (type, content) {
            return {
                'class': messageClasses[type],
                content: $sce.trustAsHtml(content)
            };
        };

        Message.fromResult = function (result) {
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
        };

        Message.fromResults = function (results) {
            var messages = [];
            angular.forEach(results, function (result) {
                var message = Message.fromResult(result);
                messages.push(message);
            });

            return messages;
        };

        var User = function (name, messages) {
            return {
                name: name,
                messages: messages
            };
        };

        function setup(scope) {
            messageScope = scope;
        }

        function config(options) {
            messageOptions = angular.extend(options, messageOptions);
        }

        function showSuccess(messageContent) {
            var message = new Message(appSettings.messageType.Success, messageContent || 'Undefined success message');
            showMessage(message);
        }

        function showWarning(messageContent) {
            var message = new Message(appSettings.messageType.Warning, messageContent || 'Undefined warning message');
            showMessage(message);
        }

        function showNoChange() {
            showWarning(resourceService.getString('NoChangeToUpdate'));
        }

        function showError(messageContent) {
            var message = new Message(appSettings.messageType.Error, messageContent || 'Undefined error message');
            showMessage(message);
        }

        function showMessage(message) {
            var messages = [message];
            showMessages(messages);
        }

        function showMessages(messages) {
            showUserMessages(undefined, messages);
        }

        function showUserMessages(userName, userMessages) {
            var user = new User(userName, userMessages);
            showUser(user);
        }

        function showUser(user) {
            var users = [user];
            showUsers(users);
        }

        function showUsers(users) {
            commonService.scrollToTop();

            $timeout(function () {
                messageScope.users = users;
            });
        }

        function filter(settingResults) {
            if (messageOptions.RemoveNoChange) {
                return filterRemoveNoChange(settingResults);
            }

            return settingResults;
        }

        // if having any successful or error messages -> remove any no-change results
        function filterRemoveNoChange(settingResults) {
            var filteredSettingResults = [];
            angular.forEach(settingResults, function (settingResult) {
                if (settingResult.ErrCode !== appSettings.errorCode.NoChange) {
                    filteredSettingResults.push(settingResult);
                }
            });

            if (filteredSettingResults.length === 0) {
                filteredSettingResults = settingResults[0];
            }

            return filteredSettingResults;
        }

        // user has list of results: position taking result, bet limit result, discount result, status result.
        function showSingle(commandResult) {
            var settingResults = commandResult.SettingResults;
            settingResults = filter(settingResults);
            var messages = Message.fromResults(settingResults);
            showMessages(messages);
        }

        function showMultiple(userNames, commandResults) {
            var users = [];
            angular.forEach(commandResults, function (commandResult, index) {
                var userName = userNames[index];
                var userMessages;
                if (commandResult.IsSuccess) {
                    var settingResults = commandResult.SettingResults;
                    settingResults = filter(settingResults);
                    userMessages = Message.fromResults(settingResults);
                } else {
                    var message = Message.fromResult(commandResult);
                    userMessages = [message];
                }
                var user = new User(userName, userMessages);
                users.push(user);
            });

            showUsers(users);
        }

        function hide() {
            if (angular.isDefined(messageScope)) {
                messageScope.users = undefined;
            }
        }

        $rootScope.$on('product-view-loaded', function () {
            hide();
        });

        return {
            setup: setup,
            config: config,
            hide: hide,
            showSuccess: showSuccess,
            showWarning: showWarning,
            showNoChange: showNoChange,
            showError: showError,
            showSingle: showSingle,
            showMultiple: showMultiple
        };
    };

    fanexMessageService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexMessageService', fanexMessageService);
})();