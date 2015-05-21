(function () {
    'use strict';
    var injectParams = ['$rootScope', 'fanexAppSettings', 'fanexUpdateMemberPtService'];
    var fanexPtUpdateMemberDialogDirective = function ($rootScope, appSettings, updateMemberPtService) {
        return {
            restrict: 'AE',
            replace: true,
            templateUrl: 'templates/fanex-pt-update-member-dialog.htm',
            controller: ['$scope', function ($scope) {
                updateMemberPtService.setup($scope);
                $scope.settingType = appSettings.settingType.PositionTaking;
                $scope.isShow = false;
                $scope.clickClose = function () {
                    $scope.isShow = false;
                };
                $scope.clickYes = function () {
                    $scope.isShow = false;
                    update(true);
                };
                $scope.clickNo = function () {
                    $scope.isShow = false;
                    update(false);
                };

                function update(isUpdateAllMembers) {
                    var memberPTs = [];
                    angular.forEach($scope.pts, function (pt) {
                        memberPTs.push({
                            SportType: pt.sportType,
                            BetType: pt.betType,
                            IsLive: pt.isLive,
                            Pt: isUpdateAllMembers ? pt.value : 0
                        });
                    });
                    $rootScope.$broadcast('update-member-position-taking', {
                        IsUpdateAllMembers: isUpdateAllMembers,
                        MemberPTs: memberPTs
                    });
                }
            } ]
        };
    };

    fanexPtUpdateMemberDialogDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtUpdateMemberDialog', fanexPtUpdateMemberDialogDirective);
})();