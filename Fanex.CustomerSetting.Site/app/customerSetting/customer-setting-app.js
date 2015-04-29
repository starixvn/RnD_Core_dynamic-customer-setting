'use strict';

(function () {
    var customerSettingApp = angular.module('customerSettingApp', []);
    customerSettingApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$window', '$rootScope', '$templateCache', 'fanexMessageService', 'fanexBlockUiService',
            function ($q, $window, $rootScope, $templateCache, messageService, blockUiService) {
                function startBlockUi(config) {
                    config.noBlock = (config.method === 'GET' && angular.isDefined($templateCache.get(config.url)));
                    if (!config.noBlock) {
                        blockUiService.show();
                    }
                }

                function stopBlockUi(config) {
                    if (!config.noBlock) {
                        blockUiService.hide();
                    }
                }

                return {
                    request: function (config) {
                        startBlockUi(config);
                        return config;
                    },
                    response: function (response) {
                        if (response) {
                            stopBlockUi(response.config);
                        }

                        return response;
                    },
                    responseError: function (response) {
                        if (response) {
                            stopBlockUi(response.config);
                        }

                        messageService.showError('Sorry, an error occurred while processing your request.');
                        return $q.reject(response);
                    }
                };
            }
        ]);
    } ]);

    customerSettingApp.run(['$rootScope', 'fanexResourceService', 'fanexAppSettings',
        function ($rootScope, resourceService, appSettings) {
            resourceService.setLanguage(appSettings.defaultLanguage);
            $rootScope.resourceService = resourceService;
        }
     ]);
} ());