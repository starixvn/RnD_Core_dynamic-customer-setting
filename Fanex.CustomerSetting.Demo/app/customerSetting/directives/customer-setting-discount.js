'use strict';

(function () {
    var injectParams = ['componentService', 'resourceService', 'appSettings'];
    var customerSettingDiscountDirective = function (componentService, resourceService, appSettings) {
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
                var discount = componentService.parseDiscount($scope.discount, $scope.formType);
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