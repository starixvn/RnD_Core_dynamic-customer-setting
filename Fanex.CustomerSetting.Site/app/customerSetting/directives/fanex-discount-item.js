'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDiscountService', 'fanexResourceService'];
    var fanexDiscountDirective = function (appSettings, discountService, resourceService) {
        function parse(data, formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return discountService.createSelect(0, data.DefaultMaxDiscount, data.SuperDiscount);

                case appSettings.formType.SuperEditMaster:
                    return discountService.createSelect(0, data.SuperDiscount, data.MasterDiscount);

                case appSettings.formType.MasterEditAgent:
                    return discountService.createSelect(0, data.MasterDiscount, data.AgentDiscount);

                case appSettings.formType.AgentEditMember:
                    return discountService.createSelect(0, data.AgentDiscount, data.PlayerDiscount);

                default:
                    throw 'Not implemented yet.';
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
                formType: '='
            },
            templateUrl: 'templates/fanex-discount-item.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                var formType = $scope.formType;
                var discount = $scope.discount;
                var settingLevel = $scope.settingLevel;
                $scope.name = resourceService.getDisplayName(settingLevel, discount);
                var data = parse(discount, formType);
                $scope.min = data.min;
                $scope.value = data.value;
                $scope.options = data.options;

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