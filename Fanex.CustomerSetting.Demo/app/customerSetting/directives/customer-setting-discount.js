'use strict';

(function () {
    var injectParams = ['componentService', 'resourceService', 'linkingService', 'appSettings'];
    var customerSettingDiscountDirective = function (componentService, resourceService, linkingService, appSettings) {
        function parse(data, formType) {
            var DiscountSelect = function (min, max, value) {
                this.min = min;
                this.value = value;
                this.options = componentService.generateOptions(min, max, 0.0005, 10000);
            };

            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return new DiscountSelect(0, data.DefaultMaxDiscount, data.SuperDiscount);

                case 2:
                    return {};

                case 3:
                    return {};

                case 4:
                    return {};

                case 5:
                    return {};

                case 6:
                    return {};
            }
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
                    var errCode = $scope.value !== $scope.discount.value
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.discount.SportType,
                            BetType: $scope.discount.BetType,
                            Value: $scope.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        scope.discount.SuperDiscount = newValue;
                    }
                });
            }
        };
    };

    customerSettingDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingDiscount', customerSettingDiscountDirective);
})();