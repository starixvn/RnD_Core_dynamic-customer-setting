'use strict';

(function () {
    var customerSettingApp = angular.module('customerSettingApp', []);
    customerSettingApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$window', '$rootScope', 'messageService',
            function ($q, $window, $rootScope, messageService) {
                var blockUI = function () {
                    $rootScope.hasLoading = true;
                };

                var unblockUI = function () {
                    $rootScope.hasLoading = false;
                };

                return {
                    request: function (config) {
                        blockUI();
                        return config;
                    },
                    response: function (response) {
                        unblockUI();
                        return response;
                    },
                    responseError: function (response) {
                        unblockUI();
                        messageService.showError('Sorry, an error occurred while processing your request.');
                        return $q.reject(response);
                    }
                };
            } ]);
    } ]);

    customerSettingApp.run(['$rootScope', 'resourceService', 'appSettings', function ($rootScope, resourceService, appSettings) {
        $rootScope.title = 'Customer Setting Renovation';
        resourceService.setLanguage(appSettings.defaultLanguage);
        $rootScope.resourceService = resourceService;
    } ]);
} ());