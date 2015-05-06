'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtAgentEditMemberDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtAgentEditMember',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-agent-edit-member.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;

                // Agent PT
                function generateAgentPT() {
                    var min = positionTaking.N2;
                    var max = positionTaking.A2;
                    var value = !isInitialized
                        ? positionTaking.A1
                        : ptService.correctValue(min, max, $scope.agentPT.value);
                    return ptService.createSelect(min, max, value);
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
                    return ptService.copyValue(source, $scope, 'agentPT', generateAgentPT);
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