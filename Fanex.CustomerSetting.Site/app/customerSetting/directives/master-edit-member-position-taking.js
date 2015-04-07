'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=masterEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/master-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                var isCopied = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Master Is Auto
                $scope.masterIsAuto = positionTaking.IM1;
                $scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.isDisabledMasterPT = positionTaking.IM2 && !$scope.masterIsAuto;
                        $scope.masterPT = generateMasterPT();
                        isCopied = false;
                    }
                });

                // Master PT
                $scope.isDisabledMasterPT = positionTaking.IM2 && !$scope.masterIsAuto;
                function generateMasterPT() {
                    var min = Math.max(0, positionTaking.N3 - positionTaking.A1);
                    var max = !$scope.masterIsAuto
                        ? positionTaking.M3 - positionTaking.A1
                        : positionTaking.M3;
                    var value = $scope.isDisabledMasterPT
                        ? positionTaking.M1
                        : isCopied === true
                            ? $scope.masterPT.value
                            : !$scope.masterIsAuto ? positionTaking.M1 : positionTaking.UM1;
                    if (isInitialized && !$scope.isDisabledMasterPT) {
                        value = commonService.correctValue(min, max, value);
                    }
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterPT = generateMasterPT();

                // Agent PT
                $scope.agentPT = positionTaking.A1 / appSettings.positionTakingDivider;

                // Copy
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }
                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        isCopied = true;
                        $scope.masterIsAuto = copyScope.masterIsAuto;
                        if (!$scope.isDisabledMasterPT || copyScope.masterIsAuto) {
                            $scope.masterPT.value = copyScope.masterPT.value;
                        }
                    }
                });

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.masterIsAuto ? positionTaking.M1 : positionTaking.UM1) === $scope.masterPT.value
                        && positionTaking.IM1 === $scope.masterIsAuto) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            SportTypeName: positionTaking.SportTypeName,
                            BetType: positionTaking.BetType,
                            BetTypeName: positionTaking.BetTypeName,
                            IsLive: positionTaking.IsLive,
                            M1: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM1: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM1: $scope.masterIsAuto
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    masterEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditMemberPositionTaking', masterEditMemberPositionTakingDirective);
})();