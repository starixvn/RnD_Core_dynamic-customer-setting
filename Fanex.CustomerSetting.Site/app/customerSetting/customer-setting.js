// customer-setting - v0.0.1 - 2015-03-17 16:03:00

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
    var injectParams = ['$scope', '$rootScope', 'locationService', 'dataService', 'resourceService',
                        'templateService', 'linkingService', 'appSettings', 'messageService'];
    var customerSettingController = function ($scope, $rootScope, locationService, dataService, resourceService,
                        templateService, linkingService, appSettings, messageService) {
        var selectModel = locationService.createSelectModel();
        dataService.select(selectModel, function (response) {
            var settingContext = response.SettingContext;
            resourceService.setLanguage(settingContext.Language);
            var customerSetting = response.CustomerSetting;
            $scope.formType = selectModel.FormType;
            $scope.settingContext = settingContext;
            $scope.setting = customerSetting.Setting;
            $scope.information = customerSetting.Information;

            var productId = $scope.settingContext.ProductId;
            var formType = $scope.formType;
            $scope.statuses = customerSetting.Statuses;
            $scope.statusTemplate = templateService.resolveStatus(productId, formType, $scope.setting.StatusLevel);
            $scope.discounts = customerSetting.Discounts;
            $scope.discountTemplate = templateService.resolveDiscount(productId, formType, $scope.setting.DiscountLevel);
            $scope.betLimits = customerSetting.BetLimits;
            $scope.betLimitTemplate = templateService.resolveBetLimit(productId, formType, $scope.setting.BetLimitLevel);
            $scope.positionTakings = customerSetting.PositionTakings;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking(productId, formType, $scope.setting.PositionTakingLevel);
        });

        $scope.updateStatuses = function () {
            update(appSettings.settingType.Status);
        };

        $scope.updateDiscounts = function () {
            update(appSettings.settingType.Discount);
        };

        $scope.updateBetLimits = function () {
            update(appSettings.settingType.BetLimit);
        };

        $scope.updatePositionTakings = function () {
            update(appSettings.settingType.PositionTaking);
        };

        $scope.updateAll = function () {
            update(appSettings.settingType.All);
        };

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

        function update(settingType) {
            messageService.hide();
            var positionTakings;
            var betLimits;
            var discounts;
            var statuses;
            switch (settingType) {
                case appSettings.settingType.All:
                    statuses = linkingService.statuses();
                    discounts = linkingService.discounts();
                    betLimits = linkingService.betLimits();
                    positionTakings = linkingService.positionTakings();
                    if (!hasUpdate(statuses, false) && !hasUpdate(discounts, false) && !hasUpdate(betLimits, false) && !hasUpdate(positionTakings, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.PositionTaking:
                    positionTakings = linkingService.positionTakings();
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.BetLimit:
                    betLimits = linkingService.betLimits();
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Discount:
                    discounts = linkingService.discounts();
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Status:
                    statuses = linkingService.statuses();
                    if (!hasUpdate(statuses, true)) {
                        return;
                    }
                    break;
            }

            var updateModel = angular.extend({
                UserName: $scope.information.UserName,
                SettingType: settingType,
                StatusLevel: $scope.setting.StatusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: $scope.setting.DiscountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: $scope.setting.BetLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: $scope.setting.PositionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, selectModel);
            dataService.update(updateModel);
        }
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());

'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', '$timeout', 'locationService', 'templateService', 'messageService'];
    var customerSettingViewController = function ($scope, $rootScope, $timeout, locationService, templateService, messageService) {
        $rootScope.$on('$locationChangeStart', function () {
            $scope.view = undefined;
            messageService.hide();
        });

        $rootScope.$on('$locationChangeSuccess', function () {
            if (!locationService.isValid) {
                messageService.showError('Please check your URL');
            } else {
                $timeout(function () {
                    $scope.view = templateService.resolveView(locationService.productId);
                });
            }
        });
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());

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
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'linkingService', 'appSettings'];
    var companyEditSuperPositionTakingDirective = function ($rootScope, commonService, resourceService, linkingService, appSettings) {
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
                settingLevel: '=',
                settingContext: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/company-edit-super-position-taking.htm',
            controller: ['$scope', function ($scope) {
                linkingService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                var name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.positionTaking);
                if ($scope.positionTaking.IsLive) {
                    name += ' (' + resourceService.getString('Live') + ')';
                }
                $scope.name = name;
                var positionTaking = parse($scope.positionTaking, $scope.formType);
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
                        var max = parseInt(scope.superMaxPT.value);
                        scope.masterMaxPT = commonService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.masterMaxPT.value)));
                        scope.superMinPT = commonService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.superMinPT.value)));
                    }
                });

                scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = parseInt(scope.masterMaxPT.value);
                        scope.agentMaxPT = commonService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.agentMaxPT.value)));
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
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, data.MaxBetMax),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, data.MaxPerMatchMax)
                    };

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

