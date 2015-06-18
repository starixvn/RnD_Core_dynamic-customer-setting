(function () {
    'use strict';
    var injectParams = ['fanexDiscountService', 'fanexResourceService'];
    var fanexDiscountMasterEditMemberItemDirective = function (discountService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountMasterEditMemberItem',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-discount-master-edit-member-item.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.discount);
                var discount = $scope.discount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexDiscountMasterEditMemberItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountMasterEditMemberItem', fanexDiscountMasterEditMemberItemDirective);
})();