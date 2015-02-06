'use strict';

(function () {
    var injectParams = ['$rootScope', 'componentService', 'resourceService', 'appSettings'];
    var companySelectSuperPositionTakingDirective = function ($rootScope, componentService, resourceService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=companySelectSuperPositionTaking',
                settingLevel: '=',
                settingContext: '=',
                formType: '=',
                hasCopy: '='
            },
            templateUrl: '/app/customerSetting/templates/company-select-super-position-taking.htm',
            controller: ['$scope', function ($scope) {
                $scope.resourceService = resourceService;
                var name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.positionTaking);
                if ($scope.positionTaking.IsLive) {
                    name += ' (' + resourceService.getString('Live') + ')';
                }
                $scope.name = name;
                var positionTaking = componentService.parsePositionTaking($scope.positionTaking, $scope.formType);
                $scope.superMaxPT = positionTaking.superMaxPT;
                $scope.masterMaxPT = positionTaking.masterMaxPT;
                $scope.agentMaxPT = positionTaking.agentMaxPT;
                $scope.superMinPT = positionTaking.superMinPT;
                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-position-taking', $scope);
                    };
                }

                $scope.$on('copy-position-taking', function(event, copyScope){
                    if(copyScope !== $scope){
                        $scope.superMaxPT.value = copyScope.superMaxPT.value;
                        $scope.masterMaxPT.value = copyScope.masterMaxPT.value;
                        $scope.agentMaxPT.value = copyScope.agentMaxPT.value;
                        $scope.superMinPT.value = copyScope.superMinPT.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ($scope.betLimit.MinBet === $scope.minBet.value
                        && $scope.betLimit.MaxBet === $scope.maxBet.value
                        && $scope.betLimit.MaxPerMatch === $scope.maxPerMatch.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.betLimit.SportType,
                            BetType: $scope.betLimit.BetType,
                            MinBet: $scope.minBet.value,
                            MaxBet: $scope.maxBet.value,
                            MaxPerMatch: $scope.maxPerMatch.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('superMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = parseInt(scope.superMaxPT.value);
                        scope.masterMaxPT = componentService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.masterMaxPT.value)));
                        scope.superMinPT = componentService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.superMinPT.value)));
                    }
                });

                scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = parseInt(scope.masterMaxPT.value);
                        scope.agentMaxPT = componentService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.agentMaxPT.value)));
                    }
                });
            }
        };
    };

    companySelectSuperPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('companySelectSuperPositionTaking', companySelectSuperPositionTakingDirective);
})();