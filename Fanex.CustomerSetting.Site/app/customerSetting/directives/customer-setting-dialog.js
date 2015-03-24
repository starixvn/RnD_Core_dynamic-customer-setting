'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService'];
    var customerSettingDialogDirective = function ($rootScope, commonService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                data: '=customerSettingDialog'
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-dialog.htm',
            link: function (scope) {
                scope.resourceService = resourceService;
                scope.isShow = false;
                function update(isUpdateAllMembers) {
                    var memberPTs = [];
                    angular.forEach(scope.pts, function (pt) {
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

                scope.clickClose = function () {
                    scope.isShow = false;
                };
                scope.clickYes = function () {
                    scope.isShow = false;
                    update(true);
                };
                scope.clickNo = function () {
                    scope.isShow = false;
                    update(false);
                };

                scope.$watch('data', function (newValue, oldValue) {
                    if (newValue === oldValue) {
                        return;
                    }

                    if (angular.isUndefined(scope.data)) {
                        scope.isShow = false;
                        return;
                    }

                    var memberPtss = scope.data.MemberPTs;
                    var pts = [];
                    angular.forEach(memberPtss, function (memberPts) {
                        var ptSelect = commonService.createPositionTakingSelect(0, memberPts.Pt, memberPts.Pt);
                        pts.push({
                            sportType: memberPts.SportType,
                            betType: memberPts.BetType,
                            isLive: memberPts.IsLive,
                            name: memberPts.Name,
                            value: ptSelect.value,
                            options: ptSelect.options
                        });
                    });
                    scope.isShow = true;
                    scope.pts = pts;
                });
            }
        };
    };

    customerSettingDialogDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingDialog', customerSettingDialogDirective);
})();