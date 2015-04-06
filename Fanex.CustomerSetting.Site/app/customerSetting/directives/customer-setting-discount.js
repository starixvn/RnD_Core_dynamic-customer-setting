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
                var discount = $scope.discount;
                var requestContext = controllerService.getRequestContext();
                var data = parse(discount, requestContext.FormType);
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, discount);
                $scope.min = data.min;
                $scope.value = data.value;
                $scope.options = data.options;

                $scope.validate = function () {
                    var errCode = $scope.value !== getValue(discount, requestContext.FormType)
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    var data = {
                        SportType: discount.SportType,
                        BetType: discount.BetType
                    };
                    data[getName(requestContext.FormType)] = $scope.value;
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