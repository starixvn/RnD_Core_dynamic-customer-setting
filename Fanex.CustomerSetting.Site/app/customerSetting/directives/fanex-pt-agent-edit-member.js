'use strict';

(function () {
    var injectParams = ['fanexCommonService', 'fanexResourceService', 'fanexControllerService', 'fanexAppSettings'];
    var fanexPtAgentEditMemberDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtAgentEditMember',
                hasCopy: '='
            },
            templateUrl: 'templates/fanex-pt-agent-edit-member.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Agent PT
                function generateAgentPT() {
                    var min = positionTaking.N2;
                    var max = positionTaking.A2;
                    var value = !isInitialized
                        ? positionTaking.A1
                        : commonService.correctValue(min, max, $scope.agentPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.agentPT = generateAgentPT();
                $scope.$watch('agentPT.value', function (newValue, oldValue) {
                    if ($scope.agentPT !== null && $scope.agentPT.valid === false) {
                        return;
                    }

                    if (newValue !== oldValue) {
                        $scope.agentPT = generateAgentPT();
                    }
                });

                // Copy
                $scope.copy = function (source) {
                    return commonService.copyValue(source, $scope, 'agentPT', generateAgentPT);
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (positionTaking.A1 === $scope.agentPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            BetType: positionTaking.BetType,
                            IsLive: positionTaking.IsLive,
                            A1: $scope.agentPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    fanexPtAgentEditMemberDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtAgentEditMember', fanexPtAgentEditMemberDirective);
})();