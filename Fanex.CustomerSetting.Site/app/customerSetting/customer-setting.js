// customer-setting - v0.0.1 - 2015-03-27 10:03:05

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
                            config.url += '?v=' + appSettings.version;
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

'use strict';

(function () {
    var injectParams = ['$scope', 'templateService', 'controllerService', 'appSettings'];
    var customerSettingController = function ($scope, templateService, controllerService, appSettings) {
        controllerService.select(function () {
            $scope.information = controllerService.information;
            $scope.statuses = controllerService.statuses;
            $scope.statusTemplate = templateService.resolveStatus();
            $scope.discounts = controllerService.discounts;
            $scope.hasUpdateDiscount = controllerService.formType !== appSettings.formType.SuperEditMember
                && controllerService.formType !== appSettings.formType.MasterEditMember;
            $scope.discountTemplate = templateService.resolveDiscount();
            $scope.betLimits = controllerService.betLimits;
            $scope.hasUpdateBetLimit = $scope.hasUpdateDiscount;
            $scope.betLimitTemplate = templateService.resolveBetLimit();
            $scope.positionTakings = controllerService.positionTakings;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking();
            if (controllerService.formType === appSettings.formType.MasterEditAgent) {
                $scope.disableAutoMPT = controllerService.settingContext.DisableAutoMPT;
            }
        });

        $scope.updateStatuses = function () {
            controllerService.update(appSettings.settingType.Status);
        };

        $scope.updateDiscounts = function () {
            controllerService.update(appSettings.settingType.Discount);
        };

        $scope.updateBetLimits = function () {
            controllerService.update(appSettings.settingType.BetLimit);
        };

        $scope.updatePositionTakings = function () {
            controllerService.update(appSettings.settingType.PositionTaking);
        };

        $scope.updateAll = function () {
            controllerService.update(appSettings.settingType.All);
        };
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());

'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', '$timeout', 'modelService', 'templateService', 'messageService'];
    var customerSettingViewController = function ($scope, $rootScope, $timeout, modelService, templateService, messageService) {
        $rootScope.$on('$locationChangeStart', function () {
            $scope.view = undefined;
            messageService.hide();
        });

        $rootScope.$on('$locationChangeSuccess', function () {
            if (!modelService.isValid) {
                messageService.showError('Please check your URL');
            } else {
                $timeout(function () {
                    var modelParams = modelService.params;
                    $scope.view = templateService.resolveView(modelParams.productId);
                });
            }
        });
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var agentEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            return {
                agentPT: commonService.createPositionTakingSelect(data.N2, data.A2, data.A1)
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=agentEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/agent-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.agentPT = positionTaking.agentPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.agentPT.value = copyScope.agentPT.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ($scope.positionTaking.A1 === $scope.agentPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            BetType: $scope.positionTaking.BetType,
                            IsLive: $scope.positionTaking.IsLive,
                            A1: $scope.agentPT.value
                        }
                    };
                };
            } ]
        };
    };

    agentEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('agentEditMemberPositionTaking', agentEditMemberPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['$filter', 'commonService', 'tooltipService', 'appSettings'];
    var betLimitInputDirective = function ($filter, commonService, tooltipService, appSettings) {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                errorMessage: '='
            },
            link: function (scope, element, attrs, modelCtrl) {
                attrs.$set('maxlength', appSettings.betLimitMaxLength);

                scope.$watch('errorMessage', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        if (angular.isDefined(scope.errorMessage)) {
                            if (element[0] === document.activeElement) {
                                tooltipService.show(element, scope.errorMessage);
                            }
                        } else {
                            if (element[0] === document.activeElement) {
                                tooltipService.hide();
                            }
                        }
                    }
                });

                element.on('focus', function () {
                    if (angular.isDefined(scope.errorMessage)) {
                        tooltipService.show(element, scope.errorMessage);
                    }
                });

                element.on('blur', function () {
                    if (angular.isDefined(scope.errorMessage)) {
                        tooltipService.hide();
                    }
                });

                element.on('keydown', function (event) {
                    if (modelCtrl.$modelValue === 0
                        && commonService.inArray(event.keyCode, [46, 8]) !== -1) {
                        event.preventDefault();
                        return;
                    }

                    if (commonService.inArray(event.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 // Allow: backspace, delete, tab, escape, enter
                        || (event.keyCode === 65 && event.ctrlKey === true) // Allow: Ctrl+A
                        || (event.keyCode >= 35 && event.keyCode <= 40)) { // Allow: home, end, left, right, down, up
                        // let it happen, don't do anything
                        return;
                    }

                    if ((event.shiftKey || (event.keyCode < 48 || event.keyCode > 57)) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                        return;
                    }
                });

                modelCtrl.$formatters.unshift(function () {
                    return $filter('number')(modelCtrl.$modelValue);
                });

                modelCtrl.$parsers.unshift(function (viewValue) {
                    var value = 0;
                    if (viewValue) {
                        var plainNumber = viewValue.replace(/[\,\.]/g, '');
                        value = parseInt(plainNumber);
                    }

                    element.val($filter('number')(value));
                    return value;
                });
            }
        };
    };

    betLimitInputDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('betLimitInput', betLimitInputDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var companyEditSuperPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            return {
                superMaxPT: commonService.createPositionTakingSelect(0, data.DefaultMaxPT, data.S4),
                masterMaxPT: commonService.createPositionTakingSelect(0, data.S4, data.M4),
                agentMaxPT: commonService.createPositionTakingSelect(0, data.M4, data.A4),
                superMinPT: commonService.createPositionTakingSelect(0, data.S4, data.N4)
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=companyEditSuperPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/company-edit-super-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.superMaxPT = positionTaking.superMaxPT;
                $scope.masterMaxPT = positionTaking.masterMaxPT;
                $scope.agentMaxPT = positionTaking.agentMaxPT;
                $scope.superMinPT = positionTaking.superMinPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.superMaxPT.value = copyScope.superMaxPT.value;
                        $scope.masterMaxPT.value = copyScope.masterMaxPT.value;
                        $scope.agentMaxPT.value = copyScope.agentMaxPT.value;
                        $scope.superMinPT.value = copyScope.superMinPT.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ($scope.positionTaking.S4 === $scope.superMaxPT.value
                        && $scope.positionTaking.M4 === $scope.masterMaxPT.value
                        && $scope.positionTaking.A4 === $scope.agentMaxPT.value
                        && $scope.positionTaking.N4 === $scope.superMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            BetType: $scope.positionTaking.BetType,
                            IsLive: $scope.positionTaking.IsLive,
                            S4: $scope.superMaxPT.value,
                            M4: $scope.masterMaxPT.value,
                            A4: $scope.agentMaxPT.value,
                            N4: $scope.superMinPT.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('superMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = scope.superMaxPT.value;
                        scope.masterMaxPT = commonService.createPositionTakingSelect(min, max, Math.min(max, scope.masterMaxPT.value));
                        scope.superMinPT = commonService.createPositionTakingSelect(min, max, Math.min(max, scope.superMinPT.value));
                    }
                });

                scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = scope.masterMaxPT.value;
                        var value = Math.min(max, scope.agentMaxPT.value);
                        scope.agentMaxPT = commonService.createPositionTakingSelect(min, max, value);
                    }
                });
            }
        };
    };

    companyEditSuperPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('companyEditSuperPositionTaking', companyEditSuperPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$filter', 'resourceService', 'controllerService', 'appSettings'];
    var errType = {
        none: 0,
        limit: 1,
        minBet: 2,
        maxBet: 3,
        maxPerMatch: 4
    };
    var customerSettingBetLimitDirective = function ($rootScope, $filter, resourceService, controllerService, appSettings) {
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
            }
        }

        return {
            restrict: 'AE',
            replace: true,
            scope: {
                betLimit: '=customerSettingBetLimit',
                hasCopy: '='
            },
            template: '<div data-ng-include="templateUrl"></div>',
            controller: ['$scope', function ($scope) {
                $scope.isReadonly = controllerService.formType === appSettings.formType.SuperEditMember
                    || controllerService.formType === appSettings.formType.MasterEditMember;
                $scope.templateUrl = $scope.isReadonly
                    ? '/app/customerSetting/templates/customer-setting-bet-limit-readonly.htm'
                    : '/app/customerSetting/templates/customer-setting-bet-limit.htm';

                var betLimit = parse($scope.betLimit, controllerService.formType);
                $scope.minBet = betLimit.minBet;
                $scope.hasMinBetLimit = $scope.minBet.limit > 0;
                $scope.maxBet = betLimit.maxBet;
                $scope.hasMaxBetLimit = controllerService.formType !== appSettings.formType.CompanyEditSuper;
                $scope.maxPerMatch = betLimit.maxPerMatch;
                $scope.hasMaxPerMatchLimit = $scope.hasMaxBetLimit;
                $scope.appSettings = appSettings;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.BetLimit, $scope.betLimit);
                $scope.resourceService = resourceService;

                if ($scope.isReadonly) {
                    return;
                }

                controllerService.linkBetLimit($scope);

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
                if (scope.isReadonly) {
                    return;
                }

                var validateMinBet = function () {
                    if (scope.minBet.value === scope.maxBet.value === scope.maxPerMatch.value === 0) {
                        scope.minBet.clearErrors();
                        scope.maxBet.clearErrors();
                        scope.minPerMatch.clearErrors();
                    }

                    if (scope.minBet.value < scope.minBet.limit) {
                        var formattedNumber = $filter('number')(scope.minBet.limit);
                        scope.minBet.setInvalid(errType.limit, resourceService.getFormatString('MinBetMoreThanOrEqualTo', formattedNumber));
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
                        var formattedNumber = $filter('number')(scope.maxBet.limit);
                        scope.maxBet.setInvalid(errType.limit, resourceService.getFormatString('MaxBetLessThanOrEqualTo', formattedNumber));
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
                        var errMsg = resourceService.getFormatString('MaxPerMatchLessThanOrEqualTo', $filter('number')(scope.maxPerMatch.limit));
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

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService'];
    var customerSettingDialogDirective = function ($rootScope, commonService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                data: '=customerSettingDialog'
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-dialog.htm',
            link: function (scope) {
                scope.resourceService = resourceService;
                scope.isShow = false;
                function update(isUpdateAllMembers) {
                    var memberPTs = [];
                    angular.forEach(scope.pts, function (pt) {
                        memberPTs.push({
                            SportType: pt.sportType,
                            BetType: pt.betType,
                            IsLive: pt.isLive,
                            Pt: isUpdateAllMembers ? pt.value : 0
                        });
                    });
                    $rootScope.$broadcast('update-member-position-taking', {
                        IsUpdateAllMembers: isUpdateAllMembers,
                        MemberPTs: memberPTs
                    });
                }

                scope.clickClose = function () {
                    scope.isShow = false;
                };
                scope.clickYes = function () {
                    scope.isShow = false;
                    update(true);
                };
                scope.clickNo = function () {
                    scope.isShow = false;
                    update(false);
                };

                scope.$watch('data', function (newValue, oldValue) {
                    if (newValue === oldValue) {
                        return;
                    }

                    if (angular.isUndefined(scope.data)) {
                        scope.isShow = false;
                        return;
                    }

                    var memberPtss = scope.data.MemberPTs;
                    var pts = [];
                    angular.forEach(memberPtss, function (memberPts) {
                        var ptSelect = commonService.createPositionTakingSelect(0, memberPts.Pt, memberPts.Pt);
                        pts.push({
                            sportType: memberPts.SportType,
                            betType: memberPts.BetType,
                            isLive: memberPts.IsLive,
                            name: memberPts.Name,
                            value: ptSelect.value,
                            options: ptSelect.options
                        });
                    });
                    scope.isShow = true;
                    scope.pts = pts;
                });
            }
        };
    };

    customerSettingDialogDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingDialog', customerSettingDialogDirective);
})();

'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'controllerService', 'appSettings'];
    var customerSettingDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        function parse(data, formType) {
            var DiscountSelect = function (min, max, value) {
                this.min = min;
                this.value = value;
                this.options = commonService.generateOptions(min, max, 0.0005, 10000);
            };

            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return new DiscountSelect(0, data.DefaultMaxDiscount, data.SuperDiscount);

                case appSettings.formType.SuperEditMaster:
                    return new DiscountSelect(0, data.SuperDiscount, data.MasterDiscount);

                case appSettings.formType.MasterEditAgent:
                    return new DiscountSelect(0, data.MasterDiscount, data.AgentDiscount);

                case appSettings.formType.AgentEditMember:
                    return new DiscountSelect(0, data.AgentDiscount, data.PlayerDiscount);
            }
        }

        function getName(formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return 'SuperDiscount';

                case appSettings.formType.SuperEditMaster:
                    return 'MasterDiscount';

                case appSettings.formType.MasterEditAgent:
                    return 'AgentDiscount';

                case appSettings.formType.AgentEditMember:
                    return 'PlayerDiscount';
            }
        }

        function getValue(data, formType) {
            var name = getName(formType);
            return data[name];
        }

        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=customerSettingDiscount'
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-discount.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = parse($scope.discount, controllerService.formType);
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.min = discount.min;
                $scope.value = discount.value;
                $scope.options = discount.options;

                $scope.validate = function () {
                    var errCode = $scope.value !== getValue($scope.discount, controllerService.formType)
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    var data = {
                        SportType: $scope.discount.SportType,
                        BetType: $scope.discount.BetType
                    };
                    data[getName(controllerService.formType)] = $scope.value;
                    return {
                        errCode: errCode,
                        data: data
                    };
                };
            } ]
        };
    };

    customerSettingDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingDiscount', customerSettingDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['resourceService'];
    var customerSettingInformationDirective = function (resourceService) {
        return {
            restrict: 'AE',
            scope: {
                information: '=customerSettingInformation'
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-information.htm',
            controller: ['$scope', function ($scope) {
                $scope.resourceService = resourceService;
                $scope.$watch('information', function () {
                    if (angular.isUndefined($scope.information)) {
                        return;
                    }

                    $scope.userName = $scope.information.UserName;
                    $scope.nickName = $scope.information.UserName2;
                    $scope.firstName = $scope.information.FirstName;
                    $scope.lastName = $scope.information.LastName;
                    $scope.phone = $scope.information.Phone;
                    $scope.mobilePhone = $scope.information.MobilePhone;
                    $scope.fax = $scope.information.Fax;
                });
            } ]
        };
    };

    customerSettingInformationDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingInformation', customerSettingInformationDirective);
})();

