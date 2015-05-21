(function () {
    'use strict';
    var injectParams = ['fanexRoleTypeHandler'];
    var fanexRoleFormat = function (RoleTypeHandler) {
        return function (roleId) {
            try {
                var roleTypeHandler = new RoleTypeHandler(roleId);
                return roleTypeHandler.RoleName;
            }
            catch (error) {
                return error;
            }
        };
    };

    fanexRoleFormat.$inject = injectParams;
    angular.module('customerSettingMatrixApp').filter('fanexRoleFormat', fanexRoleFormat);
})();