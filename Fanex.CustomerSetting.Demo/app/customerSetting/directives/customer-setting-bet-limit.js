'use strict';

(function () {
    var injectParams = ['$rootScope', 'componentService', 'resourceService', 'appSettings'];
    var customerSettingBetLimitDirective = function ($rootScope, componentService, resourceService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                betLimit: '=customerSettingBetLimit',
                settingLevel: '=',
                settingContext: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-bet-limit.htm',
            controller: ['$scope', function ($scope) {
                $scope.appSettings = appSettings;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.betLimit);
                $scope.resourceService = resourceService;
                var betLimit = componentService.parseBetLimit($scope.betLimit, $scope.formType);
                $scope.minBet = betLimit.minBet;
                $scope.maxBet = betLimit.maxBet;
                $scope.maxPerMatch = betLimit.maxPerMatch;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-bet-limit', $scope);
                    };
                }

                $scope.$on('copy-bet-limit', function(event, copyScope){
                    if(copyScope !== $scope){
                        $scope.minBet.value = copyScope.minBet.value;
                        $scope.maxBet.value = copyScope.maxBet.value;
                        $scope.maxPerMatch.value = copyScope.maxPerMatch.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ($scope.minBet.error || $scope.maxBet.error || $scope.maxPerMatch.error) {
                        errCode = appSettings.errorType.FormInvalid;
                    } else if ($scope.betLimit.MinBet === $scope.minBet.value
                        && $scope.betLimit.MaxBet === $scope.maxBet.value
                        && $scope.betLimit.MaxPerMatch === $scope.maxPerMatch.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.betLimit.SportType,
                            BetType: $scope.betLimit.BetType,
                            MinBet: $scope.minBet.value,
                            MaxBet: $scope.maxBet.value,
                            MaxPerMatch: $scope.maxPerMatch.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                var validateMinBet = function () {
                    if (scope.minBet.value === scope.maxBet.value === scope.maxPerMatch.value === 0) {
                        scope.minBet.error = '';
                    }
                    else {
                        if (scope.minBet.value < scope.minBet.limit) {
                            scope.minBet.error = resourceService.getFormatString('MinBetMoreThanOrEqualTo', scope.minBet.limit);
                        }
                        else if (scope.minBet.value > scope.maxBet.value) {
                            scope.minBet.error = resourceService.getString('MinBetLessThanOrEqualMaxBet');
                            if (scope.maxBet.value <= scope.maxPerMatch.value) {
                                scope.maxBet.error = '';
                            }
                        }
                        else {
                            scope.minBet.error = '';
                        }
                    }

                    if (!scope.minBet.error) {
                        if (scope.maxBet.error) {
                            validateMaxBet();
                        }
                    }
                };

                var validateMaxBet = function () {
                    if (scope.minBet.value === scope.maxBet.value === scope.maxPerMatch.value === 0) {
                        scope.maxBet.error = '';
                    }
                    else {
                        if (scope.maxBet.value > scope.maxBet.limit) {
                            scope.maxBet.error = resourceService.getFormatString('MaxBetLessThanOrEqualTo', scope.maxBet.limit);
                        }
                        else if (scope.maxBet.value < scope.minBet.value) {
                            scope.maxBet.error = resourceService.getString('MaxBetMoreThanOrEqualMinBet');
                            scope.minBet.error = '';
                        } else if (scope.maxBet.value > scope.maxPerMatch.value) {
                            scope.maxBet.error = resourceService.getString('MaxBetLessThanOrEqualMaxPerMatch');
                            scope.maxPerMatch.error = '';
                        }
                        else {
                            scope.maxBet.error = '';
                        }
                    }

                    if (!scope.maxBet.error) {
                        if (scope.minBet.error) {
                            validateMinBet();
                        }
                        if (scope.maxPerMatch.error) {
                            validateMaxPerMatch();
                        }
                    }
                };

                var validateMaxPerMatch = function () {
                    if (scope.minBet.value === scope.maxBet.value === scope.maxPerMatch.value === 0) {
                        scope.maxPerMatch.error = '';
                    } else {
                        if (scope.maxPerMatch.value > scope.maxPerMatch.limit) {
                            scope.maxPerMatch.error = resourceService.getFormatString('MaxPerMatchLessThanOrEqualTo', scope.maxPerMatch.limit);
                        } else if (scope.maxPerMatch.value < scope.maxBet.value) {
                            scope.maxPerMatch.error = resourceService.getString('MaxPerMatchMoreThanOrEqualMaxBet');
                            if (scope.maxBet.value >= scope.minBet.value) {
                                scope.maxBet.error = '';
                            }
                        }
                        else {
                            scope.maxPerMatch.error = '';
                        }
                    }

                    if (!scope.maxPerMatch.error) {
                        if (scope.maxBet.error) {
                            validateMaxBet();
                        }
                    }
                };

                validateMinBet();
                validateMaxBet();
                validateMaxPerMatch();

                scope.$watch('minBet.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        validateMinBet();
                    }
                });

                scope.$watch('maxBet.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        validateMaxBet();
                    }
                });

                scope.$watch('maxPerMatch.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        validateMaxPerMatch();
                    }
                });
            }
        };
    };

    customerSettingBetLimitDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingBetLimit', customerSettingBetLimitDirective);
})();