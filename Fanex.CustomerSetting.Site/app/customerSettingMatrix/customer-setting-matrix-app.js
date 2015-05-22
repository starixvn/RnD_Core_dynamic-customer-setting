(function () {
    'use strict';
    var customerSettingMatrixApp = angular.module('customerSettingMatrixApp', []);
    customerSettingMatrixApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$rootScope', '$templateCache', 'fanexBlockUiService',
            function ($q, $rootScope, $templateCache, blockUiService) {
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

                        $rootScope.showMessage('Sorry, an error occurred while processing your request.');
                        return $q.reject(response);
                    }
                };
            }
        ]);
    } ]);
} ());