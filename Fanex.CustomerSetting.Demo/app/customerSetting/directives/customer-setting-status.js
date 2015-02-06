'use strict';

(function () {
    var injectParams = ['resourceService', 'appSettings'];
    var customerSettingStatusDirective = function (resourceService, appSettings) {
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
                $scope.resourceService = resourceService;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.status);
                $scope.enable = !$scope.status.IsDisabled;
                $scope.downline = !$scope.status.DisableDownlines;

                $scope.validate = function () {
                    var errCode = $scope.enable === $scope.status.IsDisabled || $scope.downline === $scope.status.DisableDownlines
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;
                    
                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.status.SportType,
                            BetType: $scope.status.BetType,
                            IsDisabled: !$scope.enable,
                            DisableDownlines: !$scope.downline
                        }
                    };
                };
            } ]
        };
    };

    customerSettingStatusDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingStatus', customerSettingStatusDirective);
})();