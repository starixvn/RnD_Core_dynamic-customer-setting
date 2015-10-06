"use strict";

module.exports = function (configs) {
    var gulp = require("gulp");
    var fs = require("fs");
    var mssql = require("mssql");
    var path = require("path");

    gulp.task("product:configs:query", function (callback) {
        var config = {
            user: "dbateam",
            password: "@bigboy#",
            server: "10.18.200.230\\SQLSERVER2008R2",
            database: "CustDB"
        };

        var connection = new mssql.Connection(config, function (error) {
            var request = new mssql.Request(connection);
            var query = "SELECT DISTINCT [ProductId], [SettingType] FROM [CustDB].[dbo].[ProductSettingDetail] WHERE [SettingStatus] != 2";

            if (error) {
                console.log(error);
            } else if (!request) {
                console.log("Can't create request to", connection);
            } else {
                request.query(query, function (error, recordSets) {
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

                            currentProductConfig.settingTypes.push(productProfile.SettingType);
                        });

                        if (currentProductConfig.productId) {
                            productsConfig.push(currentProductConfig);
                        }

                        fs.writeFile(
                            path.join(process.cwd(), configs.gulpDir, configs.configFiles.productsConfig),
                            JSON.stringify(productsConfig, null, 4),
                            callback);
                    }
                });
            }
        });
    });
};