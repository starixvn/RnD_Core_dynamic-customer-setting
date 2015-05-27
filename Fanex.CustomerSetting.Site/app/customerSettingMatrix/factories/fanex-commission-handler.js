(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexCommonService', 'fanexSettingLevelHandler', 'fanexRoleTypeHandler'];
    var fanexCommissionHandler = function (appSettings, commonService, SettingLevelHandler, RoleTypeHandler) {
        function SettingTable(name, rows) {
            var columns = rows[0].Columns;
            var totalOfColumn = columns.length;
            return {
                Name: name,
                Rows: rows,
                Columns: columns,
                TotalOfColumn: totalOfColumn
            };
        }

        function SettingRow(name, columns) {
            return {
                Name: name,
                Columns: columns
            };
        }

        function SettingColumn(name, value) {
            return {
                Name: name,
                Value: value
            };
        }

        function inArray(value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        }

        function getRoleIds(settings) {
            var roleId;
            var roleIds = [];
            var index = 0;
            while (index < settings.length) {
                var setting = settings[index];
                if (roleId !== setting.RoleId) {
                    roleId = setting.RoleId;
                    if (inArray(roleId, roleIds) === -1) {
                        roleIds.push(roleId);
                    }
                }

                index++;
            }

            return roleIds;
        }

        function createTable(settingLevel, settings) {
            var settingLevelHandler = new SettingLevelHandler(settingLevel);
            var name = settingLevelHandler.getCommissionTableName(settings[0]);
            var roleIds = getRoleIds(settings);
            var rows = [];
            for (var i = 0; i < roleIds.length; i++) {
                var roleId = roleIds[i];
                var roleSettings = [];
                for (var j = 0; j < settings.length; j++) {
                    var setting = settings[j];
                    if (setting.RoleId === roleId) {
                        roleSettings.push(setting);
                    }
                }

                var row = createRow(settingLevel, roleSettings);
                rows.push(row);
            }

            return new SettingTable(name, rows);
        }

        function createRow(settingLevel, settings) {
            var settingLevelHandler = new SettingLevelHandler(settingLevel);
            var roleId = settings[0].RoleId;
            var roleTypeHandler = new RoleTypeHandler(roleId);
            var rowName = roleTypeHandler.RoleName;
            var columns = [];
            for (var i = 0; i < settings.length; i++) {
                var setting = settings[i];
                var columnValue = setting.Discount;
                var columnName = settingLevelHandler.getCommissionColumnName(setting);
                var column = new SettingColumn(columnName, columnValue);
                columns.push(column);
            }

            return new SettingRow(rowName, columns);
        }

        function CommissionHandler(roleId, settingLevel) {
            this.roleId = roleId;
            this.settingLevel = settingLevel;
            this.createTables = function (settings) {
                var tables = [];
                var settingLevelHandler = new SettingLevelHandler(this.settingLevel);
                var groups = commonService.group(settings, settingLevelHandler.isSameCommissionTable);
                for (var i = 0; i < groups.length; i++) {
                    var tableSettings = groups[i];
                    var table = createTable(this.settingLevel, tableSettings);
                    tables.push(table);
                }

                return tables;
            };
        }

        return CommissionHandler;
    };

    fanexCommissionHandler.$inject = injectParams;
    angular.module('customerSettingMatrixApp').factory('fanexCommissionHandler', fanexCommissionHandler);
})();