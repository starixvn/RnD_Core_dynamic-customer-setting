'use strict';

(function () {
    var customerSettingApp = angular.module('customerSettingApp', []);
    customerSettingApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$rootScope', 'messageService', 'loadingService', 'appSettings',
            function ($q, $rootScope, messageService, loadingService, appSettings) {
                return {
                    request: function (config) {
                        loadingService.block();
                        if (config.method === 'GET') {
                            config.url = appSettings.rootUrl + '/app/customerSetting/' + config.url + '?v=' + appSettings.version;
                        }

                        return config;
                    },
                    response: function (response) {
                        loadingService.unblock();
                        return response;
                    },
                    responseError: function (response) {
                        loadingService.unblock();
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