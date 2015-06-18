(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtSuperEditMasterItemDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtSuperEditMasterItem',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-super-edit-master-item.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

                // Super Is Auto
                $scope.superIsAuto = positionTaking.IS3;
                $scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.superPT = generateSuperPT();
                        $scope.masterMaxPT = generateMasterMaxPT();
                    }
                });

                // Super PT
                function generateSuperPT() {
                    var min = !$scope.superIsAuto ? Math.max(0, positionTaking.N4 - positionTaking.M4) : 0;
                    var max = positionTaking.S4;
                    var value = !isInitialized
                        ? !$scope.superIsAuto ? positionTaking.S3 : positionTaking.US3
                        : ptService.correctValue(min, max, $scope.superPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.superPT = generateSuperPT();
                $scope.$watch('superPT.value', function (newValue, oldValue) {
                    if ($scope.masterMaxPT !== null && $scope.masterMaxPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.masterMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if ($scope.masterMinPT !== null && $scope.masterMinPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.masterMinPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterMinPT = generateMasterMinPT();

                // Copy
                $scope.copy = function (source) {
                    $scope.superIsAuto = source.superIsAuto;
                    return ptService.copyValue(source, $scope, 'superPT', generateSuperPT)
                        && ptService.copyValue(source, $scope, 'masterMaxPT', generateMasterMaxPT)
                        && ptService.copyValue(source, $scope, 'masterMinPT', generateMasterMinPT);
                };

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

    fanexPtSuperEditMasterItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSuperEditMasterItem', fanexPtSuperEditMasterItemDirective);
})();