/*! fanex-customer-setting-matrix v0.0.1 | (c) 2015 Nexcel Solutions Vietnam */

(function () {
    'use strict';
    var customerSettingMatrixApp = angular.module('customerSettingMatrixApp', []);
    customerSettingMatrixApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$rootScope', '$templateCache', 'fanexBlockUiService',
            function ($q, $rootScope, $templateCache, blockUiService) {
                function startBlockUi(config) {
                    config.noBlock = (config.method === 'GET' && angular.isDefined($templateCache.get(config.url)));
                    if (!config.noBlock) {
                        blockUiService.show();
                    }
                }

                function stopBlockUi(config) {
                    if (!config.noBlock) {
                        blockUiService.hide();
                    }
                }

                return {
                    request: function (config) {
                        startBlockUi(config);
                        return config;
                    },
                    response: function (response) {
                        if (response) {
                            stopBlockUi(response.config);
                        }

                        return response;
                    },
                    responseError: function (response) {
                        if (response) {
                            stopBlockUi(response.config);
                        }

                        $rootScope.showMessage('Sorry, an error occurred while processing your request.');
                        return $q.reject(response);
                    }
                };
            }
        ]);
    } ]);
} ());

/*jshint -W101*/

angular.module('customerSettingMatrixApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/fanex-block-ui.htm',
    "<div class=loading-container data-ng-show=hasLoading><div class=block-overlay></div><div class=block-loading></div></div>"
  );


  $templateCache.put('templates/fanex-breadcrumb.htm',
    "<div class=breadcrumb-container><ol><li data-ng-repeat=\"line in lines\" data-ng-switch=$last data-ng-class=\"{'active': $last}\"><a href=# ng-switch-when=false data-ng-click=selectUserName(line.Name)>{{line.Name}} ({{line.Id}})</a> <span ng-switch-when=true>{{line.Name}} ({{line.Id}})</span></li></ol></div>"
  );


  $templateCache.put('templates/fanex-commission-matrix.htm',
    "<div class=group data-ng-repeat=\"table in tables\"><div data-ng-show=table.Name class=group-name>{{table.Name}}</div><div class=table><table><colgroup><col width=\"80\"><col width=50 data-ng-repeat=\"column in table.TotalOfColumn\"></colgroup><thead><tr><th></th><th data-ng-repeat=\"column in table.Columns\">{{column.Name}}</th></tr></thead><tbody><tr data-ng-repeat=\"row in table.Rows\"><td>{{row.Name}}</td><td data-ng-repeat=\"column in row.Columns\">{{column.Value}}</td></tr></tbody></table></div></div>"
  );


  $templateCache.put('templates/fanex-form.htm',
    "<div class=form-container><div class=controls><label for=product>Product</label><select name=product data-ng-model=product data-ng-options=\"product.Name for product in products\"></select></div><div class=controls><label for=userName>Username</label><input name=userName data-ng-model=\"userName\"></div><div class=controls><button data-ng-click=submit()>Submit</button></div></div>"
  );


  $templateCache.put('templates/fanex-header.htm',
    "<div class=header-container><ul class=navigation><li data-ng-repeat=\"navigationItem in navigationItems\" data-ng-click=activeNavigationItem(navigationItem) data-ng-class=\"{'active': selectedNavigationItem==navigationItem}\"><a href=#>{{navigationItem.Text}}</a></li></ul></div>"
  );


  $templateCache.put('templates/fanex-page.htm',
    "<div data-fanex-header></div><div data-fanex-form></div><div class=message-container data-ng-show=message data-ng-bind=message></div><div data-ng-show=customer data-fanex-breadcrumb></div><div class=result-container data-ng-show=settings data-ng-switch=selectedNavigationItem.Value><div data-ng-switch-when=0 data-fanex-position-taking-matrix></div><div data-ng-switch-when=1 data-fanex-commission-matrix></div></div>"
  );


  $templateCache.put('templates/fanex-position-taking-matrix.htm',
    "<div class=group data-ng-repeat=\"group in groups\"><div class=group-name>{{group.Name}}</div><div class=table data-ng-repeat=\"table in group.Tables\"><div class=table-name>{{table.Name}}</div><table><colgroup><col width=\"80\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"></colgroup><thead><tr><th></th><th>Super</th><th>Master</th><th>Agent</th><th>US</th><th>IS</th><th>UM</th><th>IM</th><th>N</th></tr></thead><tbody><tr data-ng-repeat=\"row in table.Rows\"><td>{{row.RoleId | fanexRoleFormat}}</td><td>{{row.SuperPT | fanexPtFormat}}</td><td>{{row.MasterPT | fanexPtFormat}}</td><td>{{row.AgentPT | fanexPtFormat}}</td><td>{{row.AutoSuperPT | fanexPtFormat}}</td><td data-ng-switch=\"row.IsAutoSuperPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoSuperPT disabled></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\"><div data-ng-switch-when=true><div class=icon-auto-pt></div></div><div data-ng-switch-when=false>{{row.AutoMasterPT | fanexPtFormat}}</div></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\" data-ng-class=\"{'auto-pt': row.RoleId==3 && !disableAutoMPT}\"><div data-ng-switch-when=false><div data-ng-switch=\"row.IsAutoMasterPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoMasterPT disabled></div></div></td><td>{{row.MinPT | fanexPtFormat}}</td></tr></tbody></table></div></div>"
  );

} ]);

