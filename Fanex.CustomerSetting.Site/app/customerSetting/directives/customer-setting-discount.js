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

        function getName(formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return 'SuperDiscount';

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