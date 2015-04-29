'use strict';

(function () {
    var injectParams = ['fanexCommonService', 'fanexResourceService', 'fanexControllerService', 'fanexAppSettings'];
    var fanexDiscountMasterEditMemberDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountMasterEditMember'
            },
            templateUrl: 'templates/fanex-discount-master-edit-member.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexDiscountMasterEditMemberDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountMasterEditMember', fanexDiscountMasterEditMemberDirective);
})();