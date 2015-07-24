var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="typings/angularjs/angular.d.ts" />
var ActorForm;
(function (ActorForm) {
    var RoleType;
    (function (RoleType) {
        RoleType[RoleType["company"] = 5] = "company";
        RoleType[RoleType["super"] = 4] = "super";
        RoleType[RoleType["master"] = 3] = "master";
        RoleType[RoleType["agent"] = 2] = "agent";
        RoleType[RoleType["member"] = 1] = "member";
    })(RoleType || (RoleType = {}));
    var Language = (function () {
        function Language(name, code) {
            this.name = name;
            this.code = code;
        }
        return Language;
    })();
    var Role = (function () {
        function Role(roleName, roleId) {
            this.roleName = roleName;
            this.roleId = roleId;
        }
        return Role;
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
            for (var role in RoleType) {
                if (!isNaN(role) && parseInt(role) !== RoleType.member) {
                    roles.push(new Role(RoleType[role], parseInt(role)));
                }
            }
            return roles.sort(function (num1, num2) { return num2.roleId - num1.roleId; });
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
            for (var role in RoleType) {
                if (!isNaN(role) && parseInt(role) < actorRole) {
                    roles.push(new Role(RoleType[role], parseInt(role)));
                }
            }
            return roles.sort(function (num1, num2) { return num2.roleId - num1.roleId; });
        };
        return Target;
    })();
    var SingleTarget = (function (_super) {
        __extends(SingleTarget, _super);
        function SingleTarget(customerId, role, productId) {
            _super.call(this, productId, role);
            this.customerId = customerId;
            this.role = role;
            this.productId = productId;
        }
        return SingleTarget;
    })(Target);
    var MultipleTargets = (function (_super) {
        __extends(MultipleTargets, _super);
        function MultipleTargets(customerIds, customerNames, role, productId) {
            _super.call(this, productId, role);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
            this.role = role;
            this.productId = productId;
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
    var SampleDataFactory = (function () {
        function SampleDataFactory() {
        }
        SampleDataFactory.getLanuages = function () {
            return new Array(new Language("English (en-US)", "en-US"), new Language("繁體中文 (zh-TW)", "zh-TW"), new Language("简体中文 (zh-CN)", "zh-CN"), new Language("日本語 (ja-JP)", "ja-JP"), new Language("ภาษาไทย (th-TH)", "ko-KR"), new Language("한국어 (ko-KR)", "zh-TW"), new Language("Tiếng Việt (vi-VN)", "vi-VN"));
        };
        SampleDataFactory.getSampleActor = function (selectedRole) {
            var actorId = 0;
            var name = "";
            var subAccId = 0;
            switch (selectedRole) {
                case RoleType.company:
                    actorId = 8;
                    name = "sa";
                    subAccId = 111;
                    break;
                case RoleType.super:
                    actorId = 2;
                    name = "SA-RM";
                    subAccId = 3;
                    break;
                case RoleType.master:
                    actorId = 4174;
                    name = "wsrm";
                    subAccId = 728497;
                    break;
                case RoleType.agent:
                    actorId = 1262;
                    name = "428rm";
                    subAccId = 728498;
                    break;
            }
            return this.createActor(actorId, name, selectedRole, subAccId);
        };
        SampleDataFactory.getSampleSingleTarget = function (role) {
            var id = 0;
            switch (role) {
                case RoleType.super:
                    id = 2;
                    break;
                case RoleType.master:
                    id = 4174;
                    break;
                case RoleType.agent:
                    id = 1262;
                    break;
                case RoleType.member:
                    id = 1277;
                    break;
            }
            return this.createSingleTarget(id, role);
        };
        SampleDataFactory.getSampleMultipleTarget = function (role) {
            var ids = "";
            var names = "";
            switch (role) {
                case RoleType.company:
                    ids = "2,3,4";
                    names = "SA-RM,SA-US$,SA-TB";
                    break;
                case RoleType.super:
                    ids = "4174,4300,4356";
                    names = "mrm,mbrm,MYRM";
                    break;
                case RoleType.master:
                    ids = "1262,1653,2400";
                    names = "wsrm,SKRM,216COM";
                    break;
                case RoleType.agent:
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
    var ActorController = (function () {
        function ActorController($http) {
            this.$http = $http;
            this.actorRoles = Actor.getAvailableRoles();
            this.languages = SampleDataFactory.getLanuages();
            this.isMultiple = false;
            this.changeActorRole();
        }
        ActorController.prototype.changeActorRole = function () {
            var selectedRoleId = this.actor !== undefined ? this.actor.role : RoleType.company;
            this.actor = SampleDataFactory.getSampleActor(selectedRoleId);
            this.targetRoles = Target.getAvailableRoles(selectedRoleId);
            this.isMultiple = false;
            this.target = undefined;
            this.changeTargetRole();
            this.enableSavingSession();
            this.disableShowingData();
        };
        ActorController.prototype.disableSavingSession = function () {
            this.preventSavingSession = true;
        };
        ActorController.prototype.enableSavingSession = function () {
            this.preventSavingSession = false;
        };
        ActorController.prototype.disableShowingData = function () {
            this.preventShowingData = true;
        };
        ActorController.prototype.enableShowingData = function () {
            this.preventShowingData = false;
        };
        ActorController.prototype.changeTargetRole = function () {
            var selectedRoleId = this.target !== undefined ? this.target.role : this.targetRoles[0].roleId;
            if (this.isMultiple) {
                this.target = SampleDataFactory.getSampleMultipleTarget(selectedRoleId);
            }
            else {
                this.target = SampleDataFactory.getSampleSingleTarget(selectedRoleId);
            }
        };
        ActorController.prototype.saveSession = function () {
            var _this = this;
            this.$http.post("home/saveSession", this.actor).success(function () {
                _this.disableSavingSession();
                _this.enableShowingData();
            });
        };
        ActorController.prototype.showSetting = function () {
            var settingPath = "ViewCustomerSetting#index?productId=" + this.target.productId + "&customerLevel=" + this.target.role;
            if (this.actor.role !== RoleType.company) {
                settingPath += "&uplineLevel=" + this.actor.role;
            }
            if (this.isMultiple) {
                settingPath += "&customerIds='" + this.target.customerIds + "'";
                settingPath += "&customerNames='" + this.target.customerNames + "'";
            }
            else {
                settingPath += "&customerId=" + this.target.customerId;
            }
            this.settingPath = settingPath;
        };
        ActorController.$inject = ["$http"];
        return ActorController;
    })();
    ActorForm.ActorController = ActorController;
    var app = angular.module("actor-form", []);
    app.controller("ActorController", ActorForm.ActorController);
})(ActorForm || (ActorForm = {}));