/*jshint -W101*/

(function () {
    'use strict';
    var injectParams = ['$scope', '$http', 'fanexAppSettings'];
    var fanexPageController = function ($scope, $http, appSettings) {
        $scope.pageUrl = 'templates/fanex-page.htm';
        $scope.changeSettingType = function (settingType) {
            $scope.settingType = settingType;
            $scope.message = undefined;
            $scope.customer = undefined;
            $scope.settings = undefined;
        };
        $scope.productId = 0;
        $scope.userName = '428RM';
        $scope.submitForm = function (productId, userName) {
            $scope.productId = productId;
            $scope.userName = userName;
            var url = appSettings.rootUrl + '/CustomerSettingMatrix/Query';
            var queryModel = {
                ProductId: productId,
                UserName: userName,
                SettingType: $scope.settingType
            };
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                $scope.customer = response.Customer;
                $scope.settings = response.Settings;
            });
        };
        $scope.selectUserName = function (userName) {
            $scope.userName = userName;
            $scope.submitForm($scope.productId, $scope.userName);
        };
        $scope.showMessage = function (message) {
            $scope.customer = undefined;
            $scope.settings = undefined;
            $scope.message = message;
        };
        $scope.hideMessage = function () {
            $scope.message = undefined;
        };
    };

    fanexPageController.$inject = injectParams;
    angular.module('customerSettingMatrixApp').controller('FanexPageController', fanexPageController);
})();

(function () {
    'use strict';
    var injectParams = ['$compile', '$templateCache', 'fanexBlockUiService'];
    var fanexBlockUiDirective = function ($compile, $templateCache, blockUiService) {
        return {
            restrict: 'A',
            compile: function () {
                return function (scope, element) {
                    var template = $templateCache.get('templates/fanex-block-ui.htm');
                    var clone = angular.element(template);
                    element.append(clone);
                    var newScope = scope.$new();
                    var link = $compile(clone);
                    link(newScope);
                    blockUiService.setup(newScope);
                };
            }
        };
    };

    fanexBlockUiDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexBlockUi', fanexBlockUiDirective);
})();

