(function () {
    'use strict';
    var injectParams = ['$timeout', 'fanexAppSettings', 'fanexStatusService', 'fanexResourceService'];
    var fanexStatusItemDirective = function ($timeout, appSettings, statusService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                status: '=fanexStatusItem',
                settingLevel: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: 'templates/fanex-status-item.htm',
            controller: ['$scope', function ($scope) {
                statusService.linkScope($scope);

                $scope.settingType = appSettings.settingType.Status;
                var formType = $scope.formType;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.status);
                $scope.enable = !$scope.status.IsDisabled;
                $scope.downline = !$scope.status.IsDisabled && !$scope.status.IsDisabledDownline;
                $scope.showDownline = formType !== appSettings.formType.AgentEditMember;
                $scope.enableDisabled = false;
                $scope.downlineDisabled = true;
                if ($scope.status.IsDisabledUpline === true) {
                    $scope.enableDisabled = true;
                    $scope.downlineDisabled = true;
                }

                var oldEnable = $scope.enable;
                if ($scope.enableDisabled === false) {
                    $scope.$watch('enable', function (newValue, oldValue) {
                        if (newValue !== oldValue) {
                            $scope.downlineDisabled = !($scope.enable && !oldEnable);
                            if (!$scope.enable) {
                                $scope.downline = false;
                            } else {
                                $scope.downline = !$scope.status.IsDisabled && !$scope.status.IsDisabledDownline;
                            }
                        }
                    });
                }

                $scope.copy = function (copyScope) {
                    if ($scope.enableDisabled === true) {
                        return;
                    }

                    if ($scope.enable !== copyScope.enable) {
                        $scope.enable = copyScope.enable;
                        $timeout(function () {
                            if (!oldEnable) {
                                $scope.downline = copyScope.downline;
                            }
                        });
                    } else {
                        $scope.downlineDisabled = !($scope.enable && !oldEnable);
                        if (!oldEnable) {
                            $scope.downline = copyScope.downline;
                        }
                    }
                };

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