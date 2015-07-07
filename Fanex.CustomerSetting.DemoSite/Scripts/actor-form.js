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
    var Actor = (function () {
        function Actor() {
        }
        return Actor;
    })();
    var SingleTarget = (function () {
        function SingleTarget() {
        }
        return SingleTarget;
    })();
    var MultipleTargets = (function () {
        function MultipleTargets() {
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
            if (this.languages === undefined) {
                this.languages = new Array(new Language("English (en-US)", "en-US"), new Language("繁體中文 (zh-TW)", "zh-TW"), new Language("简体中文 (zh-CN)", "zh-CN"), new Language("日本語 (ja-JP)", "ja-JP"), new Language("ภาษาไทย (th-TH)", "ko-KR"), new Language("한국어 (ko-KR)", "zh-TW"), new Language("Tiếng Việt (vi-VN)", "vi-VN"));
            }
            return this.languages;
        };
        SampleDataFactory.getActorRoles = function () {
        };
        SampleDataFactory.createSingleSuperInfo = function () {
            if (this.superInfo === undefined) {
                this.createSingleTarget(2, this.superInfo);
            }
            return this.superInfo;
        };
        SampleDataFactory.createMultipleSupersInfo = function () {
            if (this.supersInfo === undefined) {
                this.createMultipleTargetInfo("2,3,4", "SA-RM,SA-US$,SA-TB", this.supersInfo);
            }
            return this.supersInfo;
        };
        SampleDataFactory.createSingleMasterInfo = function () {
            if (this.masterInfo === undefined) {
                this.createSingleTarget(4174, this.masterInfo);
            }
            return this.masterInfo;
        };
        SampleDataFactory.createMultipleMastersInfo = function () {
            if (this.mastersInfo === undefined) {
                this.createMultipleTargetInfo("4174,4300,4356", "mrm,mbrm,MYRM", this.mastersInfo);
            }
            return this.mastersInfo;
        };
        SampleDataFactory.createSingleAgentInfo = function () {
            if (this.agentInfo === undefined) {
                this.createSingleTarget(1262, this.agentInfo);
            }
            return SampleDataFactory.agentInfo;
        };
        SampleDataFactory.createMultipleAgentsInfo = function () {
            if (this.agentsInfo === undefined) {
                this.createMultipleTargetInfo("1262,1653,2400", "wsrm,SKRM,216COM", this.agentsInfo);
            }
            return this.agentsInfo;
        };
        SampleDataFactory.createSingleMemberInfo = function () {
            if (this.memberInfo === undefined) {
                this.createSingleTarget(1277, this.memberInfo);
            }
            return SampleDataFactory.memberInfo;
        };
        SampleDataFactory.createMultipleMembersInfo = function () {
            if (this.membersInfo === undefined) {
                this.createMultipleTargetInfo("1277,1284,1290", "428RM,305RM,302RM", this.membersInfo);
            }
            return this.membersInfo;
        };
        SampleDataFactory.createSingleTarget = function (id, target) {
            target = target || new SingleTarget();
            target.customerId = id;
            target.productId = this.defaultProductId;
        };
        SampleDataFactory.createMultipleTargetInfo = function (ids, names, targets) {
            targets = targets || new MultipleTargets();
            targets.customerIds = ids;
            this.agentsInfo.customerNames = names;
            this.agentsInfo.productId = this.defaultProductId;
        };
        SampleDataFactory.defaultProductId = 255;
        return SampleDataFactory;
    })();
    var ActorController = (function () {
        function ActorController($scope, $http) {
            this.$scope = $scope;
            this.$http = $http;
            //this.actor = SampleDataFactory.de
        }
        return ActorController;
    })();
    ActorForm.ActorController = ActorController;
})(ActorForm || (ActorForm = {}));
//# sourceMappingURL=actor-form.js.map