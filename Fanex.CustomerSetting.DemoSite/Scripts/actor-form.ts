/// <reference path="typings/angularjs/angular.d.ts" />
module ActorForm {
    enum RoleType {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
    }

    class Language {
        constructor(public name: string, public code: string) { }
    }

    class Role {
        constructor(public roleName: string, public roleId: number) { }
    }

    class Actor {
        constructor(
            public actorId: number,
            public actorName: string,
            public role: RoleType,
            public subAccountId: number,
            public language: string) { }

        public static getAvailableRoles(): Role[] {
            var roles: Role[] = new Array<Role>();

            for (var role in RoleType) {
                if (!isNaN(role) && parseInt(role) !== RoleType.member) {
                    roles.push(new Role(RoleType[role], parseInt(role)));
                }
            }

            return roles.sort((num1, num2) => num2.roleId - num1.roleId);
        }
    }

    class Target {
        constructor(
            public productId: number,
            public role: RoleType) { }

        public static getAvailableRoles(actorRole: RoleType): Role[] {
            var roles: Role[] = new Array<Role>();

            for (var role in RoleType) {
                if (!isNaN(role) && parseInt(role) < actorRole) {
                    roles.push(new Role(RoleType[role], parseInt(role)));
                }
            }

            return roles.sort((num1, num2) => num2.roleId - num1.roleId);
        }
    }

    class SingleTarget extends Target {
        constructor(
            public customerId: number,
            public role: RoleType,
            public productId: number) {
            super(productId, role);
        }
    }

    class MultipleTargets extends Target {
        constructor(
            public customerIds: string,
            public customerNames: string,
            public role: RoleType,
            public productId: number) {
            super(productId, role);
        }

        public getFirstCustomerId(): number {
            var ids: string[] = this.customerIds.split(",");

            if (ids !== null && ids.length > 0) {
                return parseInt(ids[0]);
            }

            return 0;
        }
    }

    class SampleDataFactory {
        private static defaultProductId: number = 255;
        private static defaultLanguage: string = "en-US";

        public static getLanuages(): Language[] {
            return new Array<Language>(
                new Language("English (en-US)", "en-US"),
                new Language("繁體中文 (zh-TW)", "zh-TW"),
                new Language("简体中文 (zh-CN)", "zh-CN"),
                new Language("日本語 (ja-JP)", "ja-JP"),
                new Language("ภาษาไทย (th-TH)", "ko-KR"),
                new Language("한국어 (ko-KR)", "zh-TW"),
                new Language("Tiếng Việt (vi-VN)", "vi-VN"));
        }

        public static getSampleActor(selectedRole: RoleType): Actor {
            var actorId: number = 0;
            var name: string = "";
            var subAccId: number = 0;

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
        }

        public static getSampleSingleTarget(role: RoleType): SingleTarget {
            var id: number = 0;

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
        }

        public static getSampleMultipleTarget(role: RoleType): MultipleTargets {
            var ids: string = "";
            var names: string = "";

            switch (role) {
                case RoleType.company:
                    ids = "2, 3, 4";
                    names = "SA-RM, SA-US$, SA-TB";
                    break;

                case RoleType.super:
                    ids = "4174, 4300, 4356";
                    names = "mrm, mbrm, MYRM";
                    break;

                case RoleType.master:
                    ids = "1262, 1653, 2400";
                    names = "wsrm, SKRM, 216COM";
                    break;

                case RoleType.agent:
                    ids = "1277, 1284, 1290";
                    names = "428RM, 305RM, 302RM";
                    break;
            }

            return this.createMultipleTargetInfo(ids, names, role);
        }

        private static createActor(id: number, name: string, role: RoleType, subAccId: number): Actor {
            return new Actor(id, name, role, subAccId, this.defaultLanguage);
        }

        private static createSingleTarget(id: number, role: RoleType): SingleTarget {
            return new SingleTarget(id, role, this.defaultProductId);
        }

        private static createMultipleTargetInfo(ids: string, names: string, role: RoleType): MultipleTargets {
            return new MultipleTargets(ids, names, role, this.defaultProductId);
        }
    }

    export class ActorController {
        public static $inject: string[] = ["$http"];

        constructor(private $http: ng.IHttpService) {
            this.actorRoles = Actor.getAvailableRoles();
            this.languages = SampleDataFactory.getLanuages();
            this.isMultiple = false;
            this.changeActorRole();
        }

        public actor: Actor;
        public actorRoles: Role[];
        public languages: Language[];
        public target: Target;
        public targetRoles: Role[];
        public isMultiple: boolean;
        public preventSavingSession: boolean;
        public preventShowingData: boolean;

        public changeActorRole(): void {
            var selectedRoleId = this.actor !== undefined ? this.actor.role : RoleType.company;

            this.actor = SampleDataFactory.getSampleActor(selectedRoleId);
            this.targetRoles = Target.getAvailableRoles(selectedRoleId);

            this.isMultiple = false;
            this.target = undefined;
            this.changeTargetRole();
            this.enableSavingSession();
            this.disableShowingData();
        }

        public disableSavingSession() {
            this.preventSavingSession = true;
        }

        public enableSavingSession() {
            this.preventSavingSession = false;
        }

        public disableShowingData() {
            this.preventShowingData = true;
        }

        public enableShowingData() {
            this.preventShowingData = false;
        }

        public changeTargetRole(): void {
            var selectedRoleId = this.target !== undefined ? this.target.role : this.targetRoles[0].roleId;

            if (this.isMultiple) {
                this.target = SampleDataFactory.getSampleMultipleTarget(selectedRoleId);
            } else {
                this.target = SampleDataFactory.getSampleSingleTarget(selectedRoleId);
            }
        }

        public saveSession(): void {
            this.$http.post("home/saveSession", this.actor).success(() => {
                this.disableSavingSession();
                this.enableShowingData();
            });
        }
    }

    var app = angular.module("actor-form", [])

    app.controller("ActorController", ActorForm.ActorController);
}