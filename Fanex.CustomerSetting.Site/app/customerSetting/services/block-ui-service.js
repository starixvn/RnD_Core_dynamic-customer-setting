'use strict';

(function () {
    var injectParams = [];
    var blockUiService = function () {
        var blockUiScope;

        this.setup = function (scope) {
            if (angular.isDefined(blockUiScope)) {
                throw 'Only one block ui component in document';
            }

            blockUiScope = scope;
        };

        this.show = function () {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = true;
        };

        this.hide = function () {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = false;
        };
    };

    blockUiService.$inject = injectParams;
    angular.module('customerSettingApp').service('blockUiService', blockUiService);
})();