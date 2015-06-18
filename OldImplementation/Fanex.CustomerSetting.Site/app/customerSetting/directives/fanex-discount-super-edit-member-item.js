(function () {
    'use strict';
    var injectParams = ['fanexDiscountService', 'fanexResourceService'];
    var fanexSuperEditMemberDiscountItemDirective = function (discountService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountSuperEditMemberItem',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-discount-super-edit-member-item.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.discount);
                var discount = $scope.discount;
                $scope.masterDiscount = discount.MasterDiscount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexSuperEditMemberDiscountItemDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountSuperEditMemberItem', fanexSuperEditMemberDiscountItemDirective);
})();