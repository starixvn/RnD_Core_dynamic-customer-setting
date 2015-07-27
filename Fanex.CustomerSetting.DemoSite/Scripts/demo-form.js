/// <reference path="typings/angularjs/angular.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var DemoFrom;
(function (DemoFrom) {
    var app = angular.module("actorForm", []);
    var DemoFormController = (function () {
        function DemoFormController($http, $location) {
            this.$http = $http;
            this.$location = $location;
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
            this.actor = SampleDataFactory.getActor(selectedRole);
            this.target = undefined;
            this.targetRoles = Target.getAvailableRoles(selectedRole);
            this.isMultipleUpdates = false;
            this.getTarget();
            this.switchForm(false);
        };
        DemoFormController.prototype.getTarget = function () {
            var selectedRole = angular.isDefined(this.target) ? this.target.role : this.targetRoles[0].key;
            if (this.isMultipleUpdates) {
                this.target = SampleDataFactory.getMultipleTarget(selectedRole);
            }
            else {
                this.target = SampleDataFactory.getSingleTarget(selectedRole);
            }
        };
        DemoFormController.prototype.saveSession = function () {
            var _this = this;
            this.$http.post("home/saveSession", this.actor).success(function () {
                _this.switchForm(true);
            });
        };
        DemoFormController.prototype.showSetting = function () {
            var params = {
                language: this.actor.language,
                productId: this.target.productId,
                customerLevel: this.target.role
            };
            if (this.actor.role !== Role.company) {
                params.uplineLevel = this.actor.role;
            }
            if (this.isMultipleUpdates) {
                params.isMultiple = true;
                params.customerIds = this.target.customerIds;
                params.customerNames = this.target.customerNames;
            }
            else {
                params.customerId = this.target.customerId;
            }
            this.settingPath = this.$location.path("ViewCustomerSetting").search(params).url().replace("?", "/#?");
            document.getElementById("setting-page").contentWindow.location.reload();
        };
        DemoFormController.$inject = ["$http", "$location"];
        return DemoFormController;
    })();
    DemoFrom.DemoFormController = DemoFormController;
    app.controller("DemoFormController", DemoFormController);
    var SampleDataFactory = (function () {
        function SampleDataFactory() {
        }
        SampleDataFactory.getLanuages = function () {
            return new Array(new KeyValuePair("en-US", "English (en-US)"), new KeyValuePair("zh-TW", "繁體中文 (zh-TW)"), new KeyValuePair("zh-CN", "简体中文 (zh-CN)"), new KeyValuePair("ja-JP", "日本語 (ja-JP)"), new KeyValuePair("ko-KR", "ภาษาไทย (th-TH)"), new KeyValuePair("zh-TW", "한국어 (ko-KR)"), new KeyValuePair("vi-VN", "Tiếng Việt (vi-VN)"));
        };
        SampleDataFactory.getActor = function (selectedRole) {
            var actorId = 0;
            var name = "";
            var subAccId = 0;
            switch (selectedRole) {
                case Role.company:
                    actorId = 8;
                    name = "sa";
                    subAccId = 111;
                    break;
                case Role.super:
                    actorId = 2;
                    name = "SA-RM";
                    subAccId = 3;
                    break;
                case Role.master:
                    actorId = 4174;
                    name = "wsrm";
                    subAccId = 728497;
                    break;
                case Role.agent:
                    actorId = 1262;
                    name = "428rm";
                    subAccId = 728498;
                    break;
            }
            return this.createActor(actorId, name, selectedRole, subAccId);
        };
        SampleDataFactory.getSingleTarget = function (role) {
            var id = 0;
            switch (role) {
                case Role.super:
                    id = 2;
                    break;
                case Role.master:
                    id = 4174;
                    break;
                case Role.agent:
                    id = 1262;
                    break;
                case Role.member:
                    id = 1277;
                    break;
            }
            return this.createSingleTarget(id, role);
        };
        SampleDataFactory.getMultipleTarget = function (role) {
            var ids = "";
            var names = "";
            switch (role) {
                case Role.super:
                    ids = "2,3,4";
                    names = "SA-RM,SA-US$,SA-TB";
                    break;
                case Role.master:
                    ids = "4174,4300,4356";
                    names = "mrm,mbrm,MYRM";
                    break;
                case Role.agent:
                    ids = "1262,1653,2400";
                    names = "wsrm,SKRM,216COM";
                    break;
                case Role.member:
                    ids = "1277,1284,1290";
                    names = "428RM,305RM,302RM";
                    break;
            }
            return this.createMultipleTargetInfo(ids, names, role);
        };
        SampleDataFactory.createActor = function (id, name, role, subAccId) {
            return new Actor(id, name, role, subAccId, this.defaultLanguage);
        };
        SampleDataFactory.createSingleTarget = function (id, role) {
            return new SingleTarget(id, role, this.defaultProductId);
        };
        SampleDataFactory.createMultipleTargetInfo = function (ids, names, role) {
            return new MultipleTargets(ids, names, role, this.defaultProductId);
        };
        SampleDataFactory.defaultProductId = 255;
        SampleDataFactory.defaultLanguage = "en-US";
        return SampleDataFactory;
    })();
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
    })();
    var Actor = (function () {
        function Actor(actorId, actorName, role, subAccountId, language) {
            this.actorId = actorId;
            this.actorName = actorName;
            this.role = role;
            this.subAccountId = subAccountId;
            this.language = language;
        }
        Actor.getAvailableRoles = function () {
            var roles = new Array();
            for (var item in Role) {
                var roleId = parseInt(item);
                if (!isNaN(roleId) && roleId !== Role.member) {
                    roles.push(new KeyValuePair(roleId, Role[roleId]));
                }
            }
            return roles.sort(function (num1, num2) { return num2.key - num1.key; });
        };
        return Actor;
    })();
    var Target = (function () {
        function Target(productId, role) {
            this.productId = productId;
            this.role = role;
        }
        Target.getAvailableRoles = function (actorRole) {
            var roles = new Array();
            for (var item in Role) {
                var roleId = parseInt(item);
                if (!isNaN(roleId) && roleId < actorRole) {
                    roles.push(new KeyValuePair(roleId, Role[roleId]));
                }
            }
            return roles.sort(function (num1, num2) { return num2.key - num1.key; });
        };
        return Target;
    })();
    var SingleTarget = (function (_super) {
        __extends(SingleTarget, _super);
        function SingleTarget(customerId, role, productId) {
            _super.call(this, productId, role);
            this.customerId = customerId;
        }
        return SingleTarget;
    })(Target);
    var MultipleTargets = (function (_super) {
        __extends(MultipleTargets, _super);
        function MultipleTargets(customerIds, customerNames, role, productId) {
            _super.call(this, productId, role);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }
        MultipleTargets.prototype.getFirstCustomerId = function () {
            var ids = this.customerIds.split(",");
            if (ids !== null && ids.length > 0) {
                return parseInt(ids[0]);
            }
            return 0;
        };
        return MultipleTargets;
    })(Target);
})(DemoFrom || (DemoFrom = {}));
