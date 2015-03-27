'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        function parse(data) {
            var masterPT = 0;
            var masterPTMin = Math.max(0, data.N3 - data.A1);
            var masterPTMax = 0;
            if (!data.IS1) {
                masterPT = data.S1;
                masterPTMax = Math.max(data.M3 - data.A1);
            } else {
                masterPT = data.US1;
                masterPTMax = data.M3;
            }
            return {
                masterPT: commonService.createPositionTakingSelect(masterPTMin, masterPTMax, masterPT),
                masterIsAuto: data.IM1,
                agentPT: data.A1 / appSettings.positionTakingDivider
            };
        }

        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=masterEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/master-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, $scope.positionTaking);
                var positionTaking = parse($scope.positionTaking, controllerService.formType);
                $scope.masterPT = positionTaking.masterPT;
                $scope.masterIsAuto = positionTaking.masterIsAuto;
                $scope.agentPT = positionTaking.agentPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.masterPT.value = copyScope.masterPT.value;
                        $scope.masterIsAuto = copyScope.masterIsAuto;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (($scope.masterIsAuto ? $scope.positionTaking.UM1 === $scope.masterPT.value : $scope.positionTaking.M1 === $scope.masterPT.value)
                        && $scope.positionTaking.IM1 === $scope.masterIsAuto) {
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
                            M1: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM1: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM1: $scope.masterIsAuto
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var masterPT = scope.masterIsAuto ? scope.positionTaking.UM1 : scope.positionTaking.M1;
                        var data = scope.positionTaking;
                        var masterPTMin = Math.max(0, data.N3 - data.A1);
                        var masterPTMax = 0;
                        if (!scope.superIsAuto) {
                            masterPTMax = Math.max(data.M3 - data.A1);
                        } else {
                            masterPTMax = data.M3;
                        }
                        scope.superPT = commonService.createPositionTakingSelect(masterPTMin, masterPTMax, masterPT);
                    }
                });
            }
        };
    };

    masterEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditMemberPositionTaking', masterEditMemberPositionTakingDirective);
})();