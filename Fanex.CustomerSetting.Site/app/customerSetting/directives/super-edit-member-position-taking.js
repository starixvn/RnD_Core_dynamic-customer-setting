'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/super-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Super Is Auto
                $scope.superIsAuto = positionTaking.IS1;
                $scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.superPT = generateSuperPT();
                    }
                });

                // Super PT
                function generateSuperPT() {
                    var min = Math.max(0, positionTaking.N4 - positionTaking.M1 - positionTaking.A1);
                    var max = !$scope.superIsAuto
                        ? Math.max(positionTaking.N4 - positionTaking.M1 - positionTaking.A1, positionTaking.S4 - positionTaking.M3)
                        : positionTaking.S4;
                    var disabled = positionTaking.IS3 && !$scope.superIsAuto;
                    var value = disabled
                        ? positionTaking.S1
                        : !isInitialized
                            ? !$scope.superIsAuto ? positionTaking.S1 : positionTaking.US1
                            : commonService.correctValue(min, max, $scope.superPT.value);
                    var select = commonService.createPositionTakingSelect(min, max, value);
                    select.disabled = disabled;
                    return select;
                }
                $scope.superPT = generateSuperPT();

                // Master PT
                $scope.masterPT = positionTaking.M1 / appSettings.positionTakingDivider;

                // Agent PT
                $scope.agentPT = positionTaking.A1 / appSettings.positionTakingDivider;

                // Copy
                $scope.copy = function (source) {
                    $scope.superIsAuto = source.superIsAuto;
                    if (!$scope.superPT.disabled || source.superIsAuto) {
                        return commonService.copyValue(source, $scope, 'superPT', generateSuperPT);
                    }

                    return true;
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.superIsAuto ? positionTaking.S1 : positionTaking.US1) === $scope.superPT.value
                        && positionTaking.IS1 === $scope.superIsAuto) {
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
                            S1: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US1: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS1: $scope.superIsAuto
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    superEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMemberPositionTaking', superEditMemberPositionTakingDirective);
})();