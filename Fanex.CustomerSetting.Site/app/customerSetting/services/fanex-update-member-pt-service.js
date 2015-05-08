'use strict';

(function () {
    var injectParams = ['fanexPtService'];
    var fanexUpdateMemberPtService = function (ptService) {
        var $scope;

        function setup(scope) {
            $scope = scope;
        }

        function hide() {
            $scope.isShow = false;
        }

        function show(memberPts) {
            var pts = [];
            angular.forEach(memberPts, function (memberPt) {
                var ptSelect = ptService.createSelect(0, memberPt.Pt, memberPt.Pt);
                pts.push({
                    sportType: memberPt.SportType,
                    betType: memberPt.BetType,
                    isLive: memberPt.IsLive,
                    name: memberPt.Name,
                    value: ptSelect.value,
                    options: ptSelect.options
                });
            });
            $scope.isShow = true;
            $scope.pts = pts;
        }

        return {
            setup: setup,
            show: show,
            hide: hide
        };
    };

    fanexUpdateMemberPtService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexUpdateMemberPtService', fanexUpdateMemberPtService);
})();