(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexResourceService'];
    var fanexSettingLevelHandler = function (appSettings, resourceService) {
        var BaseHandler = function () {
            this.getPositionTakingTableName = function () {
                return undefined;
            };
            this.getCommissionTableName = function () {
                return undefined;
            };
            this.isSamePositionTakingGroup = function (table1, table2) {
                return table1 !== undefined
                    && table2 !== undefined
                    && table1.Rows[0].IsLive === table2.Rows[0].IsLive;
            };
            this.isSameCommissionTable = function () {
                return true;
            };
        };

        var ProductHandler = function () {
            BaseHandler.call(this);
            this.getPositionTakingGroupName = function () {
                return resourceService.ProductName;
            };
            this.getCommissionColumnName = function () {
                return 'Comm';
            };
        };

        var SportTypeHandler = function () {
            BaseHandler.call(this);
            this.getPositionTakingGroupName = function (row) {
                return row.SportTypeName;
            };
            this.getCommissionColumnName = function (setting) {
                return setting.SportTypeName;
            };
        };

        var BetTypeHandler = function () {
            BaseHandler.call(this);
            this.getPositionTakingGroupName = function (row) {
                return row.BetTypeName;
            };
            this.getCommissionColumnName = function (setting) {
                return setting.BetTypeName;
            };
        };

        var SportTypeBetTypeHandler = function () {
            BaseHandler.call(this);
            this.getPositionTakingTableName = function (setting) {
                var name = setting.BetTypeName;
                if (setting.IsLive) {
                    name += ' (Live)';
                }
                return name;
            };
            this.getPositionTakingGroupName = function (row) {
                return row.SportTypeName;
            };
            this.getCommissionColumnName = function (setting) {
                return this.getPositionTakingTableName(setting);
            };
            this.getCommissionTableName = function (setting) {
                return setting.SportTypeName;
            };
            this.isSamePositionTakingGroup = function (table1, table2) {
                return table1 !== undefined
                    && table2 !== undefined
                    && table1.Rows[0].SportType === table2.Rows[0].SportType;
            };
            this.isSameCommissionTable = function (setting1, setting2) {
                return setting1 !== undefined
                    && setting2 !== undefined
                    && setting1.SportType === setting2.SportType;
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