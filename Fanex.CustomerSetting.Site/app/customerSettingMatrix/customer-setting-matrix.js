/*! fanex-customer-setting-matrix v0.0.1 | (c) 2015 Nexcel Solutions Vietnam */

(function () {
    'use strict';
    angular.module('customerSettingMatrixApp', []);
} ());

/*jshint -W101*/

angular.module('customerSettingMatrixApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/fanex-breadcrumb.htm',
    "<ol><li data-ng-repeat=\"line in lines\" data-ng-switch on=$last data-ng-class=\"{'active': $last}\"><a href=# ng-switch-when=false data-ng-click=show(line.Name)>{{line.Name}} ({{line.Id}})</a> <span ng-switch-when=true>{{line.Name}} ({{line.Id}})</span></li></ol>"
  );


  $templateCache.put('templates/fanex-position-taking-matrix.htm',
    "<div class=group data-ng-repeat=\"group in groups\"><div class=group-name>{{group.Name}}</div><div class=table data-ng-repeat=\"table in group.Tables\"><div class=table-name>{{table.Name}}</div><table><colgroup><col width=\"80\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"></colgroup><thead><tr><th></th><th>Super</th><th>Master</th><th>Agent</th><th>US</th><th>IS</th><th>UM</th><th>IM</th><th>N</th></tr></thead><tbody><tr data-ng-repeat=\"row in table.Rows\"><td>{{row.RoleId | fanexRoleFormat}}</td><td>{{row.SuperPT | fanexPtFormat}}</td><td>{{row.MasterPT | fanexPtFormat}}</td><td>{{row.AgentPT | fanexPtFormat}}</td><td>{{row.AutoSuperPT | fanexPtFormat}}</td><td data-ng-switch=\"row.IsAutoSuperPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoSuperPT disabled></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\"><div data-ng-switch-when=true><div class=icon-auto-pt></div></div><div data-ng-switch-when=false>{{row.AutoMasterPT | fanexPtFormat}}</div></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\" data-ng-class=\"{'auto-pt': row.RoleId==3 && !disableAutoMPT}\"><div data-ng-switch-when=false><div data-ng-switch=\"row.IsAutoMasterPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoMasterPT disabled></div></div></td><td>{{row.MinPT | fanexPtFormat}}</td></tr></tbody></table></div></div>"
  );

} ]);

/*jshint -W101*/

(function () {
    'use strict';
    var injectParams = ['$scope', '$http', 'fanexAppSettings', 'fanexResourceService'];
    var fanexPositionTakingController = function ($scope, $http, appSettings, resourceService) {
        $scope.products = [
            { Id: 250, Name: 'Product-250' },
            { Id: 251, Name: 'Product-251' },
            { Id: 252, Name: 'Product-252' },
            { Id: 253, Name: 'Product-253' },
            { Id: 254, Name: 'Product-254' },
            { Id: 255, Name: 'Product-255' }
        ];
        $scope.product = $scope.products[0];
        $scope.userName = '428RM';
        $scope.submit = function () {
            var product = $scope.product;
            var productId = product.Id;
            var productName = product.Name;
            resourceService.ProductName = productName;
            if (angular.isUndefined(productId)) {
                showMessage('Please select a product!');
                return;
            }

            var userName = $scope.userName;
            if (angular.isUndefined(userName) || userName === '') {
                showMessage('Please input username!');
                return;
            }

            hideMessage();
            submit(productId, userName);
        };

        $scope.$on('fanex-user-clicked', function (event, userName) {
            $scope.userName = userName;
            $scope.submit();
        });

        function showMessage(message) {
            $scope.customer = undefined;
            $scope.settings = undefined;
            $scope.message = message;
        }

        function hideMessage() {
            $scope.message = undefined;
        }

        function submit(productId, userName) {
            var queryModel = {
                ProductId: productId,
                UserName: userName
            };

            var url = appSettings.rootUrl + '/CustomerSettingMatrix/PositionTaking';
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                $scope.customer = response.Customer;
                $scope.settings = response.Settings;
            });
        }
    };

    fanexPositionTakingController.$inject = injectParams;
    angular.module('customerSettingMatrixApp').controller('FanexPositionTakingController', fanexPositionTakingController);
})();

(function () {
    'use strict';
    var injectParams = ['fanexAppSettings'];
    var fanexBreadcrumbDirective = function (appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: true,
            templateUrl: 'templates/fanex-breadcrumb.htm',
            controller: ['$scope', function ($scope) {
                $scope.$watch('customer', function (newCustomer, oldCustomer) {
                    if (newCustomer !== oldCustomer) {
                        render();
                    }
                });

                function render() {
                    var lines = [];
                    var customer = $scope.customer;
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

                $scope.show = function (userName) {
                    $scope.$emit('fanex-user-clicked', userName);
                };
            } ]
        };
    };

    fanexBreadcrumbDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexBreadcrumb', fanexBreadcrumbDirective);
})();

(function () {
    'use strict';
    var injectParams = ['$compile', 'fanexAppSettings', 'fanexSettingLevelHandler', 'fanexRoleTypeHandler'];
    var fanexPositionTakingMatrixDirective = function ($compile, appSettings, SettingLevelHandler, RoleTypeHandler) {
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
                    var settings = $scope.settings;
                    var settingLevel = customer.SettingLevel;
                    var settingLevelHandler = new SettingLevelHandler(settingLevel);
                    var roleId = customer.RoleId;
                    var roleTypeHandler = new RoleTypeHandler(roleId);
                    var settingTables = roleTypeHandler.generateTables(settingLevel, settings);
                    var settingGroups = settingLevelHandler.generateGroups(settingTables);
                    $scope.disableAutoMPT = customer.DisableAutoMPT;
                    $scope.groups = settingGroups;
                }
            } ]
        };
    };

    fanexPositionTakingMatrixDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexPositionTakingMatrix', fanexPositionTakingMatrixDirective);
})();

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

(function () {
    'use strict';
    var injectParams = ['fanexAppSettings', 'fanexResourceService'];
    var fanexSettingLevelHandler = function (appSettings, resourceService) {
        function SettingGroup(name) {
            this.Name = name;
            this.Tables = [];
        }

        var ProductHandler = function () {
            this.getTableName = function (row) {
                return row.IsLive ? ' Live' : 'Non-Live';
            };
            this.generateGroups = function (tables) {
                var groups = [];
                var name = resourceService.ProductName;
                var group = new SettingGroup(name);
                group.Tables = tables;
                groups.push(group);
                return groups;
            };
        };

        var SportTypeHandler = function () {
            this.getTableName = function (row) {
                return row.IsLive ? ' Live' : 'Non-Live';
            };
            this.generateGroups = function (tables) {
                var sportType = -1;
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

        var BetTypeHandler = function () {
            this.getTableName = function (row) {
                return row.IsLive ? ' Live' : 'Non-Live';
            };
            this.generateGroups = function (tables) {
                var betType = -1;
                var groups = [];
                var group;
                angular.forEach(tables, function (table) {
                    var row = table.Rows[0];
                    if (row.BetType !== betType) {
                        betType = row.BetType;
                        var name = row.BetTypeName;
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
                if(row.IsLive){
                    name += ' (Live)';
                }
                return name;
            };
            this.generateGroups = function (tables) {
                var sportType = -1;
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
        }
    });
})();