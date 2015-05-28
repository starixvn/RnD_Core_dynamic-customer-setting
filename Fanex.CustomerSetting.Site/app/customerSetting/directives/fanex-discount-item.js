(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexDiscountService', 'fanexResourceService'];
    var fanexDiscountDirective = function (appSettings, discountService, resourceService) {
        function parse(data, formType) {
            var min = 0;
            var max;
            var value;
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    value = data.SuperDiscount;
                    max = data.DefaultMaxDiscount;
                    break;

                case appSettings.formType.SuperEditMaster:
                    value = data.MasterDiscount;
                    max = data.SuperDiscount;
                    break;

                case appSettings.formType.MasterEditAgent:
                    value = data.AgentDiscount;
                    max = data.MasterDiscount;
                    break;

                case appSettings.formType.AgentEditMember:
                    value = data.PlayerDiscount;
                    max = data.AgentDiscount;
                    break;

                default:
                    throw 'Not implemented yet.';
            }

            return {
                min: min,
                max: max,
                value: value
            };
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

                default:
                    throw 'Not implemented yet.';
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
                discount: '=fanexDiscountItem',
                settingLevel: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: 'templates/fanex-discount-item.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                $scope.settingType = appSettings.settingType.Discount;
                var formType = $scope.formType;
                var discount = $scope.discount;
                var settingLevel = $scope.settingLevel;
                $scope.name = resourceService.getDisplayName(settingLevel, discount);
                var data = parse(discount, formType);
                var select = discountService.createSelect(data.min, data.max, data.value);
                $scope.value = select.value;
                $scope.options = select.options;

                $scope.copy = function (copyScope) {
                    var data = parse(discount, formType);
                    if (copyScope.value >= data.min && copyScope.value <= data.max) {
                        $scope.value = copyScope.value;
                    }
                };

                $scope.validate = function () {
                    var errCode = $scope.value !== getValue(discount, formType)
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    var data = {
                        SportType: discount.SportType,
                        BetType: discount.BetType
                    };
                    data[getName(formType)] = $scope.value;
                    return {
                        errCode: errCode,
                        data: data
                    };
                };
            } ]
        };
    };

    fanexDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountItem', fanexDiscountDirective);
})();