(function () {
    'use strict';
    var injectParams = [];
    var fanexBlockUiService = function () {
        var blockUiScope;

        function setup(scope) {
            if (angular.isDefined(blockUiScope)) {
                throw 'Only one block ui component in document';
            }

            blockUiScope = scope;
            blockUiScope.hasLoading = false;
        }

        function show() {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = true;
        }

        function hide() {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = false;
        }

        return {
            setup: setup,
            show: show,
            hide: hide
        };
    };

    fanexBlockUiService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexBlockUiService', fanexBlockUiService);
})();