'use strict';

(function () {
    var injectParams = ['resourceService', 'controllerService', 'appSettings'];
    var customerSettingStatusDirective = function (resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                status: '=customerSettingStatus'
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-status.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkStatus($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Status, $scope.status);
                $scope.enable = !$scope.status.IsDisabled;
                $scope.downline = !$scope.status.IsDisabledDownline;
                $scope.showDownline = controllerService.formType !== appSettings.formType.AgentEditMember;

                $scope.enableDisabled = false;
                $scope.downlineDisabled = true;
                if ($scope.status.IsDisabledUpline === true) {
                    $scope.enableDisabled = true;
                    $scope.downlineDisabled = true;
                }

                if ($scope.enableDisabled === false) {
                    var oldEnable = $scope.enable;
                    $scope.$watch('enable', function (newValue, oldValue) {
                        if (newValue !== oldValue) {
                            $scope.downlineDisabled = !($scope.enable && !oldEnable);
                            if (!$scope.enable) {
                                $scope.downline = false;
                            }
                            else {
                                $scope.downline = !$scope.status.IsDisabled && !$scope.status.IsDisabledDownline;
                            }
                        }
                    });
                }

                $scope.validate = function () {
                    var errCode = $scope.enable === $scope.status.IsDisabled || $scope.downline === $scope.status.IsDisabledDownline
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.status.SportType,
                            BetType: $scope.status.BetType,
                            IsDisabled: !$scope.enable,
                            IsDisabledDownline: !$scope.downline
                        }
                    };
                };
            } ]
        };
    };

    customerSettingStatusDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingStatus', customerSettingStatusDirective);
})();

