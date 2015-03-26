'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            var superPT = 0;
            var superPTMin = Math.max(0, data.N4 - data.M1 - data.A1);
            var superPTMax = 0;
            if (!data.IS1) {
                superPT = data.S1;
                superPTMax = Math.max(data.N4 - data.M1 - data.A1, data.S4 - data.M3);
            } else {
                superPT = data.US1;
                superPTMax = data.S4;
            }
            return {
                superPT: commonService.createPositionTakingSelect(superPTMin, superPTMax, superPT),
                superIsAuto: data.IS1,
                masterPT: data.M1 / appSettings.positionTakingDivider,
                agentPT: data.A1 / appSettings.positionTakingDivider
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/super-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.superPT = positionTaking.superPT;
                $scope.superIsAuto = positionTaking.superIsAuto;
                $scope.masterPT = positionTaking.masterPT;
                $scope.agentPT = positionTaking.agentPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.superPT.value = copyScope.superPT.value;
                        $scope.superIsAuto = copyScope.superIsAuto;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.superIsAuto ? $scope.positionTaking.US1 === $scope.superPT.value : $scope.positionTaking.S1 === $scope.superPT.value)
                        && $scope.positionTaking.IS1 === $scope.superIsAutoe) {
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
                            S1: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US1: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS1: $scope.superIsAuto
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var superPT = scope.superIsAuto ? scope.positionTaking.US1 : scope.positionTaking.S1;
                        var data = scope.positionTaking;
                        var superPTMin = Math.max(0, data.N4 - data.M1 - data.A1);
                        var superPTMax = 0;
                        if (!scope.superIsAuto) {
                            superPTMax = Math.max(data.N4 - data.M1 - data.A1, data.S4 - data.M3);
                        } else {
                            superPTMax = data.S4;
                        }
                        scope.superPT = commonService.createPositionTakingSelect(superPTMin, superPTMax, superPT);
                    }
                });
            }
        };
    };

    superEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMemberPositionTaking', superEditMemberPositionTakingDirective);
})();