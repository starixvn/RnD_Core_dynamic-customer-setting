(function () {
    'use strict';
    var injectParams = ['fanexRequestService', 'fanexDirectiveBlock'];
    var fanexPtMasterEditAgentBlockDirective = function (requestService, DirectiveBlock) {
        return new DirectiveBlock('templates/fanex-pt-master-edit-agent-block.htm', function (scope) {
            var queryData = requestService.getQueryData();
            var settingContext = queryData.SettingContext;
            scope.disableAutoMPT = settingContext.DisableAutoMPT;
        });
    };

    fanexPtMasterEditAgentBlockDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtMasterEditAgentBlock', fanexPtMasterEditAgentBlockDirective);
})();