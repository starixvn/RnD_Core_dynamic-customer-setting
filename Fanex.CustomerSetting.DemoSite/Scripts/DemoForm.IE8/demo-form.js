var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var DemoForm;
(function (DemoForm) {
    "use strict";
    var app = angular.module("actorForm", []);
    var DemoFormController = (function () {
        function DemoFormController($http, $location, $window, postingToIframeService) {
            this.$http = $http;
            this.$location = $location;
            this.$window = $window;
            this.applicationPath = this.$window["applicationPath"];
            if (this.applicationPath && this.applicationPath[this.applicationPath.length - 1] !== "/") {
                this.applicationPath += "/";
            }
            this.postingToIframeService = postingToIframeService;
            this.systemTypes = SampleDataFactory.getSystemTypes();
            this.actorRoles = Actor.getAvailableRoles();
            this.languages = SampleDataFactory.getLanuages();
            this.changeActorRole();
        }
        DemoFormController.prototype.switchForm = function (state) {
            this.preventSavingSession = state;
            this.preventShowingData = !state;
        };
        DemoFormController.prototype.changeActorRole = function () {
            var selectedRole = angular.isDefined(this.actor) ? this.actor.role : Role.company;
            var selectedSystemId = angular.isDefined(this.actor) ? this.actor.system : this.systemTypes[0].key;
            this.actor = SampleDataFactory.getActor(selectedRole, selectedSystemId);
            this.target = undefined;
            this.targetRoles = Target.getAvailableRoles(selectedRole);
            this.isMultipleUpdates = false;
            this.getTarget(selectedRole);
            this.switchForm(false);
        };
        DemoFormController.prototype.getTarget = function (actorRole) {
            var selectedRole = angular.isDefined(this.target) ? this.target.role : this.targetRoles[0].key;
            var uplineRole = actorRole;
            this.uplineRoles = undefined;
            if (selectedRole === Role.member) {
                if (actorRole === Role.company) {
                    this.uplineRoles = Target.getUplineRoles(selectedRole);
                    uplineRole = this.uplineRoles[0].key;
                }
                else {
                    uplineRole = actorRole;
                }
            }
            else {
                uplineRole = undefined;
            }
            if (this.isMultipleUpdates) {
                this.target = SampleDataFactory.getMultipleTarget(selectedRole, uplineRole);
            }
            else {
                this.target = SampleDataFactory.getSingleTarget(selectedRole, uplineRole);
            }
        };
        DemoFormController.prototype.saveSession = function () {
            var _this = this;
            this.$http.post(this.resolveUrl("home/saveSession"), this.actor).then(function () {
                _this.switchForm(true);
            });
        };
        DemoFormController.prototype.showSetting = function () {
            var params = {
                language: this.actor.language,
                productId: this.target.productId,
                customerLevel: this.target.role,
                uplineLevel: this.target.uplineRole,
                path: this.resolveUrl("ViewCustomerSetting"),
                frameName: "setting-page"
            };
            if (this.isMultipleUpdates) {
                params.isMultiple = true;
                params.customerIds = this.target.customerIds;
                params.customerNames = this.target.customerNames;
            }
            else {
                params.isMultiple = false;
                params.customerId = this.target.customerId;
            }
            this.postingToIframeService.submitToIframe(params);
        };
        DemoFormController.prototype.resolveUrl = function (requestUrl) {
            return this.applicationPath + requestUrl;
        };
        DemoFormController.$inject = ["$http", "$location", "$window", "postingToIframeService"];
        return DemoFormController;
    }());
    app.controller("DemoFormController", DemoFormController);
    var PostingToIframeDirective = (function () {
        function PostingToIframeDirective(postingToIframeService) {
            var _this = this;
            this.replace = true;
            this.templateUrl = "posting-to-iframe.htm";
            this.link = function (scope, element) {
                var form = element[0];
                _this.postingToIframeService.setup(scope);
                scope.submitToIframe = function () {
                    form.submit();
                    return false;
                };
            };
            this.postingToIframeService = postingToIframeService;
        }
        return PostingToIframeDirective;
    }());
    app.directive("postingToIframe", ["postingToIframeService", function () {
            var args = Array.prototype.concat.apply([null], arguments);
            return new (Function.prototype.bind.apply(PostingToIframeDirective, args));
        }]);
    var PostingToIframeService = (function () {
        function PostingToIframeService($timeout) {
            this.$timeout = $timeout;
        }
        PostingToIframeService.prototype.setup = function (scope) {
            this.scope = scope;
        };
        PostingToIframeService.prototype.submitToIframe = function (params) {
            var _this = this;
            _.chain(params).keys().forEach(function (prop) {
                _this.scope[prop] = params[prop];
            });
            this.$timeout(function () {
                _this.scope.submitToIframe();
            });
        };
        PostingToIframeService.$inject = ["$timeout"];
        return PostingToIframeService;
    }());
    app.service("postingToIframeService", PostingToIframeService);
    var SampleDataFactory = (function () {
        function SampleDataFactory() {
        }
        SampleDataFactory.getSystemTypes = function () {
            return new Array(new KeyValuePair(1, "accounting"), new KeyValuePair(2, "agency"), new KeyValuePair(3, "external accounting"));
        };
        SampleDataFactory.getLanuages = function () {
            return new Array(new KeyValuePair("en-US", "English (en-US)"), new KeyValuePair("zh-TW", "繁體中文 (zh-TW)"), new KeyValuePair("zh-CN", "简体中文 (zh-CN)"), new KeyValuePair("ja-JP", "日本語 (ja-JP)"), new KeyValuePair("th-TH", "ภาษาไทย (th-TH)"), new KeyValuePair("zh-TW", "한국어 (ko-KR)"), new KeyValuePair("vi-VN", "Tiếng Việt (vi-VN)"));
        };
        SampleDataFactory.getActor = function (selectedRole, systemId) {
            var actorId = 0;
            var name = "";
            var subAccId = 0;
            switch (selectedRole) {
                case Role["company"]:
                    actorId = 8;
                    name = "sa";
                    subAccId = 111;
                    break;
                case Role["super"]:
                    actorId = 1176070;
                    name = "SuperTest1 ";
                    subAccId = 3;
                    break;
                case Role["master"]:
                    actorId = 1176083;
                    name = "master1";
                    subAccId = 728497;
                    break;
                case Role["agent"]:
                    actorId = 1176086;
                    name = "agent01";
                    subAccId = 728498;
                    break;
            }
            return this.createActor(actorId, name, selectedRole, subAccId, systemId);
        };
        SampleDataFactory.getSingleTarget = function (role, uplineRole) {
            var id = 0;
            switch (role) {
                case Role["super"]:
                    id = 1176070;
                    name = "SuperTest1";
                    break;
                case Role["master"]:
                    id = 1176083;
                    name = "master1";
                    break;
                case Role["agent"]:
                    id = 1176086;
                    name = "agent01";
                    break;
                case Role["member"]:
                    id = 7890346;
                    name = "AGENT01000";
                    break;
            }
            return this.createSingleTarget(id, name, role, uplineRole);
        };
        SampleDataFactory.getMultipleTarget = function (role, uplineRole) {
            var ids = "";
            var names = "";
            switch (role) {
                case Role["super"]:
                    ids = "1176070,576313,3230178";
                    names = "SuperTest1,SuperTest2,SuperTest12";
                    break;
                case Role["master"]:
                    ids = "1176083,14167350,14373487";
                    names = "master1,mastertest01,masterkz";
                    break;
                case Role["agent"]:
                    ids = "1176086,14373905,14374140";
                    names = "agent01,agent03,agentlcs000";
                    break;
                case Role["member"]:
                    ids = "7890346,8252951,8252986";
                    names = "AGENT01000,AGENT01001,AGENT01002";
                    break;
            }
            return this.createMultipleTargetInfo(ids, names, role, uplineRole);
        };
        SampleDataFactory.createActor = function (id, name, role, subAccId, systemId) {
            return new Actor(id, name, role, subAccId, this.defaultLanguage, systemId);
        };
        SampleDataFactory.createSingleTarget = function (id, name, role, uplineRole) {
            return new SingleTarget(id, name, role, uplineRole, this.defaultProductId);
        };
        SampleDataFactory.createMultipleTargetInfo = function (ids, names, role, uplineRole) {
            return new MultipleTargets(ids, names, role, uplineRole, this.defaultProductId);
        };
        SampleDataFactory.defaultProductId = 23;
        SampleDataFactory.defaultLanguage = "en-US";
        return SampleDataFactory;
    }());
    var Role;
    (function (Role) {
        Role[Role["company"] = 5] = "company";
        Role[Role["super"] = 4] = "super";
        Role[Role["master"] = 3] = "master";
        Role[Role["agent"] = 2] = "agent";
        Role[Role["member"] = 1] = "member";
    })(Role || (Role = {}));
    var KeyValuePair = (function () {
        function KeyValuePair(key, value) {
            this.key = key;
            this.value = value;
        }
        return KeyValuePair;
    }());
    var Actor = (function () {
        function Actor(actorId, actorName, role, subAccountId, language, system) {
            this.actorId = actorId;
            this.actorName = actorName;
            this.role = role;
            this.subAccountId = subAccountId;
            this.language = language;
            this.system = system;
        }
        Actor.getAvailableRoles = function () {
            var roles = new Array();
            for (var item in Role) {
                if (item) {
                    var roleId = parseInt(item, 10);
                    if (roleId !== Role.member) {
                        roles.push(new KeyValuePair(roleId, Role[roleId]));
                    }
                }
            }
            return roles;
        };
        return Actor;
    }());
    var Target = (function () {
        function Target(productId, role, uplineRole) {
            this.productId = productId;
            this.role = role;
            this.uplineRole = uplineRole;
        }
        Target.getAvailableRoles = function (uplineRole) {
            var roles = new Array();
            for (var item in Role) {
                if (item) {
                    var roleId = parseInt(item, 10);
                    if (roleId < uplineRole) {
                        roles.push(new KeyValuePair(roleId, Role[roleId]));
                    }
                }
            }
            return roles;
        };
        Target.getUplineRoles = function (downlineRole) {
            var roles = new Array();
            for (var item in Role) {
                if (item) {
                    var roleId = parseInt(item, 10);
                    if (downlineRole < roleId && roleId < Role.company) {
                        roles.push(new KeyValuePair(roleId, Role[roleId]));
                    }
                }
            }
            return roles;
        };
        return Target;
    }());
    var SingleTarget = (function (_super) {
        __extends(SingleTarget, _super);
        function SingleTarget(customerId, customerName, role, uplineRole, productId) {
            _super.call(this, productId, role, uplineRole);
            this.customerId = customerId;
            this.customerName = customerName;
        }
        return SingleTarget;
    }(Target));
    var MultipleTargets = (function (_super) {
        __extends(MultipleTargets, _super);
        function MultipleTargets(customerIds, customerNames, role, uplineRole, productId) {
            _super.call(this, productId, role, uplineRole);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }
        return MultipleTargets;
    }(Target));
})(DemoForm || (DemoForm = {}));

angular.module("actorForm").run(["$templateCache", function($templateCache) {$templateCache.put("posting-to-iframe.htm","<form action=\"{{path}}\" target=\"{{frameName}}\" method=\"post\" class=\"hide\">\r\n    <input type=\"hidden\" name=\"language\" value=\"{{language}}\" />\r\n    <input type=\"hidden\" name=\"productId\" value=\"{{productId}}\" />\r\n    <input type=\"hidden\" name=\"customerLevel\" value=\"{{customerLevel}}\" />\r\n    <input type=\"hidden\" name=\"uplineLevel\" value=\"{{uplineLevel}}\" data-ng-if=\"uplineLevel\" />\r\n    <input type=\"hidden\" name=\"customerId\" value=\"{{customerId}}\" data-ng-if=\"!isMultiple\" />\r\n    <input type=\"hidden\" name=\"isMultiple\" value=\"{{isMultiple}}\" data-ng-if=\"isMultiple\" />\r\n    <input type=\"hidden\" name=\"customerIds\" value=\"{{customerIds}}\" data-ng-if=\"isMultiple\" />\r\n    <input type=\"hidden\" name=\"customerNames\" value=\"{{customerNames}}\" data-ng-if=\"isMultiple\" />\r\n</form>");}]);