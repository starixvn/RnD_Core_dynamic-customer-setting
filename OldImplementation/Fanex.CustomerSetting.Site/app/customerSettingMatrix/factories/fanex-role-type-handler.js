(function () {
    'use strict';
    var injectParams = ['fanexAppSettings'];
    var fanexRoleTypeHandler = function (appSettings) {
        var BaseHandler = function (roleName, positionTakingRowTotal) {
            this.RoleName = roleName;
            this.PositionTakingRowTotal = positionTakingRowTotal;
        };

        var SuperHandler = function () {
            BaseHandler.call(this, 'Super (4)', 1);
            this.createPositionTakingRow = function (setting) {
                return {
                    SuperPT: setting.SuperPT,
                    MasterPT: setting.MasterPT,
                    AgentPT: setting.AgentPT,
                    MinPT: setting.MinPT
                };
            };
        };

        var MasterHandler = function () {
            BaseHandler.call(this, 'Master (3)', 2);
            this.createPositionTakingRow = function (setting) {
                return {
                    SuperPT: setting.SuperPT,
                    MasterPT: setting.MasterPT,
                    AutoSuperPT: setting.AutoSuperPT,
                    IsAutoSuperPT: setting.IsAutoSuperPT,
                    MinPT: setting.MinPT
                };
            };
        };

        var AgentHandler = function () {
            BaseHandler.call(this, 'Agent (2)', 3);
            this.createPositionTakingRow = function (setting) {
                return {
                    MasterPT: setting.MasterPT,
                    AgentPT: setting.AgentPT,
                    AutoMasterPT: setting.AutoMasterPT,
                    IsAutoMasterPT: setting.IsAutoMasterPT,
                    MinPT: setting.MinPT
                };
            };
        };

        var MemberHandler = function () {
            BaseHandler.call(this, 'Member (1)', 4);
            this.createPositionTakingRow = function (setting) {
                return {
                    SuperPT: setting.SuperPT,
                    MasterPT: setting.MasterPT,
                    AgentPT: setting.AgentPT,
                    AutoSuperPT: setting.AutoSuperPT,
                    IsAutoSuperPT: setting.IsAutoSuperPT,
                    AutoMasterPT: setting.AutoMasterPT,
                    IsAutoMasterPT: setting.IsAutoMasterPT
                };
            };
        };

        function RoleTypeHandler(roleType) {
            switch (roleType) {
                case appSettings.roleType.Super:
                    return new SuperHandler();

                case appSettings.roleType.Master:
                    return new MasterHandler();

                case appSettings.roleType.Agent:
                    return new AgentHandler();

                case appSettings.roleType.Member:
                    return new MemberHandler();

                default:
                    throw 'Not implemented yet.';
            }
        }

        return RoleTypeHandler;
    };

    fanexRoleTypeHandler.$inject = injectParams;
    angular.module('customerSettingMatrixApp').factory('fanexRoleTypeHandler', fanexRoleTypeHandler);
})();