'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'linkingService', 'appSettings'];
    var customerSettingDiscountDirective = function (commonService, resourceService, linkingService, appSettings) {
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
                    return {};

                case appSettings.formType.SuperEditMember:
                    return {};

                case appSettings.formType.MasterEditMember:
                    return {};

                case appSettings.formType.AgentEditMember:
                    return {};
            }
        }

        function getName(formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return 'SuperDiscount';

                case appSettings.formType.SuperEditMaster:
                    return 'MasterDiscount';

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

        function getValue(data, formType) {
            var name = getName(formType);
            return data[name];
        }

        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=customerSettingDiscount',
                settingLevel: '=',
                settingContext: '=',
                formType: '='
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-discount.htm',
            controller: ['$scope', function ($scope) {
                linkingService.linkDiscount($scope);
                var discount = parse($scope.discount, $scope.formType);
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.discount);
                $scope.min = discount.min;
                $scope.value = discount.value;
                $scope.options = discount.options;

                $scope.validate = function () {
                    var errCode = $scope.value !== getValue($scope.discount, $scope.formType)
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    var data = {
                        SportType: $scope.discount.SportType,
                        BetType: $scope.discount.BetType
                    };
                    data[getName($scope.formType)] = $scope.value;
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
    var injectParams = ['resourceService', 'linkingService', 'appSettings'];
    var customerSettingStatusDirective = function (resourceService, linkingService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                status: '=customerSettingStatus',
                settingLevel: '=',
                settingContext: '='
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-status.htm',
            controller: ['$scope', function ($scope) {
                linkingService.linkStatus($scope);
                $scope.resourceService = resourceService;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.status);
                $scope.enable = !$scope.status.IsDisabled;
                $scope.downline = !$scope.status.IsDisabledDownline;

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
    var injectParams = ['$parse'];
    var positionTakingSelectOptionsDirective = function ($parse) {
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
                function render() {
                    selectElement.children().remove();
                    var modelValue = ngModelCtrl.$modelValue;
                    var options = valueFn(scope);
                    angular.forEach(options, function (option) {
                        var value = option;
                        var label = value / 100;
                        var selected = modelValue === option;
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
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'linkingService', 'appSettings'];
    var superEditMasterPositionTakingDirective = function ($rootScope, commonService, resourceService, linkingService, appSettings) {
        function parse(data) {
            var superPT = 0;
            var masterMaxPTMin = 0;
            var masterMaxPTMax = 0;
            if (data.IS3) {
                superPT = data.US3;
                masterMaxPTMin = data.N4 - data.US3;
                masterMaxPTMax = Math.min(data.S4, data.M4);
            } else {
                superPT = data.S3;
                masterMaxPTMin = data.N4 - data.N4;
                masterMaxPTMax = Math.min(data.S4 - data.S4, data.M4);
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
                settingLevel: '=',
                settingContext: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/super-edit-master-position-taking.htm',
            controller: ['$scope', function ($scope) {
                linkingService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                var name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.positionTaking);
                if ($scope.positionTaking.IsLive) {
                    name += ' (' + resourceService.getString('Live') + ')';
                }
                $scope.name = name;
                var positionTaking = parse($scope.positionTaking, $scope.formType);
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
                            BetType: $scope.positionTaking.BetType,
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
                scope.$watch('superPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        if (scope.superIsAuto) {
                            scope.masterMaxPT = commonService.createPositionTakingSelect(
                                scope.positionTaking.N4 - scope.positionTaking.US3,
                                Math.min(scope.positionTaking.S4, scope.positionTaking.M4),
                                scope.positionTaking.M3);
                        } else {
                            scope.masterMaxPT = commonService.createPositionTakingSelect(
                                scope.positionTaking.N4 - scope.positionTaking.S3,
                                Math.min(scope.positionTaking.S4 - scope.positionTaking.S3, scope.positionTaking.M4),
                                scope.positionTaking.M3);
                        }
                    }
                });

                scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        if (scope.superIsAuto) {
                            scope.superPT = commonService.createPositionTakingSelect(0, scope.positionTaking.S4, scope.positionTaking.US3);
                            scope.masterMaxPT = commonService.createPositionTakingSelect(
                                scope.positionTaking.N4 - scope.positionTaking.US3,
                                Math.min(scope.positionTaking.S4, scope.positionTaking.M4),
                                scope.positionTaking.M3);
                        } else {
                            scope.superPT = commonService.createPositionTakingSelect(0, scope.positionTaking.S4, scope.positionTaking.S3);
                            scope.masterMaxPT = commonService.createPositionTakingSelect(
                                scope.positionTaking.N4 - scope.positionTaking.S3,
                                Math.min(scope.positionTaking.S4 - scope.positionTaking.S3, scope.positionTaking.M4),
                                scope.positionTaking.M3);
                        }
                    }
                });

                scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        scope.masterMinPT = commonService.createPositionTakingSelect(0, scope.masterMaxPT.value, scope.masterMinPT.value);
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
                    options = options.length - 1;
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
    var injectParams = ['$http', '$rootScope', 'messageService'];
    var dataService = function ($http, $rootScope, messageService) {
        function isSuccess(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        this.select = function (selectModel, successCallback) {
            var request = $http.post('/CustomerSetting/Select', selectModel);
            request.success(function (response) {
                if (isSuccess(response) && angular.isFunction(successCallback)) {
                    successCallback(response);
                }
            });

            return request;
        };

        this.update = function (updateModel, successCallback) {
            var request = $http.post('/CustomerSetting/Update', updateModel);
            request.success(function (response) {
                var message = response.ErrMsg;
                if (response.Results) {
                    var messsages = [];
                    angular.forEach(response.Results, function (result) {
                        messsages.push(result.ErrMsg);
                    });
                    message = messsages.join('<br />');
                }
                if (isSuccess(response)) {
                    messageService.showSuccess(message);
                } else {
                    messageService.showError(message);
                }

                if (isSuccess(response) && angular.isFunction(successCallback)) {
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
    var injectParams = ['appSettings'];
    var linkingService = function (appSettings) {
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

        var statusScopes = [];
        this.linkStatus = function (scope) {
            statusScopes.push(scope);
        };
        this.statuses = function () {
            return getData(statusScopes);
        };

        var discountScopes = [];
        this.linkDiscount = function (scope) {
            discountScopes.push(scope);
        };
        this.discounts = function () {
            return getData(discountScopes);
        };

        var betLimitScopes = [];
        this.linkBetLimit = function (scope) {
            betLimitScopes.push(scope);
        };
        this.betLimits = function () {
            return getData(betLimitScopes);
        };

        var positionTakingScopes = [];
        this.linkPositionTaking = function (scope) {
            positionTakingScopes.push(scope);
        };
        this.positionTakings = function () {
            return getData(positionTakingScopes);
        };
    };

    linkingService.$inject = injectParams;
    angular.module('customerSettingApp').service('linkingService', linkingService);
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
    var injectParams = ['$location', '$rootScope', 'appSettings'];
    var locationService = function ($location, $rootScope, appSettings) {
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
                formType: appSettings.formType.SuperEditMember,
                pattern: '/superEditMember/:superId/:memberId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditMember,
                pattern: '/masterEditMember/:masterId/:memberId/:productId'
            },
            {
                formType: appSettings.formType.AgentEditMember,
                pattern: '/agentEditMember/:masterId/:memberId/:productId'
            }
        ];

        var self = this;
        function loadParams() {
            self.isValid = false;
            var path = $location.path();
            angular.forEach(formSettings, function (formSetting) {
                if (self.isValid) {
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
                    self.isValid = true;
                    angular.forEach(params, function (param, index) {
                        self[param] = parseInt(matches[index + 1]);
                    });
                    self.formType = formType;
                }
            });
        }

        this.createSelectModel = function () {
            if (!this.isValid) {
                return undefined;
            }

            var custId;
            var roleId;
            var targetCustId;
            var targetRoleId;
            switch (this.formType) {
                case appSettings.formType.CompanyEditSuper:
                    targetCustId = this.superId;
                    targetRoleId = appSettings.roleType.Super;
                    break;

                case appSettings.formType.SuperEditMaster:
                    targetCustId = this.masterId;
                    targetRoleId = appSettings.roleType.Master;
                    break;

                case appSettings.formType.MasterEditAgent:
                    targetCustId = this.agentId;
                    targetRoleId = appSettings.roleType.Agent;
                    break;

                case appSettings.formType.SuperEditMember:
                    custId = this.superId;
                    roleId = appSettings.roleType.Super;
                    targetCustId = this.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.MasterEditMember:
                    custId = this.masterId;
                    roleId = appSettings.roleType.Master;
                    targetCustId = this.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.AgentEditMember:
                    custId = this.agentId;
                    roleId = appSettings.roleType.Agent;
                    targetCustId = this.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;
            }

            var model = {
                CustId: custId,
                RoleId: roleId,
                TargetCustId: targetCustId,
                TargetRoleId: targetRoleId,
                FormType: this.formType,
                ProductId: this.productId
            };
            return model;
        };

        $rootScope.$on('$locationChangeStart', function () {
            loadParams();
        });
    };

    locationService.$inject = injectParams;
    angular.module('customerSettingApp').service('locationService', locationService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$timeout', 'commonService', '$sce', 'appSettings'];
    var messageService = function ($rootScope, $timeout, commonService, $sce, appSettings) {
        var messageClasses = {};
        messageClasses[appSettings.messageType.Success] = 'success';
        messageClasses[appSettings.messageType.Warning] = 'warning';
        messageClasses[appSettings.messageType.Error] = 'error';

        var show = function (message, messageType) {
            $timeout(function () {
                $rootScope.hasMessage = true;
                $rootScope.messageClass = messageClasses[messageType];
                $rootScope.message = $sce.trustAsHtml(message);
            });
            commonService.scrollToTop();
        };

        this.showSuccess = function (message) {
            show(message || 'Undefined success', appSettings.messageType.Success);
        };

        this.showWarning = function (message) {
            show(message || 'Undefined warning', appSettings.messageType.Warning);
        };

        this.showError = function (message) {
            show(message || 'Undefined error', appSettings.messageType.Error);
        };

        this.hide = function () {
            $rootScope.hasMessage = false;
            $rootScope.message = undefined;
        };
    };

    messageService.$inject = injectParams;
    angular.module('customerSettingApp').service('messageService', messageService);
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
            return angular.isDefined(value) ? value : key;
        };

        this.getFormatString = function (key) {
            var input = this.getString(key);
            var args = Array.prototype.slice.call(arguments, 1);
            return input.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined' ? args[number] : match;
            });
        };

        this.getDisplayName = function (settingLevel, settingContext, data) {
            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return settingContext.ProductName;
                case appSettings.settingLevel.SportType:
                    return data.SportTypeName;
                case appSettings.settingLevel.BetType:
                    return data.BetTypeName;
                case appSettings.settingLevel.SportTypeBetType:
                    return data.BetTypeName;
            }

            return undefined;
        };
    };

    resourceService.$inject = injectParams;
    angular.module('customerSettingApp').service('resourceService', resourceService);
})();

'use strict';

(function () {
    var injectParams = ['appSettings'];
    var templateService = function (appSettings) {
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

        this.resolveView = function (productId) {
            return productId > 0
                ? '/app/customerSetting/templates/customer-setting-view.htm'
                : undefined;
        };

        this.resolveStatus = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/status-container.htm'
            : undefined;
        };

        this.resolveDiscount = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/discount-container.htm'
            : undefined;
        };

        this.resolveBetLimit = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            return angular.isDefined(levelFolder)
            ? '/app/customerSetting/templates/' + levelFolder + '/betlimit-container.htm'
            : undefined;
        };

        this.resolvePositionTaking = function (productId, formType, settingLevel) {
            var levelFolder = levelFolders[settingLevel];
            var fileName = positionTakingFiles[formType];
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
        Agents: "Agents",
        BetSetting: "Bet Setting",
        Commission: "Commission",
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
        positionTakingStep: 0.01,
        positionTakingDivider: 100,
        formType: {
            CompanyEditSuper: 1,
            SuperEditMaster: 2,
            MasterEditAgent: 3,
            SuperEditMember: 4,
            MasterEditMember: 5,
            AgentEditMember: 6
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
        messageType: {
            Success: 0,
            Warning: 1,
            Error: 2
        }
    });
})();