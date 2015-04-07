'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMasterPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMasterPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/super-edit-master-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                var isCopied = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Super Is Auto
                $scope.superIsAuto = positionTaking.IS3;
                $scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.superPT = generateSuperPT();
                        $scope.masterMaxPT = generateMasterMaxPT();
                        isCopied = false;
                    }
                });

                // Super PT
                function generateSuperPT() {
                    var min = !$scope.superIsAuto ? Math.max(0, positionTaking.N4 - positionTaking.M4) : 0;
                    var max = positionTaking.S4;
                    var value = isCopied
                        ? $scope.superPT.value
                        : !$scope.superIsAuto ? positionTaking.S3 : positionTaking.US3;
                    if (isInitialized) {
                        value = commonService.correctValue(min, max, value);
                    }
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.superPT = generateSuperPT();
                $scope.$watch('superPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterMaxPT = generateMasterMaxPT();
                    }
                });

                // Master Max PT
                function generateMasterMaxPT() {
                    var min = Math.max(0, positionTaking.N4 - $scope.superPT.value);
                    var max = !$scope.superIsAuto
                        ? Math.min(positionTaking.S4 - $scope.superPT.value, positionTaking.M4)
                        : Math.min(positionTaking.S4, positionTaking.M4);
                    var value = !isInitialized
                        ? positionTaking.M3
                        : commonService.correctValue(min, max, $scope.masterMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterMinPT = generateMasterMinPT();
                    }
                });

                // Master Min PT
                function generateMasterMinPT() {
                    var min = 0;
                    var max = $scope.masterMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N3
                        : commonService.correctValue(min, max, $scope.masterMinPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterMinPT = generateMasterMinPT();

                // Copy
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }
                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        isCopied = true;
                        $scope.superIsAuto = copyScope.superIsAuto;
                        $scope.superPT.value = copyScope.superPT.value;
                        $scope.masterMaxPT.value = copyScope.masterMaxPT.value;
                        $scope.masterMinPT.value = copyScope.masterMinPT.value;
                    }
                });

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.superIsAuto ? positionTaking.S3 : positionTaking.US3) === $scope.superPT.value
                        && positionTaking.IS3 === $scope.superIsAuto
                        && positionTaking.M3 === $scope.masterMaxPT.value
                        && positionTaking.N3 === $scope.masterMinPT.value) {
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
                            S3: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US3: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS3: $scope.superIsAuto,
                            M3: $scope.masterMaxPT.value,
                            N3: $scope.masterMinPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    superEditMasterPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMasterPositionTaking', superEditMasterPositionTakingDirective);
})();