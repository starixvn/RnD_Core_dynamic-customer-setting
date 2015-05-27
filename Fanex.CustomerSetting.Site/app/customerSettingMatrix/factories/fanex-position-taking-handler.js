(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexSettingLevelHandler', 'fanexRoleTypeHandler'];
    var fanexPositionTakingHandler = function (appSettings, SettingLevelHandler, RoleTypeHandler) {
        function SettingTable(name, rows) {
            return {
                Name: name,
                Rows: rows
            };
        }

        function SettingGroup(name) {
            this.Name = name;
            this.Tables = [];
        }

        function createTables(roleId, settingLevel, settings) {
            var roleTypeHandler = new RoleTypeHandler(roleId);
            var settingLevelHandler = new SettingLevelHandler(settingLevel);
            var total = roleTypeHandler.PositionTakingRowTotal;
            var tables = [];
            var length = settings.length / total;
            for (var i = 0; i < length; i++) {
                var tableName;
                var tableRows = [];
                for (var j = 0; j < total; j++) {
                    var index = i * total + j;
                    var setting = settings[index];
                    var settingRoleId = setting.RoleId;
                    roleTypeHandler = new RoleTypeHandler(settingRoleId);
                    var tableRow = roleTypeHandler.createPositionTakingRow(setting);
                    tableName = settingLevelHandler.getPositionTakingTableName(setting);
                    angular.extend(tableRow, {
                        SportType: setting.SportType,
                        SportTypeName: setting.SportTypeName,
                        BetType: setting.BetType,
                        BetTypeName: setting.BetTypeName,
                        IsLive: setting.IsLive,
                        RoleId: setting.RoleId
                    });

                    tableRows.push(tableRow);
                }

                var table = new SettingTable(tableName, tableRows);
                tables.push(table);
            }

            return tables;
        }

        function createGroups(settingLevel, tables) {
            var groups = [];
            var group;
            var currentTable;
            var settingLevelHandler = new SettingLevelHandler(settingLevel);
            angular.forEach(tables, function (table) {
                if (!settingLevelHandler.isSamePositionTakingGroup(currentTable, table)) {
                    currentTable = table;
                    var tableRow = table.Rows[0];
                    var groupName = settingLevelHandler.getPositionTakingGroupName(tableRow);
                    groupName += tableRow.IsLive ? ' (Live)' : '';
                    group = new SettingGroup(groupName);
                    groups.push(group);
                }

                group.Tables.push(table);
            });

            return groups;
        }

        function PositionTakingHandler(roleId, settingLevel) {
            this.roleId = roleId;
            this.settingLevel = settingLevel;
            this.createGroups = function (settings) {
                var tables = createTables(this.roleId, this.settingLevel, settings);
                return createGroups(this.settingLevel, tables);
            };
        }

        return PositionTakingHandler;
    };

    fanexPositionTakingHandler.$inject = injectParams;
    angular.module('customerSettingMatrixApp').factory('fanexPositionTakingHandler', fanexPositionTakingHandler);
})();