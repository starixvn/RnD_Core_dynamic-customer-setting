'use strict';

(function () {
    var customerSettingApp = angular.module('customerSettingApp', []);
    customerSettingApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$rootScope', 'messageService', 'appSettings',
            function ($q, $rootScope, messageService, appSettings) {
                var blockUI = function () {
                    $rootScope.hasLoading = true;
                };

                var unblockUI = function () {
                    $rootScope.hasLoading = false;
                };

                return {
                    request: function (config) {
                        blockUI();
                        if (config.method === 'GET') {
                            config.url = appSettings.rootUrl + '/app/customerSetting/' + config.url + '?v=' + appSettings.version;
                        }

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