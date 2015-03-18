'use strict';

(function () {
    var injectParams = ['commonService'];
    var customerSettingDialogDirective = function (commonService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                data: '=customerSettingDialog'
            },
            templateUrl: '/app/customerSetting/templates/customer-setting-dialog.htm',
            link: function (scope) {
                scope.isShow = false;
                scope.hide = function () {
                    scope.isShow = false;
                };

                scope.$watch('data', function (newValue, oldValue) {
                    if (newValue === oldValue) {
                        return;
                    }

                    if (angular.isUndefined(scope.data)) {
                        scope.isShow = false;
                        return;
                    }

                    var memberPTs = scope.data.MemberPTs;
                    var pts = [];
                    angular.forEach(memberPTs, function(memberPT){
                        var ptSelect = commonService.createPositionTakingSelect(0, memberPT.Pt, memberPT.Pt);
                        pts.push({
                            name: memberPT.Name,
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