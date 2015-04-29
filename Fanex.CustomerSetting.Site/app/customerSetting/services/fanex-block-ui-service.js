'use strict';

(function () {
    var injectParams = [];
    var fanexBlockUiService = function () {
        var blockUiScope;

        this.setup = function (scope) {
            if (angular.isDefined(blockUiScope)) {
                throw 'Only one block ui component in document';
            }

            blockUiScope = scope;
            blockUiScope.hasLoading = false;
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

    fanexBlockUiService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexBlockUiService', fanexBlockUiService);
})();