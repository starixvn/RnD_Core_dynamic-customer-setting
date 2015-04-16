'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var agentEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=agentEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/agent-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                var isCopied = false;
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
                    if (newValue !== oldValue) {
                        $scope.agentPT = generateAgentPT();
                        isCopied = false;
                    }
                });

                // Copy
                $scope.copy = function (source) {
                    var copy = function (field, generator) {
                        var value = source[field].value;
                        $scope[field].value = value;
                        $scope[field] = generator();
                        return $scope[field].value === value;
                    };

                    return copy('agentPT', generateAgentPT);
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

    agentEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('agentEditMemberPositionTaking', agentEditMemberPositionTakingDirective);
})();