'use strict';

(function () {
    var customerSettingApp = angular.module('customerSettingApp', []);

    customerSettingApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$rootScope', 'errorService', 'appSettings',
            function ($q, $rootScope, errorService, appSettings) {
                return {
                    request: function (config) {
                        if (config.method === 'GET') {
                            config.url += '?v=' + appSettings.version;
                        }

                        return config;
                    },
                    responseError: function (response) {
                        errorService.show('Sorry, an error occurred while processing your request.');
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