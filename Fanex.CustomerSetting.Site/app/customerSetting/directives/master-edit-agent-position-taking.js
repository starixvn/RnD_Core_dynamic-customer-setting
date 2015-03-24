'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'linkingService', 'appSettings'];
    var masterEditAgentPositionTakingDirective = function ($rootScope, commonService, resourceService, linkingService, appSettings) {
        function parse(data) {
            var masterPT = 0;
            var agentMaxPTMin = 0;
            var agentMaxPTMax = 0;
            if (!data.IM2) {
                masterPT = data.M2;
                agentMaxPTMin = data.N3 - data.M2;
                agentMaxPTMax = Math.min(data.M3 - data.M2, data.A4);
            } else {
                masterPT = data.UM2;
                agentMaxPTMin = data.N3 - data.UM2;
                agentMaxPTMax = Math.min(data.M3, data.A4);
            }
            return {
                masterPT: commonService.createPositionTakingSelect(0, data.M3, masterPT),
                masterIsAuto: data.IM2,
                agentMaxPT: commonService.createPositionTakingSelect(agentMaxPTMin, agentMaxPTMax, data.A2),
                agentMinPT: commonService.createPositionTakingSelect(0, data.A2, data.N2)
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=masterEditAgentPositionTaking',
                settingLevel: '=',
                settingContext: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/master-edit-agent-position-taking.htm',
            controller: ['$scope', function ($scope) {
                linkingService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, $scope.formType);
                $scope.masterPT = positionTaking.masterPT;
                $scope.masterIsAuto = positionTaking.masterIsAuto;
                $scope.agentMaxPT = positionTaking.agentMaxPT;
                $scope.agentMinPT = positionTaking.agentMinPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.masterPT.value = copyScope.masterPT.value;
                        $scope.masterIsAuto = copyScope.masterIsAuto;
                        $scope.agentMaxPT.value = copyScope.agentMaxPT.value;
                        $scope.agentMinPT.value = copyScope.agentMinPT.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.masterIsAuto ? $scope.positionTaking.IM2 === $scope.masterPT.value : $scope.positionTaking.M2 === $scope.masterPT.value)
                        && $scope.positionTaking.IM2 === $scope.masterIsAuto
                        && $scope.positionTaking.A2 === $scope.agentMaxPT.value
                        && $scope.positionTaking.N2 === $scope.agentMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            BetType: $scope.positionTaking.BetType,
                            IsLive: $scope.positionTaking.IsLive,
                            M2: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM2: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM2: $scope.masterIsAuto,
                            A2: $scope.agentMaxPT.value,
                            N2: $scope.agentMinPT.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                function updateAgentMaxPT(){
                    if (!scope.masterIsAuto) {
                        scope.agentMaxPT = commonService.createPositionTakingSelect(
                            scope.positionTaking.N3 - scope.masterPT.value,
                            Math.min(scope.positionTaking.S4 - scope.superPT.value, scope.positionTaking.A4),
                            scope.agentMaxPT.value);
                    } else {
                        scope.agentMaxPT = commonService.createPositionTakingSelect(
                            scope.positionTaking.N3 - scope.masterPT.value,
                            Math.min(scope.positionTaking.M3, scope.positionTaking.A4),
                            scope.agentMaxPT.value);
                    }
                }

                scope.$watch('masterPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        updateAgentMaxPT();
                    }
                });

                scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var masterPT = scope.masterIsAuto ? scope.positionTaking.UM2 : scope.positionTaking.M2;
                        scope.masterPT = commonService.createPositionTakingSelect(0, scope.positionTaking.M3, masterPT);
                        updateAgentMaxPT();
                    }
                });

                scope.$watch('agentMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        scope.agentMinPT = commonService.createPositionTakingSelect(0, scope.agentMaxPT.value, scope.agentMinPT.value);
                    }
                });
            }
        };
    };

    masterEditAgentPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditAgentPositionTaking', masterEditAgentPositionTakingDirective);
})();