'use strict';

(function () {
    var injectParams = ['tooltipService'];
    var customerSettingTooltipDirective = function (tooltipService) {
        return {
            restrict: 'AE',
            replace: true,
            templateUrl: '/app/customerSetting/templates/customer-setting-tooltip.htm',
            scope: {},
            link: function (scope, element) {
                tooltipService.setup(scope, element);
                scope.$watch('message', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        tooltipService.calculatePosition();
                    }
                });
            }
        };
    };

    customerSettingTooltipDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingTooltip', customerSettingTooltipDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditAgentPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            var masterPT = 0;
            var agentMaxPTMin = 0;
            var agentMaxPTMax = 0;
            if (!data.IM2) {
                masterPT = data.M2;
                agentMaxPTMin = data.N3 - data.M2;
                agentMaxPTMax = Math.min(data.M3 - data.M2, data.A4);
            } else {
                masterPT = data.UM2;
                agentMaxPTMin = data.N3 - data.UM2;
                agentMaxPTMax = Math.min(data.M3, data.A4);
            }
            return {
                masterPT: commonService.createPositionTakingSelect(0, data.M3, masterPT),
                masterIsAuto: data.IM2,
                agentMaxPT: commonService.createPositionTakingSelect(agentMaxPTMin, agentMaxPTMax, data.A2),
                agentMinPT: commonService.createPositionTakingSelect(0, data.A2, data.N2)
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=masterEditAgentPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/master-edit-agent-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.masterPT = positionTaking.masterPT;
                $scope.masterIsAuto = positionTaking.masterIsAuto;
                $scope.agentMaxPT = positionTaking.agentMaxPT;
                $scope.agentMinPT = positionTaking.agentMinPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.masterPT.value = copyScope.masterPT.value;
                        $scope.masterIsAuto = copyScope.masterIsAuto;
                        $scope.agentMaxPT.value = copyScope.agentMaxPT.value;
                        $scope.agentMinPT.value = copyScope.agentMinPT.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.masterIsAuto ? $scope.positionTaking.IM2 === $scope.masterPT.value : $scope.positionTaking.M2 === $scope.masterPT.value)
                        && $scope.positionTaking.IM2 === $scope.masterIsAuto
                        && $scope.positionTaking.A2 === $scope.agentMaxPT.value
                        && $scope.positionTaking.N2 === $scope.agentMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            SportTypeName: $scope.positionTaking.SportTypeName,
                            BetType: $scope.positionTaking.BetType,
                            BetTypeName: $scope.positionTaking.BetTypeName,
                            IsLive: $scope.positionTaking.IsLive,
                            M2: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM2: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM2: $scope.masterIsAuto,
                            A2: $scope.agentMaxPT.value,
                            N2: $scope.agentMinPT.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                function updateAgentMaxPT(){
                    var min = Math.max(0, scope.positionTaking.N3 - scope.masterPT.value);
                    var max;
                    if (!scope.masterIsAuto) {
                        max = Math.min(scope.positionTaking.M3 - scope.masterPT.value, scope.positionTaking.A4);
                    } else {
                        max = Math.min(scope.positionTaking.M3, scope.positionTaking.A4);
                    }
                    var value = Math.min(scope.agentMaxPT.value, max);
                    scope.agentMaxPT = commonService.createPositionTakingSelect(min, max, value);
                }

                scope.$watch('masterPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        updateAgentMaxPT();
                    }
                });

                scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var masterPT = scope.masterIsAuto ? scope.positionTaking.UM2 : scope.positionTaking.M2;
                        scope.masterPT = commonService.createPositionTakingSelect(0, scope.positionTaking.M3, masterPT);
                        updateAgentMaxPT();
                    }
                });

                scope.$watch('agentMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        scope.agentMinPT = commonService.createPositionTakingSelect(0, scope.agentMaxPT.value, scope.agentMinPT.value);
                    }
                });
            }
        };
    };

    masterEditAgentPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditAgentPositionTaking', masterEditAgentPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditMemberDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=masterEditMemberDiscount'
            },
            templateUrl: '/app/customerSetting/templates/master-edit-member-discount.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    masterEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditMemberDiscount', masterEditMemberDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            var masterPT = 0;
            var masterPTMin = Math.max(0, data.N3 - data.A1);
            var masterPTMax = 0;
            if (!data.IS1) {
                masterPT = data.S1;
                masterPTMax = Math.max(data.M3 - data.A1);
            } else {
                masterPT = data.US1;
                masterPTMax = data.M3;
            }
            return {
                masterPT: commonService.createPositionTakingSelect(masterPTMin, masterPTMax, masterPT),
                masterIsAuto: data.IM1,
                agentPT: data.A1 / appSettings.positionTakingDivider
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=masterEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/master-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.masterPT = positionTaking.masterPT;
                $scope.masterIsAuto = positionTaking.masterIsAuto;
                $scope.agentPT = positionTaking.agentPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.masterPT.value = copyScope.masterPT.value;
                        $scope.masterIsAuto = copyScope.masterIsAuto;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.masterIsAuto ? $scope.positionTaking.UM1 === $scope.masterPT.value : $scope.positionTaking.M1 === $scope.masterPT.value)
                        && $scope.positionTaking.IM1 === $scope.masterIsAuto) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            SportTypeName: $scope.positionTaking.SportTypeName,
                            BetType: $scope.positionTaking.BetType,
                            BetTypeName: $scope.positionTaking.BetTypeName,
                            IsLive: $scope.positionTaking.IsLive,
                            M1: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM1: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM1: $scope.masterIsAuto
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var masterPT = scope.masterIsAuto ? scope.positionTaking.UM1 : scope.positionTaking.M1;
                        var data = scope.positionTaking;
                        var masterPTMin = Math.max(0, data.N3 - data.A1);
                        var masterPTMax = 0;
                        if (!scope.superIsAuto) {
                            masterPTMax = Math.max(data.M3 - data.A1);
                        } else {
                            masterPTMax = data.M3;
                        }
                        scope.superPT = commonService.createPositionTakingSelect(masterPTMin, masterPTMax, masterPT);
                    }
                });
            }
        };
    };

    masterEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditMemberPositionTaking', masterEditMemberPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['$parse', 'appSettings'];
    var positionTakingSelectOptionsDirective = function ($parse, appSettings) {
        var optionTemplate = angular.element(document.createElement('option'));
        return {
            restrict: 'A',
            require: ['select', '?ngModel'],
            link: function (scope, selectElement, attrs, ctrls) {
                //var selectCtrl = ctrls[0];
                var ngModelCtrl = ctrls[1];
                if (!ngModelCtrl) {
                    return;
                }

                if (angular.isDefined(attrs.ngOptions)) {
                    throw "Don't use ng-options in this directive";
                }

                scope.$watchCollection(attrs.positionTakingSelectOptions, function (newValues, oldValues) {
                    if (newValues !== oldValues) {
                        ngModelCtrl.$render();
                    }
                });

                var valueFn = $parse(attrs.positionTakingSelectOptions);
                var currentOptions;
                var currentValue;
                function render() {
                    var value = ngModelCtrl.$modelValue;
                    var options = valueFn(scope);
                    var hasChange = false;
                    if (currentValue !== value) {
                        hasChange = true;
                    } else if (angular.isUndefined(currentOptions) || currentOptions.length !== options.length) {
                        hasChange = true;
                    } else {
                        for (var i = 0; i < currentOptions.length; i++) {
                            if (currentOptions[i] !== options[i]) {
                                hasChange = true;
                                break;
                            }
                        }
                    }

                    if (!hasChange) {
                        //console.log('This check is useful');
                        return;
                    }

                    currentOptions = options;
                    currentValue = value;
                    selectElement.children().remove();
                    angular.forEach(currentOptions, function (option) {
                        var value = option;
                        var label = value / appSettings.positionTakingDivider;
                        var selected = currentValue === option;
                        var optionElement = optionTemplate.clone();
                        optionElement.val(value);
                        optionElement.prop('selected', selected);
                        optionElement.attr('selected', selected);
                        optionElement.text(label);
                        selectElement.append(optionElement);
                    });
                }
                ngModelCtrl.$render = render;
                ngModelCtrl.$parsers.unshift(function (viewValue) {
                    return parseInt(viewValue);
                });
            }
        };
    };

    positionTakingSelectOptionsDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('positionTakingSelectOptions', positionTakingSelectOptionsDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMasterPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            var superPT = 0;
            var masterMaxPTMin = 0;
            var masterMaxPTMax = 0;
            if (!data.IS3) {
                superPT = data.S3;
                masterMaxPTMin = data.N4 - data.S3;
                masterMaxPTMax = Math.min(data.S4 - data.S3, data.M4);
            } else {
                superPT = data.US3;
                masterMaxPTMin = data.N4 - data.US3;
                masterMaxPTMax = Math.min(data.S4, data.M4);
            }
            return {
                superPT: commonService.createPositionTakingSelect(0, data.S4, superPT),
                superIsAuto: data.IS3,
                masterMaxPT: commonService.createPositionTakingSelect(masterMaxPTMin, masterMaxPTMax, data.M3),
                masterMinPT: commonService.createPositionTakingSelect(0, data.M3, data.N3)
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMasterPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/super-edit-master-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.superPT = positionTaking.superPT;
                $scope.superIsAuto = positionTaking.superIsAuto;
                $scope.masterMaxPT = positionTaking.masterMaxPT;
                $scope.masterMinPT = positionTaking.masterMinPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.superPT.value = copyScope.superPT.value;
                        $scope.superIsAuto = copyScope.superIsAuto;
                        $scope.masterMaxPT.value = copyScope.masterMaxPT.value;
                        $scope.masterMinPT.value = copyScope.masterMinPT.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.superIsAuto ? $scope.positionTaking.US3 === $scope.superPT.value : $scope.positionTaking.S3 === $scope.superPT.value)
                        && $scope.positionTaking.IS3 === $scope.superIsAuto
                        && $scope.positionTaking.M3 === $scope.masterMaxPT.value
                        && $scope.positionTaking.N3 === $scope.masterMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            SportTypeName: $scope.positionTaking.SportTypeName,
                            BetType: $scope.positionTaking.BetType,
                            BetTypeName: $scope.positionTaking.BetTypeName,
                            IsLive: $scope.positionTaking.IsLive,
                            S3: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US3: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS3: $scope.superIsAuto,
                            M3: $scope.masterMaxPT.value,
                            N3: $scope.masterMinPT.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                function updateMasterMaxPT(){
                    var min = Math.max(0, scope.positionTaking.N4 - scope.superPT.value);
                    var max;
                    if (!scope.superIsAuto) {
                        max = Math.min(scope.positionTaking.S4 - scope.superPT.value, scope.positionTaking.M4);
                    } else {
                        max = Math.min(scope.positionTaking.S4, scope.positionTaking.M4);
                    }
                    var value = Math.min(scope.masterMaxPT.value, max);
                    scope.masterMaxPT = commonService.createPositionTakingSelect(min, max, value);
                }

                scope.$watch('superPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        updateMasterMaxPT();
                    }
                });

                scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var superPT = scope.superIsAuto ? scope.positionTaking.US3 : scope.positionTaking.S3;
                        scope.superPT = commonService.createPositionTakingSelect(0, scope.positionTaking.S4, superPT);
                        updateMasterMaxPT();
                    }
                });

                scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = scope.masterMaxPT.value;
                        var value = Math.min(max, scope.masterMinPT.value);
                        scope.masterMinPT = commonService.createPositionTakingSelect(min, max, value);
                    }
                });
            }
        };
    };

    superEditMasterPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMasterPositionTaking', superEditMasterPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMemberDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=superEditMemberDiscount'
            },
            templateUrl: '/app/customerSetting/templates/super-edit-member-discount.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.masterDiscount = discount.MasterDiscount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    superEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMemberDiscount', superEditMemberDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            var superPT = 0;
            var superPTMin = Math.max(0, data.N4 - data.M1 - data.A1);
            var superPTMax = 0;
            if (!data.IS1) {
                superPT = data.S1;
                superPTMax = Math.max(data.N4 - data.M1 - data.A1, data.S4 - data.M3);
            } else {
                superPT = data.US1;
                superPTMax = data.S4;
            }
            return {
                superPT: commonService.createPositionTakingSelect(superPTMin, superPTMax, superPT),
                superIsAuto: data.IS1,
                masterPT: data.M1 / appSettings.positionTakingDivider,
                agentPT: data.A1 / appSettings.positionTakingDivider
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/super-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.superPT = positionTaking.superPT;
                $scope.superIsAuto = positionTaking.superIsAuto;
                $scope.masterPT = positionTaking.masterPT;
                $scope.agentPT = positionTaking.agentPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.superPT.value = copyScope.superPT.value;
                        $scope.superIsAuto = copyScope.superIsAuto;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.superIsAuto ? $scope.positionTaking.US1 === $scope.superPT.value : $scope.positionTaking.S1 === $scope.superPT.value)
                        && $scope.positionTaking.IS1 === $scope.superIsAuto) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            SportTypeName: $scope.positionTaking.SportTypeName,
                            BetType: $scope.positionTaking.BetType,
                            BetTypeName: $scope.positionTaking.BetTypeName,
                            IsLive: $scope.positionTaking.IsLive,
                            S1: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US1: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS1: $scope.superIsAuto
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var superPT = scope.superIsAuto ? scope.positionTaking.US1 : scope.positionTaking.S1;
                        var data = scope.positionTaking;
                        var superPTMin = Math.max(0, data.N4 - data.M1 - data.A1);
                        var superPTMax = 0;
                        if (!scope.superIsAuto) {
                            superPTMax = Math.max(data.N4 - data.M1 - data.A1, data.S4 - data.M3);
                        } else {
                            superPTMax = data.S4;
                        }
                        scope.superPT = commonService.createPositionTakingSelect(superPTMin, superPTMax, superPT);
                    }
                });
            }
        };
    };

    superEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMemberPositionTaking', superEditMemberPositionTakingDirective);
})();