(function () {
    'use strict';
    var injectParams = ['fanexAppSettings'];
    var fanexBreadcrumbDirective = function (appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: false,
            templateUrl: 'templates/fanex-breadcrumb.htm',
            controller: ['$scope', function ($scope) {
                $scope.$watch('customer', function (newCustomer, oldCustomer) {
                    if (newCustomer !== oldCustomer) {
                        render();
                    }
                });

                function render() {
                    var customer = $scope.customer;
                    if (angular.isUndefined(customer)) {
                        $scope.lines = undefined;
                        return;
                    }

                    var lines = [];
                    switch (customer.RoleId) {
                        case appSettings.roleType.Super:
                            break;

                        case appSettings.roleType.Master:
                            lines.push({ Id: customer.SuperId, Name: customer.SuperName });
                            break;

                        case appSettings.roleType.Agent:
                            lines.push({ Id: customer.SuperId, Name: customer.SuperName });
                            lines.push({ Id: customer.MasterId, Name: customer.MasterName });
                            break;

                        case appSettings.roleType.Member:
                            lines.push({ Id: customer.SuperId, Name: customer.SuperName });
                            lines.push({ Id: customer.MasterId, Name: customer.MasterName });
                            lines.push({ Id: customer.AgentId, Name: customer.AgentName });
                            break;

                        default:
                            throw 'Not implemented yet.';
                    }

                    lines.push({ Id: customer.CustId, Name: customer.UserName });
                    $scope.lines = lines;
                }
            } ]
        };
    };

    fanexBreadcrumbDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexBreadcrumb', fanexBreadcrumbDirective);
})();

(function () {
    'use strict';
    var injectParams = ['$compile', 'fanexAppSettings', 'fanexCommissionHandler'];
    var fanexCommissionMatrixDirective = function ($compile, appSettings, CommissionHandler) {
        return {
            restrict: 'AE',
            scope: true,
            templateUrl: 'templates/fanex-commission-matrix.htm',
            controller: ['$scope', function ($scope) {
                $scope.$watch('settings', function (newSettings, oldSettings) {
                    if (newSettings !== oldSettings) {
                        update();
                    }
                });

                function update() {
                    var customer = $scope.customer;
                    var settings = $scope.settings;
                    var roleId = customer.RoleId;
                    var settingLevel = customer.SettingLevel;
                    var commissionHandler = new CommissionHandler(roleId, settingLevel);
                    $scope.tables = commissionHandler.createTables(settings);
                }
            } ]
        };
    };

    fanexCommissionMatrixDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexCommissionMatrix', fanexCommissionMatrixDirective);
})();

(function () {
    'use strict';
    var injectParams = ['$http', 'fanexAppSettings', 'fanexResourceService'];
    var fanexFormDirective = function ($http, appSettings, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: false,
            templateUrl: 'templates/fanex-form.htm',
            controller: ['$scope', function ($scope) {
                $scope.products = [
                    { Id: 250, Name: 'Product-250' },
                    { Id: 251, Name: 'Product-251' },
                    { Id: 252, Name: 'Product-252' },
                    { Id: 253, Name: 'Product-253' },
                    { Id: 254, Name: 'Product-254' },
                    { Id: 255, Name: 'Product-255' }
                ];
                $scope.product = $scope.products[0];
                $scope.submit = function () {
                    var product = $scope.product;
                    var productId = product.Id;
                    var productName = product.Name;
                    resourceService.ProductName = productName;
                    var userName = $scope.userName;

                    if (angular.isUndefined(productId)) {
                        $scope.showMessage('Please select a product!');
                        return;
                    }

                    if (angular.isUndefined(userName) || userName === '') {
                        $scope.showMessage('Please input username!');
                        return;
                    }

                    $scope.hideMessage();
                    $scope.submitForm(productId, userName);
                };
            } ]
        };
    };

    fanexFormDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexForm', fanexFormDirective);
})();

(function () {
    'use strict';
    var injectParams = ['fanexAppSettings'];
    var fanexHeaderDirective = function (appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: false,
            templateUrl: 'templates/fanex-header.htm',
            controller: ['$scope', function ($scope) {
                $scope.navigationItems = [{
                    Value: appSettings.settingType.PositionTaking,
                    Text: 'PT Matrix'
                }, {
                    Value: appSettings.settingType.Commission,
                    Text: 'Commission Matrix'
                }];
                $scope.activeNavigationItem = function (navigationItem) {
                    $scope.selectedNavigationItem = navigationItem;
                    $scope.changeSettingType(navigationItem.Value);
                };
                $scope.activeNavigationItem($scope.navigationItems[0]);
            } ]
        };
    };

    fanexHeaderDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexHeader', fanexHeaderDirective);
})();

