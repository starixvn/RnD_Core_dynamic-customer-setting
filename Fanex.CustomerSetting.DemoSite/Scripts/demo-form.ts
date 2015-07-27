/// <reference path="typings/angularjs/angular.d.ts" />

module DemoFrom {
    var app = angular.module("actorForm", [])

    export class DemoFormController {
        public static $inject = ["$http", "$location"];

        public actor: Actor
        public actorRoles: KeyValuePair<number, string>[];
        public languages: KeyValuePair<string, string>[];
        public target: Target;
        public targetRoles: KeyValuePair<number, string>[];
        public isMultipleUpdates: boolean;
        public preventSavingSession: boolean;
        public preventShowingData: boolean;
        public settingPath: string;

        constructor(private $http: ng.IHttpService, private $location: ng.ILocationService) {
            this.actorRoles = Actor.getAvailableRoles();
            this.languages = SampleDataFactory.getLanuages();

            this.changeActorRole();
        }

        public switchForm(state: boolean) {
            this.preventSavingSession = state;
            this.preventShowingData = !state;
        }

        public changeActorRole(): void {
            var selectedRole = angular.isDefined(this.actor) ? this.actor.role : Role.company;

            this.actor = SampleDataFactory.getActor(selectedRole);
            this.target = undefined;
            this.targetRoles = Target.getAvailableRoles(selectedRole);
            this.isMultipleUpdates = false;

            this.getTarget();
            this.switchForm(false);
        }

        public getTarget(): void {
            var selectedRole = angular.isDefined(this.target) ? this.target.role : this.targetRoles[0].key;

            if (this.isMultipleUpdates) {
                this.target = SampleDataFactory.getMultipleTarget(selectedRole);
            } else {
                this.target = SampleDataFactory.getSingleTarget(selectedRole);
            }
        }

        public saveSession(): void {
            this.$http.post("home/saveSession", this.actor).success(() => {
                this.switchForm(true);
            });
        }

        public showSetting(): void {
            var params: any = {
                language: this.actor.language,
                productId: this.target.productId,
                customerLevel: this.target.role
            };

            if (this.actor.role !== Role.company) {
                params.uplineLevel = this.actor.role;
            }

            if (this.isMultipleUpdates) {
                params.isMultiple = true;
                params.customerIds = (<MultipleTargets>this.target).customerIds;
                params.customerNames = (<MultipleTargets>this.target).customerNames;
            } else {
                params.customerId = (<SingleTarget>this.target).customerId;
            }

            this.settingPath = this.$location.path("ViewCustomerSetting").search(params).url().replace("?", "/#?");

            // Cheating step, force the iframe to reload. Because I don't want to create another super simple directive just for the demo page.
            (<HTMLIFrameElement>document.getElementById("setting-page")).contentWindow.location.reload();
        }
    }

    app.controller("DemoFormController", DemoFormController);

    class SampleDataFactory {
        private static defaultProductId: number = 255;
        private static defaultLanguage: string = "en-US";

        public static getLanuages(): KeyValuePair<string, string>[] {
            return new Array<KeyValuePair<string, string>>(
                new KeyValuePair<string, string>("en-US", "English (en-US)"),
                new KeyValuePair<string, string>("zh-TW", "繁體中文 (zh-TW)"),
                new KeyValuePair<string, string>("zh-CN", "简体中文 (zh-CN)"),
                new KeyValuePair<string, string>("ja-JP", "日本語 (ja-JP)"),
                new KeyValuePair<string, string>("ko-KR", "ภาษาไทย (th-TH)"),
                new KeyValuePair<string, string>("zh-TW", "한국어 (ko-KR)"),
                new KeyValuePair<string, string>("vi-VN", "Tiếng Việt (vi-VN)"));
        }

        public static getActor(selectedRole: Role): Actor {
            var actorId: number = 0;
            var name: string = "";
            var subAccId: number = 0;

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
        }

        public static getSingleTarget(role: Role): SingleTarget {
            var id: number = 0;

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
        }

        public static getMultipleTarget(role: Role): MultipleTargets {
            var ids: string = "";
            var names: string = "";

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
        }

        private static createActor(id: number, name: string, role: Role, subAccId: number): Actor {
            return new Actor(id, name, role, subAccId, this.defaultLanguage);
        }

        private static createSingleTarget(id: number, role: Role): SingleTarget {
            return new SingleTarget(id, role, this.defaultProductId);
        }

        private static createMultipleTargetInfo(ids: string, names: string, role: Role): MultipleTargets {
            return new MultipleTargets(ids, names, role, this.defaultProductId);
        }
    }

    enum Role {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
    }

    class KeyValuePair<T, K> {
        constructor(public key: T, public value: K) { }
    }

    class Actor {
        constructor(
            public actorId: number,
            public actorName: string,
            public role: Role,
            public subAccountId: number,
            public language: string) { }

        public static getAvailableRoles(): KeyValuePair<number, string>[] {
            var roles = new Array<KeyValuePair<number, string>>();

            for (var item in Role) {
                var roleId: number = parseInt(item);

                if (!isNaN(roleId) && roleId !== Role.member) {
                    roles.push(new KeyValuePair<number, string>(roleId, Role[roleId]));
                }
            }

            return roles.sort((num1, num2) => num2.key - num1.key);
        }
    }

    class Target {
        constructor(
            public productId: number,
            public role: Role) { }

        public static getAvailableRoles(actorRole: Role): KeyValuePair<number, string>[] {
            var roles = new Array<KeyValuePair<number, string>>();

            for (var item in Role) {
                var roleId: number = parseInt(item);

                if (!isNaN(roleId) && roleId < actorRole) {
                    roles.push(new KeyValuePair<number, string>(roleId, Role[roleId]));
                }
            }

            return roles.sort((num1, num2) => num2.key - num1.key);
        }
    }

    class SingleTarget extends Target {
        public customerId: number;

        constructor(customerId: number, role: Role, productId: number) {
            super(productId, role);
            this.customerId = customerId;
        }
    }

    class MultipleTargets extends Target {
        public customerIds: string;
        public customerNames: string;

        constructor(customerIds: string, customerNames: string, role: Role, productId: number) {
            super(productId, role);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }

        public getFirstCustomerId(): number {
            var ids: string[] = this.customerIds.split(",");

            if (ids !== null && ids.length > 0) {
                return parseInt(ids[0]);
            }

            return 0;
        }
    }
}