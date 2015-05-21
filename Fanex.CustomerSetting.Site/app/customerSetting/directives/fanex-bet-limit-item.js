(function () {
    'use strict';
    var injectParams = ['$filter', 'fanexAppSettings', 'fanexBetLimitService', 'fanexResourceService'];
    var errorType = {
        Limit: 1,
        MinBet: 2,
        MaxBet: 3,
        MaxPerMatch: 4
    };
    var fanexBetLimitItemDirective = function ($filter, appSettings, betLimitService, resourceService) {
        function parse(data, formType) {
            var BetLimitInput = function (value, limit) {
                this.errors = [];
                this.value = value;
                this.limit = limit;

                this.setInvalid = function (errorType, errMsg) {
                    var hasErrType = false;
                    angular.forEach(this.errors, function (error) {
                        if (!hasErrType) {
                            if (error.errorType === errorType) {
                                hasErrType = true;
                            }
                        }
                    });

                    if (!hasErrType) {
                        this.errors.push({
                            errorType: errorType,
                            errMsg: errMsg
                        });
                    }
                };

                this.setValid = function (errorType) {
                    var hasErrType = false;
                    var errorIndex = -1;
                    angular.forEach(this.errors, function (error, index) {
                        if (!hasErrType) {
                            if (error.errorType === errorType) {
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
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, data.MaxBetMax),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, data.MaxPerMatchMax)
                    };

                case appSettings.formType.MasterEditAgent:
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, data.MaxBetMax),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, data.MaxPerMatchMax)
                    };

                case appSettings.formType.SuperEditMember:
                    return {
                        minBet: data.MinBet,
                        maxBet: data.MaxBet,
                        maxPerMatch: data.MaxPerMatch
                    };

                case appSettings.formType.MasterEditMember:
                    return {
                        minBet: data.MinBet,
                        maxBet: data.MaxBet,
                        maxPerMatch: data.MaxPerMatch
                    };

                case appSettings.formType.AgentEditMember:
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, data.MaxBetMax),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, data.MaxPerMatchMax)
                    };

                default:
                    throw 'Not implemented yet.';
            }
        }

        return {
            restrict: 'AE',
            replace: true,
            scope: {
                betLimit: '=fanexBetLimitItem',
                settingLevel: '=',
                formType: '='
            },
            template: '<div data-ng-include="templateUrl"></div>',
            controller: ['$scope', function ($scope) {
                betLimitService.linkScope($scope);

                var formType = $scope.formType;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.betLimit);
                var betLimit = $scope.betLimit;
                $scope.isReadonly = formType === appSettings.formType.SuperEditMember
                    || formType === appSettings.formType.MasterEditMember;
                $scope.templateUrl = $scope.isReadonly
                    ? 'templates/fanex-bet-limit-item-readonly.htm'
                    : 'templates/fanex-bet-limit-item.htm';

                var data = parse(betLimit, formType);
                $scope.minBet = data.minBet;
                $scope.hasMinBetLimit = $scope.minBet.limit > 0;
                $scope.maxBet = data.maxBet;
                $scope.hasMaxBetLimit = formType !== appSettings.formType.CompanyEditSuper;
                $scope.maxPerMatch = data.maxPerMatch;
                $scope.hasMaxPerMatchLimit = $scope.hasMaxBetLimit;
                $scope.appSettings = appSettings;

                if ($scope.isReadonly) {
                    return;
                }

                $scope.copy = function (copyScope) {
                    $scope.minBet.value = copyScope.minBet.value;
                    $scope.maxBet.value = copyScope.maxBet.value;
                    $scope.maxPerMatch.value = copyScope.maxPerMatch.value;
                };

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (!$scope.minBet.isValid() || !$scope.maxBet.isValid() || !$scope.maxPerMatch.isValid()) {
                        errCode = appSettings.errorType.FormInvalid;
                    } else if (betLimit.MinBet === $scope.minBet.value
                        && betLimit.MaxBet === $scope.maxBet.value
                        && betLimit.MaxPerMatch === $scope.maxPerMatch.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: betLimit.SportType,
                            BetType: betLimit.BetType,
                            MinBet: $scope.minBet.value,
                            MaxBet: $scope.maxBet.value,
                            MaxPerMatch: $scope.maxPerMatch.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                if (scope.isReadonly) {
                    return;
                }

                var validateCount = 0;

                function resetValidate() {
                    validateCount = 0;
                }

                function hasValidate() {
                    validateCount++;
                    if (validateCount === 3) {
                        validateCount = 0;
                        return false;
                    }

                    if (scope.minBet.value === scope.maxBet.value
                        && scope.maxBet.value === scope.maxPerMatch.value
                        && scope.maxPerMatch.value === 0) {
                        scope.minBet.clearErrors();
                        scope.maxBet.clearErrors();
                        scope.maxPerMatch.clearErrors();
                        return false;
                    }

                    return true;
                }

                var validateMinBet = function () {
                    if (!hasValidate()) {
                        return;
                    }

                    if (scope.minBet.value < scope.minBet.limit) {
                        var formattedNumber = $filter('number')(scope.minBet.limit);
                        scope.minBet.setInvalid(errorType.Limit, resourceService.getFormatString('MinBetMoreThanOrEqualTo', formattedNumber));
                    }
                    else {
                        scope.minBet.setValid(errorType.Limit);
                    }

                    scope.maxBet.setValid(errorType.MinBet);
                    if (scope.minBet.value > scope.maxBet.value) {
                        scope.minBet.setInvalid(errorType.MaxBet, resourceService.getString('MinBetLessThanOrEqualMaxBet'));
                    }
                    else {
                        scope.minBet.setValid(errorType.MaxBet);
                    }

                    if (scope.minBet.isValid() && scope.maxPerMatch.isValid()) {
                        validateMaxBet();
                    }
                };

                var validateMaxBet = function () {
                    if (!hasValidate()) {
                        return;
                    }

                    if (scope.maxBet.value > scope.maxBet.limit) {
                        var formattedNumber = $filter('number')(scope.maxBet.limit);
                        scope.maxBet.setInvalid(errorType.Limit, resourceService.getFormatString('MaxBetLessThanOrEqualTo', formattedNumber));
                    }
                    else {
                        scope.maxBet.setValid(errorType.Limit);
                    }

                    scope.minBet.setValid(errorType.MaxBet);
                    if (scope.maxBet.value < scope.minBet.value) {
                        scope.maxBet.setInvalid(errorType.MinBet, resourceService.getString('MaxBetMoreThanOrEqualMinBet'));
                    } else {
                        scope.maxBet.setValid(errorType.MinBet);
                    }

                    scope.maxPerMatch.setValid(errorType.MaxBet);
                    if (scope.maxBet.value > scope.maxPerMatch.value) {
                        scope.maxBet.setInvalid(errorType.MaxPerMatch, resourceService.getString('MaxBetLessThanOrEqualMaxPerMatch'));
                    }
                    else {
                        scope.maxBet.setValid(errorType.MaxPerMatch);
                    }

                    if (scope.minBet.isValid() && scope.maxBet.isValid()) {
                        validateMaxPerMatch();
                    }
                };

                var validateMaxPerMatch = function () {
                    if (!hasValidate()) {
                        return;
                    }

                    if (scope.maxPerMatch.value > scope.maxPerMatch.limit) {
                        var errMsg = resourceService.getFormatString('MaxPerMatchLessThanOrEqualTo', $filter('number')(scope.maxPerMatch.limit));
                        scope.maxPerMatch.setInvalid(errorType.Limit, errMsg);
                    } else {
                        scope.maxPerMatch.setValid(errorType.Limit);
                    }

                    scope.maxBet.setValid(errorType.MaxPerMatch);
                    if (scope.maxPerMatch.value < scope.maxBet.value) {
                        scope.maxPerMatch.setInvalid(errorType.MaxBet, resourceService.getString('MaxPerMatchMoreThanOrEqualMaxBet'));
                    }
                    else {
                        scope.maxPerMatch.setValid(errorType.MaxBet);
                    }

                    if (scope.maxBet.isValid() && scope.maxPerMatch.isValid()) {
                        validateMinBet();
                    }
                };

                resetValidate();
                validateMinBet();

                scope.$watch('minBet.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        resetValidate();
                        validateMinBet();
                    }
                });

                scope.$watch('maxBet.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        resetValidate();
                        validateMaxBet();
                    }
                });

                scope.$watch('maxPerMatch.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        resetValidate();
                        validateMaxPerMatch();
                    }
                });
            }
        };
    };

    fanexBetLimitItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitItem', fanexBetLimitItemDirective);
})();