'use strict';

(function () {
    angular.module('customerSettingApp').filter('dataGroup', function () {
        function DataGroup(sportType, sportTypeName) {
            this.SportType = sportType;
            this.SportTypeName = sportTypeName;
            this.Items = [];
            this.addItem = function (item) {
                this.Items.push(item);
            };
        }

        return function (items) {
            var dataGroups = [];
            var currentSportType = -1;
            var dataGroup = null;
            angular.forEach(items, function (item) {
                var sportType = item.SportType;
                var sportTypeName = item.SportTypeName;
                if (dataGroup === null || sportType !== currentSportType) {
                    currentSportType = sportType;
                    dataGroup = new DataGroup(sportType, sportTypeName);
                    dataGroups.push(dataGroup);
                }
                dataGroup.addItem(item);
            });

            return dataGroups;
        };
    });
})();

'use strict';

(function () {
    var injectParams = ['$window'];
    var commonService = function ($window) {
        this.scrollToTop = function () {
            $window.scrollTo(0, 0);
        };

        this.inArray = function (value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        };

        this.generateOptions = function (min, max, step, factor) {
            var options = [];
            min = Math.max(0, min);
            max = Math.max(0, max);
            while (max >= min) {
                options.push(max);
                max -= step;
                max = Math.round(max * factor) / factor;
            }
            return options;
        };

        var self = this;
        var PositionTakingSelect = function (min, max, value) {
            var options = self.generateOptions(min, max, 1, 1);
            var selectIndex = -1;
            angular.forEach(options, function (option, index) {
                if (selectIndex === -1 && option === value) {
                    selectIndex = index;
                }
            });

            if (selectIndex === -1) {
                if (value < min) {
                    options.push(value);
                    selectIndex = options.length - 1;
                } else if (value > max) {
                    options.unshift(value);
                    selectIndex = 0;
                }
            }

            this.options = options;
            this.value = this.options[selectIndex];
        };

        this.createPositionTakingSelect = function (min, max, value) {
            return new PositionTakingSelect(min, max, value);
        };
    };

    commonService.$inject = injectParams;
    angular.module('customerSettingApp').service('commonService', commonService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'appSettings', 'modelService', 'dataService', 'resourceService', 'messageService'];
    var controllerService = function ($rootScope, appSettings, modelService, dataService, resourceService, messageService) {
        var self = this;
        var statusScopes = [];
        var discountScopes = [];
        var betLimitScopes = [];
        var positionTakingScopes = [];
        this.linkStatus = function (scope) {
            statusScopes.push(scope);
        };
        this.linkDiscount = function (scope) {
            discountScopes.push(scope);
        };
        this.linkBetLimit = function (scope) {
            betLimitScopes.push(scope);
        };
        this.linkPositionTaking = function (scope) {
            positionTakingScopes.push(scope);
        };

        this.getDisplayName = function (settingType, data) {
            var liveName = data.IsLive === true ? ' (' + resourceService.getString('Live') + ')' : '';
            var settingLevel;
            switch (settingType) {
                case appSettings.settingType.PositionTaking:
                    settingLevel = self.positionTakingLevel;
                    break;

                case appSettings.settingType.BetLimit:
                    settingLevel = self.betLimitLevel;
                    break;

                case appSettings.settingType.Discount:
                    settingLevel = self.discountLevel;
                    break;

                case appSettings.settingType.Status:
                    settingLevel = self.statusLevel;
                    break;
            }

            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return self.productName;
                case appSettings.settingLevel.SportType:
                    return data.SportTypeName;
                case appSettings.settingLevel.BetType:
                    return data.BetTypeName + liveName;
                case appSettings.settingLevel.SportTypeBetType:
                    return data.BetTypeName + liveName;
            }

            return undefined;
        };

        this.select = function (callback) {
            var selectModel = modelService.createSelectModel();
            dataService.select(selectModel, function (response) {
                var requestContext = response.RequestContext;
                self.requestContext = requestContext;
                self.productId = requestContext.ProductId;
                self.productName = requestContext.ProductName;
                self.formType = requestContext.FormType;
                self.language = requestContext.Language;

                var customerSetting = response.CustomerSetting;
                self.customerSetting = customerSetting;
                self.information = customerSetting.Information;
                self.statuses = customerSetting.Statuses;
                self.discounts = customerSetting.Discounts;
                self.betLimits = customerSetting.BetLimits;
                self.positionTakings = customerSetting.PositionTakings;

                var settingContext = customerSetting.SettingContext;
                self.settingContext = settingContext;
                self.statusLevel = settingContext.StatusLevel;
                self.discountLevel = settingContext.DiscountLevel;
                self.betLimitLevel = settingContext.BetLimitLevel;
                self.positionTakingLevel = settingContext.PositionTakingLevel;
                self.disableAutoMPT = settingContext.DisableAutoMPT;

                callback();
            });
        };

        var selectSettingType;
        this.update = function (settingType) {
            selectSettingType = settingType;
            update();
        };

        $rootScope.$on('update-member-position-taking', function (event, updateMemberPT) {
            update(updateMemberPT);
        });

        function update(updateMemberPT) {
            function getData(scopes) {
                var data = [];
                var isFormInvalid = false;
                angular.forEach(scopes, function (scope) {
                    if (!isFormInvalid) {
                        var result = scope.validate();
                        if (result.errCode === appSettings.errorType.Success) {
                            data.push(result.data);
                        } else if (result.errCode === appSettings.errorType.FormInvalid) {
                            isFormInvalid = true;
                        }
                    }
                });

                return isFormInvalid
                    ? undefined
                    : data;
            }

            function hasUpdate(data, isShowNoChange) {
                if (angular.isUndefined(data)) {
                    return false; // form invalid
                }

                if (data.length === 0) {
                    if (isShowNoChange) {
                        messageService.showError(resourceService.getString('NoChangeToUpdate'));
                    }
                    return false;
                }

                return true;
            }

            function hasUncheckAutoPT(data) {
                var isAutoPTName, ptName;
                switch (self.formType) {
                    case appSettings.formType.SuperEditMaster:
                        isAutoPTName = 'IS3';
                        ptName = 'S3';
                        break;

                    case appSettings.formType.MasterEditAgent:
                        isAutoPTName = 'IM2';
                        ptName = 'M2';
                        break;
                }

                if (angular.isDefined(isAutoPTName) && angular.isDefined(ptName)) {
                    var memberPTs = [];
                    angular.forEach(data, function (pt) {
                        var isAuto = pt[isAutoPTName];
                        if (isAuto === false) {
                            var ptValue = pt[ptName];
                            var name = self.getDisplayName(appSettings.settingType.PositionTaking, pt);
                            memberPTs.push({
                                SportType: pt.SportType,
                                BetType: pt.BetType,
                                IsLive: pt.IsLive,
                                Name: name,
                                Pt: ptValue
                            });
                        }
                    });

                    if (memberPTs.length > 0) {
                        $rootScope.updateModal = {
                            MemberPTs: memberPTs
                        };
                        return true;
                    }
                }

                return false;
            }

            messageService.hide();
            var positionTakings;
            var betLimits;
            var discounts;
            var statuses;
            switch (selectSettingType) {
                case appSettings.settingType.All:
                    statuses = getData(statusScopes);
                    discounts = getData(discountScopes);
                    betLimits = getData(betLimitScopes);
                    positionTakings = getData(positionTakingScopes);
                    var hasUpdatePT = hasUpdate(positionTakings, true);
                    if (!hasUpdate(statuses, false) && !hasUpdate(discounts, false) && !hasUpdate(betLimits, false) && !hasUpdatePT) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUpdatePT && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }

                    break;

                case appSettings.settingType.PositionTaking:
                    positionTakings = getData(positionTakingScopes);
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }
                    break;

                case appSettings.settingType.BetLimit:
                    betLimits = getData(betLimitScopes);
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Discount:
                    discounts = getData(discountScopes);
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Status:
                    statuses = getData(statusScopes);
                    if (!hasUpdate(statuses, true)) {
                        return;
                    }
                    break;
            }

            if (angular.isDefined(updateMemberPT)) {
                var isUpdateAllMembers = updateMemberPT.IsUpdateAllMembers;
                var memberPTs = updateMemberPT.MemberPTs;
                var newPositionTakings = [];
                angular.forEach(positionTakings, function (positionTaking) {
                    var isFound = false;
                    var newPositionTaking = positionTaking;
                    angular.forEach(memberPTs, function (memberPt) {
                        if (!isFound
                            && newPositionTaking.SportType === memberPt.SportType
                            && newPositionTaking.BetType === memberPt.BetType
                            && newPositionTaking.IsLive === memberPt.IsLive) {
                            newPositionTaking = angular.extend({
                                IsUpdateAllMembers: isUpdateAllMembers,
                                MemberPositionTaking: memberPt.Pt
                            }, newPositionTaking);
                            newPositionTakings.push(newPositionTaking);
                            isFound = true;
                        }
                    });
                });

                positionTakings = newPositionTakings;
            }

            // delete un-used properties
            angular.forEach(positionTakings, function (positionTaking) {
                delete (positionTaking.SportTypeName);
                delete (positionTaking.BetTypeName);
            });

            var selectModel = modelService.createSelectModel();
            var updateModel = angular.extend({
                UserName: self.information.UserName,
                SettingType: selectSettingType,
                StatusLevel: self.statusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: self.discountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: self.betLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: self.positionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, selectModel);
            dataService.update(updateModel);
        }
    };

    controllerService.$inject = injectParams;
    angular.module('customerSettingApp').service('controllerService', controllerService);
})();

