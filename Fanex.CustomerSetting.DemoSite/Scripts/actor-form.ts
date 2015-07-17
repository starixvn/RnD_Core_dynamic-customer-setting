/// <reference path="typings/tsd.d.ts" />
module ActorForm {
    class Language {
        constructor(public name: string, public code: string) { }
    }

    enum RoleType {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
    }

    class Role {
        constructor(public roleName: string, public roleId: number) { }
    }

    class Actor {
        constructor(
            public actorId: number,
            public role: RoleType,
            public subAccountId: number,
            public language: string) { }
    }

    interface Target {
        productId: number;
        role: RoleType;
    }

    class SingleTarget implements Target {
        constructor(
            public customerId: number,
            public role: RoleType,
            public productId: number) { }
    }

    class MultipleTargets implements Target {
        constructor(
            public customerIds: string,
            public customerNames: string,
            public role: RoleType,
            public productId: number) { }

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

        public static getAvailableActorRoles(): Role[] {
            var roles: Role[] = new Array<Role>();

            for (var role in RoleType) {
                roles.push(new Role(RoleType[role], role));
            }

            return roles;
        }

        public static getSampleActor(selectedRole: RoleType): Actor {
            var actorId: number = 0;
            var subAccId: number = 0;

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
        }

        private static createActor(id: number, role: RoleType, subAccId: number): Actor {
            return new Actor(id, role, subAccId, this.defaultLanguage);
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
            this.actor = SampleDataFactory.getSampleActor(RoleType.company);
            this.target = null;
            this.isMultiple = false;
            this.languages = SampleDataFactory.getLanuages();
            this.roles = SampleDataFactory.getAvailableActorRoles();
        }

        public actor: Actor;
        public isMultiple: boolean;
        public target: Target;
        public languages: Language[];
        public roles: Role[];
    }
}

var app = angular.module("actor-form", [])

app.controller("ActorController", ActorForm.ActorController);