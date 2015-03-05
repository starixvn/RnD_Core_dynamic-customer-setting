'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'linkingService', 'appSettings'];
    var companySelectSuperPositionTakingDirective = function ($rootScope, commonService, resourceService, linkingService, appSettings) {
        function parse(data) {
            return {
                superMaxPT: commonService.createPositionTakingSelect(0, data.DefaultMaxPT, data.S4),
                masterMaxPT: commonService.createPositionTakingSelect(0, data.S4, data.M4),
                agentMaxPT: commonService.createPositionTakingSelect(0, data.M4, data.A4),
                superMinPT: commonService.createPositionTakingSelect(0, data.S4, data.N4)
            };
        }

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
                linkingService.linkPositionTaking($scope);
                $scope.resourceService = resourceService;
                var name = resourceService.getDisplayName($scope.settingLevel, $scope.settingContext, $scope.positionTaking);
                if ($scope.positionTaking.IsLive) {
                    name += ' (' + resourceService.getString('Live') + ')';
                }
                $scope.name = name;
                var positionTaking = parse($scope.positionTaking, $scope.formType);
                $scope.superMaxPT = positionTaking.superMaxPT;
                $scope.masterMaxPT = positionTaking.masterMaxPT;
                $scope.agentMaxPT = positionTaking.agentMaxPT;
                $scope.superMinPT = positionTaking.superMinPT;
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

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ($scope.positionTaking.S4 === $scope.superMaxPT.value
                        && $scope.positionTaking.M4 === $scope.masterMaxPT.value
                        && $scope.positionTaking.A4 === $scope.agentMaxPT.value
                        && $scope.positionTaking.N4 === $scope.superMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.positionTaking.SportType,
                            BetType: $scope.positionTaking.BetType,
                            S4: $scope.superMaxPT.value,
                            M4: $scope.masterMaxPT.value,
                            A4: $scope.agentMaxPT.value,
                            N4: $scope.superMinPT.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                scope.$watch('superMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = parseInt(scope.superMaxPT.value);
                        scope.masterMaxPT = commonService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.masterMaxPT.value)));
                        scope.superMinPT = commonService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.superMinPT.value)));
                    }
                });

                scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var min = 0;
                        var max = parseInt(scope.masterMaxPT.value);
                        scope.agentMaxPT = commonService.createPositionTakingSelect(min, max, Math.min(max, parseInt(scope.agentMaxPT.value)));
                    }
                });
            }
        };
    };

    companySelectSuperPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('companySelectSuperPositionTaking', companySelectSuperPositionTakingDirective);
})();