'use strict';

(function () {
    var injectParams = ['$http', '$rootScope', 'messageService', 'appSettings'];
    var dataService = function ($http, $rootScope, messageService, appSettings) {
        function isSuccessResponse(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        this.select = function (selectModel, successCallback) {
            var request = $http.post('/CustomerSetting/Select', selectModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                if (angular.isFunction(successCallback)) {
                    successCallback(response);
                }
            });

            return request;
        };

        this.update = function (updateModel, successCallback) {
            var request = $http.post('/CustomerSetting/Update', updateModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                var results = [];
                switch (updateModel.SettingType) {
                    case appSettings.settingType.All:
                        results.push(response.StatusResult);
                        results.push(response.DiscountResult);
                        results.push(response.BetLimitResult);
                        results.push(response.PositionTakingResult);
                        break;

                    case appSettings.settingType.Status:
                        results.push(response.StatusResult);
                        break;

                    case appSettings.settingType.Discount:
                        results.push(response.DiscountResult);
                        break;

                    case appSettings.settingType.BetLimit:
                        results.push(response.BetLimitResult);
                        break;

                    case appSettings.settingType.PositionTaking:
                        results.push(response.PositionTakingResult);
                        break;
                }

                messageService.showResults(results);

                if (angular.isFunction(successCallback)) {
                    successCallback(response);
                }
            });

            return request;
        };
    };

    dataService.$inject = injectParams;
    angular.module('customerSettingApp').service('dataService', dataService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope'];
    var loadingService = function ($rootScope) {
        this.block = function () {
            $rootScope.hasLoading = true;
        };

        this.unblock = function () {
            $rootScope.hasLoading = false;
        };
    };

    loadingService.$inject = injectParams;
    angular.module('customerSettingApp').service('loadingService', loadingService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$timeout', 'commonService', '$sce', 'appSettings'];
    var messageService = function ($rootScope, $timeout, commonService, $sce, appSettings) {
        var messageClasses = {};
        messageClasses[appSettings.messageType.Success] = 'success';
        messageClasses[appSettings.messageType.Warning] = 'warning';
        messageClasses[appSettings.messageType.Error] = 'error';

        var Message = function (messageType, messageContent) {
            this.MessageType = messageType;
            this.MessageContent = messageContent;
        };

        function generateMesage(result) {
            var messageType = appSettings.messageType.Error;
            switch (result.ErrCode) {
                case appSettings.errCode.Success:
                    messageType = appSettings.messageType.Success;
                    break;

                case appSettings.errCode.NoChange:
                    messageType = appSettings.messageType.Warning;
                    break;
            }

            var messageContent = result.ErrMsg || 'Undefined messsage';
            return new Message(messageType, messageContent);
        }

        function showMessage(message) {
            var htmlMessages = [];
            htmlMessages.push({
                messageClass: messageClasses[message.MessageType],
                messageContent: $sce.trustAsHtml(message.MessageContent)
            });
            commonService.scrollToTop();

            $timeout(function () {
                $rootScope.messages = htmlMessages;
            });
        }

        function showMessages(messages) {
            var htmlMessages = [];
            angular.forEach(messages, function (message) {
                htmlMessages.push({
                    messageClass: messageClasses[message.MessageType],
                    messageContent: $sce.trustAsHtml(message.MessageContent)
                });
            });
            commonService.scrollToTop();

            $timeout(function () {
                $rootScope.messages = htmlMessages;
            });
        }

        this.showMessage = function (messageType, messageContent) {
            var message = new Message(messageType, messageContent);
            showMessage(message);
        };

        this.showResult = function (result) {
            var message = generateMesage(result);
            showMessage(message);
        };

        this.showResults = function (results) {
            var messages = [];
            angular.forEach(results, function (result) {
                var message = generateMesage(result);
                messages.push(message);
            });

            showMessages(messages);
        };

        this.showSuccess = function (messageContent) {
            this.showMessage(appSettings.messageType.Success, messageContent || 'Undefined success message');
        };

        this.showWarning = function (messageContent) {
            this.showMessage(appSettings.messageType.Warning, messageContent || 'Undefined warning message');
        };

        this.showError = function (messageContent) {
            this.showMessage(appSettings.messageType.Error, messageContent || 'Undefined error message');
        };

        this.hide = function () {
            $rootScope.messages = undefined;
        };
    };

    messageService.$inject = injectParams;
    angular.module('customerSettingApp').service('messageService', messageService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$location', 'appSettings'];
    var modelService = function ($rootScope, $location, appSettings) {
        var formSettings = [
            {
                formType: appSettings.formType.CompanyEditSuper,
                pattern: '/editSuper/:superId/:productId'
            },
            {
                formType: appSettings.formType.SuperEditMaster,
                pattern: '/editMaster/:masterId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditAgent,
                pattern: '/editAgent/:agentId/:productId'
            },
            {
                formType: appSettings.formType.AgentEditMember,
                pattern: '/editMember/:memberId/:productId'
            },
            {
                formType: appSettings.formType.SuperEditMember,
                pattern: '/superEditMember/:superId/:memberId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditMember,
                pattern: '/masterEditMember/:masterId/:memberId/:productId'
            }
        ];

        var self = this;
        function loadParams() {
            var isValidModel = false;
            var modelParams = {};
            var path = $location.path();
            angular.forEach(formSettings, function (formSetting) {
                if (isValidModel) {
                    return;
                }

                var formType = formSetting.formType;
                var pattern = formSetting.pattern;
                var matches = pattern.match(/\/:\w+/g);
                var params = [];
                angular.forEach(matches, function (match) {
                    params.push(match.replace('/:', ''));
                });

                var regExp = new RegExp('^' + pattern.replace(/\/:\w+/g, '/(\\d+)') + '$');
                matches = regExp.exec(path);
                if (matches !== null && matches.length - 1 === params.length) {
                    isValidModel = true;
                    angular.forEach(params, function (param, index) {
                        modelParams[param] = parseInt(matches[index + 1]);
                    });
                    modelParams.formType = formType;
                }
            });

            self.isValid = isValidModel;
            self.params = modelParams;
        }

        this.createSelectModel = function () {
            var isValid = this.isValid;
            var params = this.params;
            if (!isValid) {
                return undefined;
            }

            var custId;
            var roleId;
            var targetCustId;
            var targetRoleId;
            switch (params.formType) {
                case appSettings.formType.CompanyEditSuper:
                    targetCustId = params.superId;
                    targetRoleId = appSettings.roleType.Super;
                    break;

                case appSettings.formType.SuperEditMaster:
                    targetCustId = params.masterId;
                    targetRoleId = appSettings.roleType.Master;
                    break;

                case appSettings.formType.MasterEditAgent:
                    targetCustId = params.agentId;
                    targetRoleId = appSettings.roleType.Agent;
                    break;

                case appSettings.formType.SuperEditMember:
                    custId = params.superId;
                    roleId = appSettings.roleType.Super;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.MasterEditMember:
                    custId = params.masterId;
                    roleId = appSettings.roleType.Master;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.AgentEditMember:
                    custId = params.agentId;
                    roleId = appSettings.roleType.Agent;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;
            }

            return {
                CustId: custId,
                RoleId: roleId,
                TargetCustId: targetCustId,
                TargetRoleId: targetRoleId,
                FormType: params.formType,
                ProductId: params.productId
            };
        };

        $rootScope.$on('$locationChangeStart', function () {
            loadParams();
        });
        
        loadParams();
    };

    modelService.$inject = injectParams;
    angular.module('customerSettingApp').service('modelService', modelService);
})();

'use strict';

(function () {
    var injectParams = ['resources', 'appSettings'];
    var resourceService = function (resources, appSettings) {
        var resource = {};

        this.setLanguage = function (language) {
            if (angular.isUndefined(language)) {
                language = appSettings.defaultLanguage.toLowerCase();
            } else {
                language = language.toLowerCase();
            }

            var selectResource = resources[language];
            if (angular.isUndefined(selectResource)) {
                selectResource = resources[appSettings.defaultLanguage.toLowerCase()];
            }
            resource = selectResource;
        };

        this.getString = function (key) {
            var value = resource[key];
            if (angular.isDefined(value)) {
                value = value.replace(/&apos;|&#x0*27;/g, "'");
            } else {
                value = key;
            }
            return value;
        };

        this.getFormatString = function (key) {
            var input = this.getString(key);
            var args = Array.prototype.slice.call(arguments, 1);
            return input.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined' ? args[number] : match;
            });
        };
    };

    resourceService.$inject = injectParams;
    angular.module('customerSettingApp').service('resourceService', resourceService);
})();

'use strict';

(function () {
    var injectParams = ['appSettings', 'controllerService'];
    var templateService = function (appSettings, controllerService) {
        var levelFolders = {};
        levelFolders[appSettings.settingLevel.Product] = 'no-group';
        levelFolders[appSettings.settingLevel.SportType] = 'no-group';
        levelFolders[appSettings.settingLevel.BetType] = 'no-group';
        levelFolders[appSettings.settingLevel.SportTypeBetType] = 'sport-group';

        var positionTakingFiles = {};
        positionTakingFiles[appSettings.formType.CompanyEditSuper] = 'company-edit-super-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMaster] = 'super-edit-master-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditAgent] = 'master-edit-agent-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMember] = 'super-edit-member-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditMember] = 'master-edit-member-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.AgentEditMember] = 'agent-edit-member-position-taking-container.htm';

        var discountFiles = {};
        discountFiles[appSettings.formType.CompanyEditSuper] = 'discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMaster] = 'discount-container.htm';
        discountFiles[appSettings.formType.MasterEditAgent] = 'discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMember] = 'super-edit-member-discount-container.htm';
        discountFiles[appSettings.formType.MasterEditMember] = 'master-edit-member-discount-container.htm';
        discountFiles[appSettings.formType.AgentEditMember] = 'discount-container.htm';

        this.resolveView = function () {
            return '/app/customerSetting/templates/customer-setting-view.htm';
        };

        this.resolveStatus = function () {
            var levelFolder = levelFolders[controllerService.statusLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/status-container.htm'
            : undefined;
        };

        this.resolveDiscount = function () {
            var levelFolder = levelFolders[controllerService.discountLevel];
            var fileName = discountFiles[controllerService.formType];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/' + fileName
            : undefined;
        };

        this.resolveBetLimit = function () {
            var levelFolder = levelFolders[controllerService.betLimitLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/betlimit-container.htm'
            : undefined;
        };

        this.resolvePositionTaking = function () {
            var levelFolder = levelFolders[controllerService.positionTakingLevel];
            var fileName = positionTakingFiles[controllerService.formType];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/' + fileName
            : undefined;
        };
    };

    templateService.$inject = injectParams;
    angular.module('customerSettingApp').service('templateService', templateService);
})();

'use strict';

(function () {
    var injectParams = ['$timeout'];
    var tooltipService = function ($timeout) {
        var tooltipScope;
        var tooltipElement;
        var boundingElement;

        this.calculatePosition = function () {
            var rect = boundingElement.getBoundingClientRect();
            var tooltipRect = tooltipElement[0].getBoundingClientRect();
            var scrollX = window.pageXOffset || document.body.scrollLeft;
            var scrollY = window.pageYOffset || document.body.scrollTop;
            var left = Math.max(0, rect.left + ((rect.right - rect.left) - (tooltipRect.right - tooltipRect.left)) / 2 + scrollX);
            var top = Math.max(0, rect.top - (tooltipRect.bottom - tooltipRect.top) + scrollY);
            tooltipElement.css({
                'top': top + 'px',
                'left': left + 'px'
            });
        };

        this.setup = function (scope, element) {
            if (angular.isDefined(tooltipScope)) {
                throw 'Only one tooltip component in document';
            }

            tooltipScope = scope;
            tooltipElement = element;
            tooltipElement.addClass('ng-hide');
        };

        this.show = function (element, message) {
            if (angular.isUndefined(tooltipScope)) {
                throw 'setup is required';
            }

            boundingElement = element[0];
            if (tooltipScope.message !== message) {
                $timeout(function () {
                    tooltipScope.message = message;
                });
            }

            if (tooltipElement.hasClass('ng-hide')) {
                tooltipElement.removeClass('ng-hide');
                this.calculatePosition();
            }
        };

        this.hide = function () {
            if (angular.isUndefined(tooltipScope)) {
                throw 'setup is required';
            }

            if (!tooltipElement.hasClass('ng-hide')) {
                tooltipElement.addClass('ng-hide');
            }
        };
    };

    tooltipService.$inject = injectParams;
    angular.module('customerSettingApp').service('tooltipService', tooltipService);
})();

'use strict';
/*jshint -W101,-W106,-W044,-W098*/

(function () {
    var resources = {};
    
    resources['en-us'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "Agent Max PT",
        AgentMinPT: "Agent Min PT",
        AgentPT: "Agent PT",
        Agents: "Agents",
        BetSetting: "Bet Setting",
        Commission: "Commission",
        CommissionForAgent: "Comm for Agent",
        CommissionForMaster: "Comm for Master",
        CommissionForMember: "Comm for Member",
        CopyTooltip: "Click to duplicate the values",
        Enable: "Enable",
        EnableDownline: "Enable Downline",
        Fax: "Fax",
        FirstName: "First Name",
        GeneralInformation: "General Information",
        IsAuto: "Is Auto",
        LastName: "Last Name",
        Live: "Live",
        MasterMaxPT: "Master Max PT",
        MasterMinPT: "Master Min PT",
        MasterPT: "Master PT",
        Masters: "Masters",
        Max: "Max",
        MaxBet: "Max Bet",
        MaxBetLessThanOrEqualMaxPerMatch: "Max Bet must be less than or equal to Max Per Match",
        MaxBetLessThanOrEqualTo: "Max Bet must be less than or equal to {0}",
        MaxBetMoreThanOrEqualMinBet: "Max Bet must be greater than or equal to Min Bet",
        MaxLive: "Max Live",
        MaxPerMatch: "Max Per Match",
        MaxPerMatchLessThanOrEqualTo: "Max Per Match must be less than or equal to {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "Max Per Match must be greater than or equal to Max Bet",
        Members: "Members",
        Min: "Min",
        MinBet: "Min Bet",
        MinBetLessThanOrEqualMaxBet: "Min Bet must be less than or equal to Max Bet",
        MinBetMoreThanOrEqualTo: "Min Bet must be greater than or equal to {0}",
        MinLive: "Min Live",
        MobilePhone: "Mobile Phone",
        Nickname: "Nickname",
        NoChangeToUpdate: "There is no change to update!",
        Phone: "Phone",
        PositionTaking: "Position Taking",
        ProductStatus: "Product Status",
        SuperMaxPT: "Super Max PT",
        SuperMinPT: "Super Min PT",
        SuperPT: "Super PT",
        Update: "Update",
        UpdateBetSettingSuccessfully: "Updated bet setting successfully.",
        UpdateCommissionSuccessfully: "Updated commission setting successfully.",
        UpdateMemberPtInfo: "You have changed your position taking",
        UpdateMemberPtQuestion: "Do you want to update all member&apos;s position taking to:",
        UpdateMemberPtTitle: "Confirm Update Members&apos;s PT",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "UserName"
    };
    
    resources['zh-cn'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "代理最高占成",
        BetSetting: "投注设定",
        Commission: "佣金",
        CopyTooltip: "Click to duplicate the values",
        Enable: "启用",
        EnableDownline: "Enable Downline",
        Fax: "传真",
        FirstName: "名",
        GeneralInformation: "基本信息",
        LastName: "姓",
        Live: "现场",
        MasterMaxPT: "总代理最高占成",
        MaxBet: "最高投注",
        MaxBetLessThanOrEqualMaxPerMatch: "最大投注额必须小于或等于最大单场比赛投注额",
        MaxBetLessThanOrEqualTo: "最大投注额必须小于或等于 {0}",
        MaxBetMoreThanOrEqualMinBet: "最大投注额必须大于或等于最小投注额",
        MaxPerMatch: "单场限额",
        MaxPerMatchLessThanOrEqualTo: "最大单场比赛投注额必须小于或等于 {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "最大单场比赛投注额必须大于或等于最大投注额",
        MinBet: "最低投注",
        MinBetLessThanOrEqualMaxBet: "最小投注额必须小于或等于最大投注额",
        MinBetMoreThanOrEqualTo: "最大投注额必须大于或等于 {0}",
        MobilePhone: "手机",
        Nickname: "昵称",
        NoChangeToUpdate: "没有更新！",
        Phone: "电话",
        PositionTaking: "占成数",
        ProductStatus: "Product Status",
        SuperMaxPT: "超级总代理最高占成",
        SuperMinPT: "超级总代理最低占成",
        Update: "更改",
        UserName: "用户名"
    };
    
    resources['zh-tw'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "代理最高佔成",
        BetSetting: "投注設定",
        Commission: "佣金",
        CopyTooltip: "Click to duplicate the values",
        Enable: "啟用",
        EnableDownline: "Enable Downline",
        Fax: "傳真",
        FirstName: "名",
        GeneralInformation: "基本信息",
        LastName: "姓",
        Live: "現場",
        MasterMaxPT: "總代理最高佔成",
        MaxBet: "最高投注",
        MaxBetLessThanOrEqualMaxPerMatch: "最大投注額必須小於或等於最大單場比賽投注額",
        MaxBetLessThanOrEqualTo: "最大投注額必須小於或等於 {0}",
        MaxBetMoreThanOrEqualMinBet: "最大投注額必須大於或等於最小投注額",
        MaxPerMatch: "單場限額",
        MaxPerMatchLessThanOrEqualTo: "最大單場比賽投注額必須小於或等於 {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "最大單場比賽投注額必須大於或等於最大投注額",
        MinBet: "最低投注",
        MinBetLessThanOrEqualMaxBet: "最小投注額必須小於或等於最大投注額",
        MinBetMoreThanOrEqualTo: "最大投注額必須大於或等於 {0}",
        MobilePhone: "手機",
        Nickname: "暱稱",
        NoChangeToUpdate: "沒有更新!",
        Phone: "電話",
        PositionTaking: "佔成數",
        ProductStatus: "Product Status",
        SuperMaxPT: "超級總代理最高佔成",
        SuperMinPT: "超級總代理最低佔成",
        Update: "更改",
        UserName: "用戶名"
    };

    angular.module('customerSettingApp').value('resources', resources);
})();


'use strict';

(function () {
    angular.module('customerSettingApp').value('appSettings', {
        version: '0.1',
        defaultLanguage: 'en-US',
        defaultMaxBet: 99999999999,
        betLimitMaxLength: 14,
        positionTakingDivider: 100,
        formType: {
            CompanyEditSuper: 1,
            SuperEditMaster: 2,
            MasterEditAgent: 3,
            AgentEditMember: 4,
            SuperEditMember: 5,
            MasterEditMember: 6
        },
        roleType: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4,
            Company: 5
        },
        settingType: {
            All: 0,
            PositionTaking: 1,
            BetLimit: 2,
            Discount: 3,
            Status: 4
        },
        settingLevel: {
            Product: 1,
            SportType: 2,
            BetType: 3,
            SportTypeBetType: 4
        },
        errorType: {
            Success: 0,
            FormInvalid: 1,
            NoChange: 2
        },
        errCode: {
            Success: 0,
            ErrorValue: -1,
            NoChange: -2,
            AccessDenied: -3
        },
        messageType: {
            Success: 0,
            Warning: 1,
            Error: 2
        }
    });
})();