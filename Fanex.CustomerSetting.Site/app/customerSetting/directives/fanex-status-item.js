(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexStatusService', 'fanexResourceService'];
    var fanexStatusItemDirective = function (appSettings, statusService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                status: '=fanexStatusItem',
                settingLevel: '=',
                formType: '='
            },
            templateUrl: 'templates/fanex-status-item.htm',
            controller: ['$scope', function ($scope) {
                statusService.linkScope($scope);

                var formType = $scope.formType;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.status);
                $scope.enable = !$scope.status.IsDisabled;
                $scope.downline = !$scope.status.IsDisabledDownline;
                $scope.showDownline = formType !== appSettings.formType.AgentEditMember;
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

    fanexStatusItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexStatusItem', fanexStatusItemDirective);
})();