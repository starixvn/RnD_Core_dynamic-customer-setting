(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtMasterEditAgentItemDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtMasterEditAgentItem',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-master-edit-agent-item.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

                // Master Is Auto
                $scope.masterIsAuto = positionTaking.IM2;
                $scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterPT = generateMasterPT();
                        $scope.agentMaxPT = generateAgentMaxPT();
                    }
                });

                // Master PT
                function generateMasterPT() {
                    var min = !$scope.masterIsAuto ? Math.max(0, positionTaking.N3 - positionTaking.A4) : 0;
                    var max = positionTaking.M3;
                    var value = !isInitialized
                        ? !$scope.masterIsAuto ? positionTaking.M2 : positionTaking.UM2
                        : ptService.correctValue(min, max, $scope.masterPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterPT = generateMasterPT();
                $scope.$watch('masterPT.value', function (newValue, oldValue) {
                    if ($scope.agentMaxPT !== null && $scope.agentMaxPT.valid === false) {
                        return;
                    }

                    if (newValue !== oldValue) {
                        $scope.agentMaxPT = generateAgentMaxPT();
                    }
                });

                // Agent Max PT
                function generateAgentMaxPT() {
                    var min = Math.max(0, positionTaking.N3 - $scope.masterPT.value);
                    var max = !$scope.masterIsAuto
                        ? Math.min(positionTaking.M3 - $scope.masterPT.value, positionTaking.A4)
                        : Math.min(positionTaking.M3, positionTaking.A4);
                    var value = !isInitialized
                        ? positionTaking.A2
                        : ptService.correctValue(min, max, $scope.agentMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.agentMaxPT = generateAgentMaxPT();
                $scope.$watch('agentMaxPT.value', function (newValue, oldValue) {
                    if ($scope.agentMinPT !== null && $scope.agentMinPT.valid === false) {
                        return;
                    }

                    if (newValue !== oldValue) {
                        $scope.agentMinPT = generateAgentMinPT();
                    }
                });

                // Agent Min PT
                function generateAgentMinPT() {
                    var min = 0;
                    var max = $scope.agentMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N2
                        : ptService.correctValue(min, max, $scope.agentMinPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.agentMinPT = generateAgentMinPT();

                // Copy
                $scope.copy = function (source) {
                    $scope.masterIsAuto = source.masterIsAuto;
                    return ptService.copyValue(source, $scope, 'masterPT', generateMasterPT)
                        && ptService.copyValue(source, $scope, 'agentMaxPT', generateAgentMaxPT)
                        && ptService.copyValue(source, $scope, 'agentMinPT', generateAgentMinPT);
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.masterIsAuto ? positionTaking.M2 : positionTaking.UM2) === $scope.masterPT.value
                        && positionTaking.IM2 === $scope.masterIsAuto
                        && positionTaking.A2 === $scope.agentMaxPT.value
                        && positionTaking.N2 === $scope.agentMinPT.value) {
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
                            M2: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM2: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM2: $scope.masterIsAuto,
                            A2: $scope.agentMaxPT.value,
                            N2: $scope.agentMinPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    fanexPtMasterEditAgentItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtMasterEditAgentItem', fanexPtMasterEditAgentItemDirective);
})();