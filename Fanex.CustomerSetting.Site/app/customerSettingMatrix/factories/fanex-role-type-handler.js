(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexSettingLevelHandler'];
    var fanexRoleTypeHandler = function (appSettings, SettingLevelHandler) {
        function SettingTable(name, rows) {
            return {
                Name: name,
                Rows: rows
            };
        }

        var BaseHandler = function (total) {
            this.total = total;
            this.generateTables = function (settingLevel, settings) {
                var settingLevelHandler = new SettingLevelHandler(settingLevel);
                var tables = [];
                var length = settings.length / total;
                for (var i = 0; i < length; i++) {
                    var name;
                    var rows = [];
                    for (var j = 0; j < total; j++) {
                        var index = i * total + j;
                        var setting = settings[index];
                        var roleId = setting.RoleId;
                        var roleTypeHandler = new RoleTypeHandler(roleId);
                        var row = roleTypeHandler.createRow(setting);
                        name = settingLevelHandler.getTableName(setting);
                        angular.extend(row, {
                            SportType: setting.SportType,
                            SportTypeName: setting.SportTypeName,
                            BetType: setting.BetType,
                            BetTypeName: setting.BetTypeName,
                            IsLive: setting.IsLive,
                            RoleId: setting.RoleId
                        });
                        rows.push(row);
                    }

                    var table = new SettingTable(name, rows);
                    tables.push(table);
                }

                return tables;
            };
        };

        var SuperHandler = function () {
            BaseHandler.call(this, 1);
            this.RoleName = 'Super (4)';
            this.createRow = function (setting) {
                return {
                    SuperPT: setting.SuperPT,
                    MasterPT: setting.MasterPT,
                    AgentPT: setting.AgentPT,
                    MinPT: setting.MinPT
                };
            };
        };

        var MasterHandler = function () {
            BaseHandler.call(this, 2);
            this.RoleName = 'Master (3)';
            this.createRow = function (setting) {
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
            BaseHandler.call(this, 3);
            this.RoleName = 'Agent (2)';
            this.createRow = function (setting) {
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
            BaseHandler.call(this, 4);
            this.RoleName = 'Member (1)';
            this.createRow = function (setting) {
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