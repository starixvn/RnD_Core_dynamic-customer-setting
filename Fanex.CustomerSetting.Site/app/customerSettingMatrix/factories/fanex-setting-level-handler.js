(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexResourceService'];
    var fanexSettingLevelHandler = function (appSettings, resourceService) {
        function SettingGroup(name) {
            this.Name = name;
            this.Tables = [];
        }

        var ProductHandler = function () {
            this.getTableName = function () {
                return undefined;
            };
            this.generateGroups = function (tables) {
                var isLive;
                var groups = [];
                var group;
                angular.forEach(tables, function (table) {
                    var row = table.Rows[0];
                    if (row.IsLive !== isLive) {
                        isLive = row.IsLive;
                        var name = resourceService.ProductName;
                        name += row.IsLive ? ' (Live)' : '';
                        group = new SettingGroup(name);
                        groups.push(group);
                    }

                    group.Tables.push(table);
                });

                return groups;
            };
        };

        var SportTypeHandler = function () {
            this.getTableName = function () {
                return undefined;
            };
            this.generateGroups = function (tables) {
                var isLive;
                var groups = [];
                var group;
                angular.forEach(tables, function (table) {
                    var row = table.Rows[0];
                    if (row.IsLive !== isLive) {
                        isLive = row.IsLive;
                        var name = row.SportTypeName;
                        name += row.IsLive ? ' (Live)' : '';
                        group = new SettingGroup(name);
                        groups.push(group);
                    }

                    group.Tables.push(table);
                });

                return groups;
            };
        };

        var BetTypeHandler = function () {
            this.getTableName = function () {
                return undefined;
            };
            this.generateGroups = function (tables) {
                var isLive;
                var groups = [];
                var group;
                angular.forEach(tables, function (table) {
                    var row = table.Rows[0];
                    if (row.IsLive !== isLive) {
                        isLive = row.IsLive;
                        var name = row.BetTypeName;
                        name += row.IsLive ? ' (Live)' : '';
                        group = new SettingGroup(name);
                        groups.push(group);
                    }

                    group.Tables.push(table);
                });

                return groups;
            };
        };

        var SportTypeBetTypeHandler = function () {
            this.getTableName = function (row) {
                var name = row.BetTypeName;
                if (row.IsLive) {
                    name += ' (Live)';
                }
                return name;
            };
            this.generateGroups = function (tables) {
                var sportType;
                var groups = [];
                var group;
                angular.forEach(tables, function (table) {
                    var row = table.Rows[0];
                    if (row.SportType !== sportType) {
                        sportType = row.SportType;
                        var name = row.SportTypeName;
                        group = new SettingGroup(name);
                        groups.push(group);
                    }

                    group.Tables.push(table);
                });

                return groups;
            };
        };

        function SettingLevelHandler(settingLevel) {
            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return new ProductHandler();

                case appSettings.settingLevel.SportType:
                    return new SportTypeHandler();

                case appSettings.settingLevel.BetType:
                    return new BetTypeHandler();

                case appSettings.settingLevel.SportTypeBetType:
                    return new SportTypeBetTypeHandler();

                default:
                    throw 'Not implemented yet.';
            }
        }

        return SettingLevelHandler;
    };

    fanexSettingLevelHandler.$inject = injectParams;
    angular.module('customerSettingMatrixApp').factory('fanexSettingLevelHandler', fanexSettingLevelHandler);
})();