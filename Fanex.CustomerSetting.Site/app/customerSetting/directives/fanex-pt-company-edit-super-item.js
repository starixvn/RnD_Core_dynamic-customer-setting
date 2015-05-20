'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtCompanyEditSuperItemDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtCompanyEditSuperItem',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-company-edit-super-item.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

                // Super Max PT
                function generateSuperMaxPT() {
                    var min = 0;
                    var max = positionTaking.DefaultMaxPT;
                    var value = !isInitialized
                        ? positionTaking.S4
                        : ptService.correctValue(min, max, $scope.superMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.superMaxPT = generateSuperMaxPT();
                $scope.$watch('superMaxPT.value', function (newValue, oldValue) {
                    if ($scope.superMaxPT !== null && $scope.superMaxPT.valid === false) {
                        return;
                    }

                    if (newValue !== oldValue) {
                        $scope.masterMaxPT = generateMasterMaxPT();
                        $scope.superMinPT = generateSuperMinPT();
                    }
                });

                // Master Max PT
                function generateMasterMaxPT() {
                    var min = 0;
                    var max = $scope.superMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.M4
                        : ptService.correctValue(min, max, $scope.masterMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if ($scope.agentMaxPT !== null && $scope.agentMaxPT.valid === false) {
                        return;
                    }

                    if (newValue !== oldValue) {
                        $scope.agentMaxPT = generateAgentMaxPT();
                    }
                });

                // Agent Max PT
                function generateAgentMaxPT() {
                    var min = 0;
                    var max = $scope.masterMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.A4
                        : ptService.correctValue(min, max, $scope.agentMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.agentMaxPT = generateAgentMaxPT();

                // Super Min PT
                function generateSuperMinPT() {
                    var min = 0;
                    var max = $scope.superMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N4
                        : ptService.correctValue(min, max, $scope.superMinPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.superMinPT = generateSuperMinPT();

                // Copy
                $scope.copy = function (source) {
                    return ptService.copyValue(source, $scope, 'superMaxPT', generateSuperMaxPT)
                        && ptService.copyValue(source, $scope, 'masterMaxPT', generateMasterMaxPT)
                        && ptService.copyValue(source, $scope, 'agentMaxPT', generateAgentMaxPT)
                        && ptService.copyValue(source, $scope, 'superMinPT', generateSuperMinPT);
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (positionTaking.S4 === $scope.superMaxPT.value
                        && positionTaking.M4 === $scope.masterMaxPT.value
                        && positionTaking.A4 === $scope.agentMaxPT.value
                        && positionTaking.N4 === $scope.superMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            BetType: positionTaking.BetType,
                            IsLive: positionTaking.IsLive,
                            S4: $scope.superMaxPT.value,
                            M4: $scope.masterMaxPT.value,
                            A4: $scope.agentMaxPT.value,
                            N4: $scope.superMinPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    fanexPtCompanyEditSuperItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtCompanyEditSuperItem', fanexPtCompanyEditSuperItemDirective);
})();