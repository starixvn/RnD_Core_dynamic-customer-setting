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
            templateUrl: 'templates/master-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Master Is Auto
                $scope.masterIsAuto = positionTaking.IM1;
                $scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.isDisabledMasterPT = positionTaking.IM2 && !$scope.masterIsAuto;
                        $scope.masterPT = generateMasterPT();
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
                        : !isInitialized
                            ? !$scope.masterIsAuto ? positionTaking.M1 : positionTaking.UM1
                            : commonService.correctValue(min, max, $scope.masterPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterPT = generateMasterPT();

                // Agent PT
                $scope.agentPT = positionTaking.A1 / appSettings.positionTakingDivider;

                // Copy
                $scope.copy = function (source) {
                    var copy = function (field, generator) {
                        var value = source[field].value;
                        $scope[field].value = value;
                        $scope[field] = generator();
                        return $scope[field].value === value;
                    };

                    $scope.masterIsAuto = source.masterIsAuto;
                    if (!$scope.isDisabledMasterPT || source.masterIsAuto) {
                        return copy('masterPT', generateMasterPT);
                    }

                    return true;
                };

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