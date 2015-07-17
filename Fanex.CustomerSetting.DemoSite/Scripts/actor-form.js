/// <reference path="typings/tsd.d.ts" />
var ActorForm;
(function (ActorForm) {
    var Language = (function () {
        function Language(name, code) {
            this.name = name;
            this.code = code;
        }
        return Language;
    })();
    var RoleType;
    (function (RoleType) {
        RoleType[RoleType["company"] = 5] = "company";
        RoleType[RoleType["super"] = 4] = "super";
        RoleType[RoleType["master"] = 3] = "master";
        RoleType[RoleType["agent"] = 2] = "agent";
        RoleType[RoleType["member"] = 1] = "member";
    })(RoleType || (RoleType = {}));
    var Role = (function () {
        function Role(roleName, roleId) {
            this.roleName = roleName;
            this.roleId = roleId;
        }
        return Role;
    })();
    var Actor = (function () {
        function Actor(actorId, role, subAccountId, language) {
            this.actorId = actorId;
            this.role = role;
            this.subAccountId = subAccountId;
            this.language = language;
        }
        return Actor;
    })();
    var SingleTarget = (function () {
        function SingleTarget(customerId, role, productId) {
            this.customerId = customerId;
            this.role = role;
            this.productId = productId;
        }
        return SingleTarget;
    })();
    var MultipleTargets = (function () {
        function MultipleTargets(customerIds, customerNames, role, productId) {
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
    })();
    var SampleDataFactory = (function () {
        function SampleDataFactory() {
        }
        SampleDataFactory.getLanuages = function () {
            return new Array(new Language("English (en-US)", "en-US"), new Language("繁體中文 (zh-TW)", "zh-TW"), new Language("简体中文 (zh-CN)", "zh-CN"), new Language("日本語 (ja-JP)", "ja-JP"), new Language("ภาษาไทย (th-TH)", "ko-KR"), new Language("한국어 (ko-KR)", "zh-TW"), new Language("Tiếng Việt (vi-VN)", "vi-VN"));
        };
        SampleDataFactory.getAvailableActorRoles = function () {
            var roles = new Array();
            for (var role in RoleType) {
                roles.push(new Role(RoleType[role], role));
            }
            return roles;
        };
        SampleDataFactory.getSampleActor = function (selectedRole) {
            var actorId = 0;
            var subAccId = 0;
            switch (selectedRole) {
                case RoleType.company:
                    actorId = 8;
                    subAccId = 111;
                    break;
                case RoleType.super:
                    actorId = 2;
                    subAccId = 3;
                    break;
                case RoleType.master:
                    actorId = 4174;
                    subAccId = 728497;
                    break;
                case RoleType.agent:
                    actorId = 1262;
                    subAccId = 728498;
                    break;
            }
            return this.createActor(actorId, selectedRole, subAccId);
        };
        SampleDataFactory.createActor = function (id, role, subAccId) {
            return new Actor(id, role, subAccId, this.defaultLanguage);
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
            this.actor = SampleDataFactory.getSampleActor(RoleType.company);
            this.target = null;
            this.isMultiple = false;
            this.languages = SampleDataFactory.getLanuages();
            this.roles = SampleDataFactory.getAvailableActorRoles();
        }
        ActorController.$inject = ["$http"];
        return ActorController;
    })();
    ActorForm.ActorController = ActorController;
})(ActorForm || (ActorForm = {}));
var app = angular.module("actor-form", []);
app.controller("ActorController", ActorForm.ActorController);
