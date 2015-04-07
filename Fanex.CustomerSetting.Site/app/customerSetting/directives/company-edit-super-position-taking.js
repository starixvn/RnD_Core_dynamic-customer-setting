'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var companyEditSuperPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=companyEditSuperPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/company-edit-super-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Super Max PT
                function generateSuperMaxPT() {
                    var min = 0;
                    var max = positionTaking.DefaultMaxPT;
                    var value = positionTaking.S4;
                    if (isInitialized) {
                        value = commonService.correctValue(min, max, value);
                    }
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.superMaxPT = generateSuperMaxPT();
                $scope.$watch('superMaxPT.value', function (newValue, oldValue) {
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
                        : commonService.correctValue(min, max, $scope.masterMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
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
                        : commonService.correctValue(min, max, $scope.agentMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.agentMaxPT = generateAgentMaxPT();

                // Super Min PT
                function generateSuperMinPT() {
                    var min = 0;
                    var max = $scope.superMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N4
                        : commonService.correctValue(min, max, $scope.superMinPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.superMinPT = generateSuperMinPT();

                // Copy
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }
                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.superMaxPT.value = copyScope.superMaxPT.value;
                        $scope.masterMaxPT.value = copyScope.masterMaxPT.value;
                        $scope.agentMaxPT.value = copyScope.agentMaxPT.value;
                        $scope.superMinPT.value = copyScope.superMinPT.value;
                    }
                });

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

    companyEditSuperPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('companyEditSuperPositionTaking', companyEditSuperPositionTakingDirective);
})();