'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService'];
    var errorService = function ($rootScope, commonService) {
        this.show = function (message) {
            $rootScope.hasError = true;
            $rootScope.errorMessage = message || 'Unknown error';
            commonService.scrollToTop();
        };

        this.hide = function () {
            $rootScope.hasError = false;
            $rootScope.errorMessage = undefined;
        };
    };

    errorService.$inject = injectParams;
    angular.module('customerSettingApp').service('errorService', errorService);
})();