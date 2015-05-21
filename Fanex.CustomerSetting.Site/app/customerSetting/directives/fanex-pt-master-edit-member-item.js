(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtMasterEditMemberItemDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtMasterEditMemberItem',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-master-edit-member-item.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

                // Master Is Auto
                $scope.masterIsAuto = positionTaking.IM1;
                $scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterPT = generateMasterPT();
                    }
                });

                // Master PT
                function generateMasterPT() {
                    var min = Math.max(0, positionTaking.N3 - positionTaking.A1);
                    var max = !$scope.masterIsAuto
                        ? positionTaking.M3 - positionTaking.A1
                        : positionTaking.M3;
                    var disabled = positionTaking.IM2 && !$scope.masterIsAuto;
                    var value = disabled
                        ? positionTaking.M1
                        : !isInitialized
                            ? !$scope.masterIsAuto ? positionTaking.M1 : positionTaking.UM1
                            : ptService.correctValue(min, max, $scope.masterPT.value);
                    var select = ptService.createSelect(min, max, value);
                    select.disabled = disabled;
                    return select;
                }
                $scope.masterPT = generateMasterPT();

                // Agent PT
                $scope.agentPT = positionTaking.A1;

                // Copy
                $scope.copy = function (source) {
                    $scope.masterIsAuto = source.masterIsAuto;
                    if (!$scope.masterPT.disabled || source.masterIsAuto) {
                        return ptService.copyValue(source, $scope, 'masterPT', generateMasterPT);
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

    fanexPtMasterEditMemberItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtMasterEditMemberItem', fanexPtMasterEditMemberItemDirective);
})();