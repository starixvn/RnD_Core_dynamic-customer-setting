"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var fs = require("fs");
    var mssql = require("mssql");
    var path = require("path");

    gulp.task("product:configs:query", function () {
        var config = {
            user: "dbateam",
            password: "@bigboy#",
            server: "10.18.200.230\\SQLSERVER2008R2",
            database: "CustDB"
        };

        var connection = new mssql.Connection(config, function (error) {
            var request = new mssql.Request(connection);
            var query = "SELECT DISTINCT [ProductId], [SettingType], [SettingTemplate] FROM [CustDB].[dbo].[ProductSettingDetail]";

            if (error) {
                console.log(error);
            } else {
                request.query(query, function (error, recordSets, returnValue) {
                    if (error) {
                        console.log(error);
                    } else {
                        var productsConfig = [];
                        var currentProductConfig = {};

                        recordSets.forEach(function (productProfile) {
                            if (productProfile.ProductId !== currentProductConfig.productId) {
                                if (currentProductConfig.productId) {
                                    productsConfig.push(currentProductConfig);
                                }

                                currentProductConfig = {
                                    productId: productProfile.ProductId,
                                    settingTypes: []
                                };
                            }

                            currentProductConfig.settingTypes.push({
                                settingType: productProfile.SettingType,
                                settingTemplate: productProfile.SettingTemplate
                            });
                        });

                        if (currentProductConfig.productId) {
                            productsConfig.push(currentProductConfig);
                        }

                        fs.writeFile(path.join(process.cwd(), configs.gulpDir, configs.configFiles.productsConfig), JSON.stringify(productsConfig, null, 4));
                    }
                });
            }
        });
    });
};