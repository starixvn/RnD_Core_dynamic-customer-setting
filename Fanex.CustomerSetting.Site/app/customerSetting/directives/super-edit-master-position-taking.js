'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'linkingService', 'appSettings'];
    var superEditMasterPositionTakingDirective = function ($rootScope, commonService, resourceService, linkingService, appSettings) {
        function parse(data) {
            var superPT = 0;
            var masterMaxPTMin = 0;
            var masterMaxPTMax = 0;
            if (!data.IS3) {
                superPT = data.S3;
                masterMaxPTMin = data.N4 - data.S3;
                masterMaxPTMax = Math.min(data.S4 - data.S3, data.M4);
            } else {
                superPT = data.US3;
                masterMaxPTMin = data.N4 - data.US3;
                masterMaxPTMax = Math.min(data.S4, data.M4);
            }
            return {
                superPT: commonService.createPositionTakingSelect(0, data.S4, superPT),
                superIsAuto: data.IS3,
                masterMaxPT: commonService.createPositionTakingSelect(masterMaxPTMin, masterMaxPTMax, data.M3),
                masterMinPT: commonService.createPositionTakingSelect(0, data.M3, data.N3)
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMasterPositionTaking',
                settingLevel: '=',
                settingContext: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/super-edit-master-position-taking.htm',
            controller: ['$scope', function ($scope) {
                linkingService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, $scope.formType);
                $scope.superPT = positionTaking.superPT;
                $scope.superIsAuto = positionTaking.superIsAuto;
                $scope.masterMaxPT = positionTaking.masterMaxPT;
                $scope.masterMinPT = positionTaking.masterMinPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.superPT.value = copyScope.superPT.value;
                        $scope.superIsAuto = copyScope.superIsAuto;
                        $scope.masterMaxPT.value = copyScope.masterMaxPT.value;
                        $scope.masterMinPT.value = copyScope.masterMinPT.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.superIsAuto ? $scope.positionTaking.US3 === $scope.superPT.value : $scope.positionTaking.S3 === $scope.superPT.value)
                        && $scope.positionTaking.IS3 === $scope.superIsAuto
                        && $scope.positionTaking.M3 === $scope.masterMaxPT.value
                        && $scope.positionTaking.N3 === $scope.masterMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            SportTypeName: $scope.positionTaking.SportTypeName,
                            BetType: $scope.positionTaking.BetType,
                            BetTypeName: $scope.positionTaking.BetTypeName,
                            IsLive: $scope.positionTaking.IsLive,
                            S3: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US3: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS3: $scope.superIsAuto,
                            M3: $scope.masterMaxPT.value,
                            N3: $scope.masterMinPT.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                function updateMasterMaxPT(){
                    if (!scope.superIsAuto) {
                        scope.masterMaxPT = commonService.createPositionTakingSelect(
                            scope.positionTaking.N4 - scope.superPT.value,
                            Math.min(scope.positionTaking.S4 - scope.superPT.value, scope.positionTaking.M4),
                            scope.masterMaxPT.value);
                    } else {
                        scope.masterMaxPT = commonService.createPositionTakingSelect(
                            scope.positionTaking.N4 - scope.superPT.value,
                            Math.min(scope.positionTaking.S4, scope.positionTaking.M4),
                            scope.masterMaxPT.value);
                    }
                }

                scope.$watch('superPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        updateMasterMaxPT();
                    }
                });

                scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var superPT = scope.superIsAuto ? scope.positionTaking.US3 : scope.positionTaking.S3;
                        scope.superPT = commonService.createPositionTakingSelect(0, scope.positionTaking.S4, superPT);
                        updateMasterMaxPT();
                    }
                });

                scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        scope.masterMinPT = commonService.createPositionTakingSelect(0, scope.masterMaxPT.value, scope.masterMinPT.value);
                    }
                });
            }
        };
    };

    superEditMasterPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMasterPositionTaking', superEditMasterPositionTakingDirective);
})();