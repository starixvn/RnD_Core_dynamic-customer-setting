'use strict';

(function () {
    var injectParams = ['$rootScope', 'resourceService', 'linkingService', 'appSettings'];
    var errType = {
        none: 0,
        limit: 1,
        minBet: 2,
        maxBet: 3,
        maxPerMatch: 4
    };
    var customerSettingBetLimitDirective = function ($rootScope, resourceService, linkingService, appSettings) {
        function parse(data, formType) {
            var BetLimitInput = function (value, limit) {
                this.errors = [];
                this.value = value;
                this.limit = limit;

                this.setInvalid = function (errType, errMsg) {
                    var hasErrType = false;
                    angular.forEach(this.errors, function (error) {
                        if (!hasErrType) {
                            if (error.errType === errType) {
                                hasErrType = true;
                            }
                        }
                    });

                    if (!hasErrType) {
                        this.errors.push({
                            errType: errType,
                            errMsg: errMsg
                        });
                    }
                };

                this.setValid = function (errType) {
                    var hasErrType = false;
                    var errorIndex = -1;
                    angular.forEach(this.errors, function (error, index) {
                        if (!hasErrType) {
                            if (error.errType === errType) {
                                hasErrType = true;
                                errorIndex = index;
                            }
                        }
                    });

                    if (hasErrType) {
                        this.errors.splice(errorIndex, 1);
                    }
                };

                this.clearErrors = function () {
                    this.errors.length = 0;
                };

                this.isValid = function () {
                    return this.errors.length === 0;
                };

                this.errorMessage = function () {
                    if (this.errors.length === 0) {
                        return undefined;
                    }

                    return this.errors[0].errMsg;
                };
            };

            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, appSettings.defaultMaxBet),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, appSettings.defaultMaxBet)
                    };

                case appSettings.formType.SuperEditMaster:
                    return {};

                case appSettings.formType.MasterEditAgent:
                    return {};

                case appSettings.formType.SuperEditMember:
                    return {};

                case appSettings.formType.MasterEditMember:
                    return {};

                case appSettings.formType.AgentEditMember:
                    return {};
            }
        }

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
                linkingService.linkBetLimit($scope);
                $scope.appSettings = appSettings;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.betLimit);
                $scope.resourceService = resourceService;
                var betLimit = parse($scope.betLimit, $scope.formType);
                $scope.minBet = betLimit.minBet;
                $scope.maxBet = betLimit.maxBet;
                $scope.maxPerMatch = betLimit.maxPerMatch;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-bet-limit', $scope);
                    };
                }

                $scope.$on('copy-bet-limit', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.minBet.value = copyScope.minBet.value;
                        $scope.maxBet.value = copyScope.maxBet.value;
                        $scope.maxPerMatch.value = copyScope.maxPerMatch.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (!$scope.minBet.isValid() || !$scope.maxBet.isValid() || !$scope.maxPerMatch.isValid()) {
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
                        scope.minBet.clearErrors();
                        scope.maxBet.clearErrors();
                        scope.minPerMatch.clearErrors();
                    }

                    if (scope.minBet.value < scope.minBet.limit) {
                        scope.minBet.setInvalid(errType.limit, resourceService.getFormatString('MinBetMoreThanOrEqualTo', scope.minBet.limit));
                    }
                    else {
                        scope.minBet.setValid(errType.limit);
                    }

                    scope.maxBet.setValid(errType.minBet);
                    if (scope.minBet.value > scope.maxBet.value) {
                        scope.minBet.setInvalid(errType.maxBet, resourceService.getString('MinBetLessThanOrEqualMaxBet'));
                    }
                    else {
                        scope.minBet.setValid(errType.maxBet);
                    }
                };

                var validateMaxBet = function () {
                    if (scope.minBet.value === scope.maxBet.value === scope.maxPerMatch.value === 0) {
                        scope.minBet.clearErrors();
                        scope.maxBet.clearErrors();
                        scope.minPerMatch.clearErrors();
                    }

                    if (scope.maxBet.value > scope.maxBet.limit) {
                        scope.maxBet.setInvalid(errType.limit, resourceService.getFormatString('MaxBetLessThanOrEqualTo', scope.maxBet.limit));
                    }
                    else {
                        scope.maxBet.setValid(errType.limit);
                    }

                    scope.minBet.setValid(errType.maxBet);
                    if (scope.maxBet.value < scope.minBet.value) {
                        scope.maxBet.setInvalid(errType.minBet, resourceService.getString('MaxBetMoreThanOrEqualMinBet'));
                    } else {
                        scope.maxBet.setValid(errType.minBet);
                    }

                    scope.maxPerMatch.setValid(errType.maxBet);
                    if (scope.maxBet.value > scope.maxPerMatch.value) {
                        scope.maxBet.setInvalid(errType.maxPerMatch, resourceService.getString('MaxBetLessThanOrEqualMaxPerMatch'));
                    }
                    else {
                        scope.maxBet.setValid(errType.maxPerMatch);
                    }
                };

                var validateMaxPerMatch = function () {
                    if (scope.minBet.value === scope.maxBet.value === scope.maxPerMatch.value === 0) {
                        scope.minBet.clearErrors();
                        scope.maxBet.clearErrors();
                        scope.minPerMatch.clearErrors();
                        return;
                    }

                    if (scope.maxPerMatch.value > scope.maxPerMatch.limit) {
                        var errMsg = resourceService.getFormatString('MaxPerMatchLessThanOrEqualTo', scope.maxPerMatch.limit);
                        scope.maxPerMatch.setInvalid(errType.limit, errMsg);
                    } else {
                        scope.maxPerMatch.setValid(errType.limit);
                    }

                    scope.maxBet.setValid(errType.maxPerMatch);
                    if (scope.maxPerMatch.value < scope.maxBet.value) {
                        scope.maxPerMatch.setInvalid(errType.maxBet, resourceService.getString('MaxPerMatchMoreThanOrEqualMaxBet'));
                    }
                    else {
                        scope.maxPerMatch.setValid(errType.maxBet);
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