(function () {
    'use strict';
    var injectParams = ['$compile', 'fanexAppSettings', 'fanexPositionTakingHandler'];
    var fanexPositionTakingMatrixDirective = function ($compile, appSettings, PositionTakingHandler) {
        return {
            restrict: 'AE',
            scope: true,
            templateUrl: 'templates/fanex-position-taking-matrix.htm',
            controller: ['$scope', function ($scope) {
                $scope.$watch('settings', function (newSettings, oldSettings) {
                    if (newSettings !== oldSettings) {
                        update();
                    }
                });

                function update() {
                    var customer = $scope.customer;
                    $scope.disableAutoMPT = customer.DisableAutoMPT;
                    var settings = $scope.settings;
                    var roleId = customer.RoleId;
                    var settingLevel = customer.SettingLevel;
                    var positionTakingHandler = new PositionTakingHandler(roleId, settingLevel);
                    $scope.groups = positionTakingHandler.createGroups(settings);
                }
            } ]
        };
    };

    fanexPositionTakingMatrixDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexPositionTakingMatrix', fanexPositionTakingMatrixDirective);
})();

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

(function () {
    'use strict';
    var injectParams = ['$filter'];
    var fanexPtFormat = function ($filter) {
        return function (value) {
            if (angular.isNumber(value)) {
                return $filter('number')(value / 100, 2);
            }

            return undefined;
        };
    };

    fanexPtFormat.$inject = injectParams;
    angular.module('customerSettingMatrixApp').filter('fanexPtFormat', fanexPtFormat);
})();

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

(function () {
    'use strict';
    var injectParams = [];
    var fanexBlockUiService = function () {
        var blockUiScope;

        function setup(scope) {
            if (angular.isDefined(blockUiScope)) {
                throw 'Only one block ui component in document';
            }

            blockUiScope = scope;
            blockUiScope.hasLoading = false;
        }

        function show() {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = true;
        }

        function hide() {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = false;
        }

        return {
            setup: setup,
            show: show,
            hide: hide
        };
    };

    fanexBlockUiService.$inject = injectParams;
    angular.module('customerSettingMatrixApp').service('fanexBlockUiService', fanexBlockUiService);
})();

(function () {
    'use strict';
    var injectParams = [];
    var fanexCommonService = function () {
        function group(array, condition) {
            var results = [];
            var currentItem;
            var items = [];
            angular.forEach(array, function (item) {
                if (angular.isDefined(currentItem) && !condition(currentItem, item)) {
                    results.push(items);
                    items = [];
                }

                items.push(item);
                currentItem = item;
            });

            results.push(items);
            return results;
        }

        return {
            group: group
        };
    };

    fanexCommonService.$inject = injectParams;
    angular.module('customerSettingMatrixApp').service('fanexCommonService', fanexCommonService);
})();

(function () {
    'use strict';
    var injectParams = [];
    var fanexResourceService = function () {
        var productName;
        return {
            ProductName: productName
        };
    };

    fanexResourceService.$inject = injectParams;
    angular.module('customerSettingMatrixApp').service('fanexResourceService', fanexResourceService);
})();

(function () {
    'use strict';
    var body = angular.element(document.body);
    var rootUrl = body.attr('data-root-url');
    angular.module('customerSettingMatrixApp').value('fanexAppSettings', {
        rootUrl: rootUrl,
        roleType: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4
        },
        settingLevel: {
            Product: 1,
            SportType: 2,
            BetType: 3,
            SportTypeBetType: 4
        },
        settingType: {
            PositionTaking: 0,
            Commission: 1
        }
    });
})();