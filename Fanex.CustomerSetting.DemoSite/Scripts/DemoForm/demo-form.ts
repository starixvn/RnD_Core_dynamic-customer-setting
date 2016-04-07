/// <reference path="../typings/angularjs/angular.d.ts" />

module DemoFrom {
    "use strict";

    var app = angular.module("actorForm", []);

    class DemoFormController {
        public static $inject = ["$http", "$location", "$window", "postingToIframeService"];

        public actor: Actor;
        public systemTypes: KeyValuePair<number, string>[];
        public actorRoles: KeyValuePair<number, string>[];
        public languages: KeyValuePair<string, string>[];
        public target: Target;
        public targetRoles: KeyValuePair<number, string>[];
        public uplineRoles: KeyValuePair<number, string>[];
        public isMultipleUpdates: boolean;
        public preventSavingSession: boolean;
        public preventShowingData: boolean;

        private $http: ng.IHttpService;
        private $location: ng.ILocationService;
        private $window: ng.IWindowService;
        private postingToIframeService: PostingToIframeService;

        private applicationPath: string;

        constructor(
            $http: ng.IHttpService,
            $location: ng.ILocationService,
            $window: ng.IWindowService,
            postingToIframeService: PostingToIframeService) {
            this.$http = $http;
            this.$location = $location;
            this.$window = $window;

            /* tslint:disable:no-string-literal */
            this.applicationPath = this.$window["applicationPath"];
            /* tslint:enable:no-string-literal */

            if (this.applicationPath && this.applicationPath[this.applicationPath.length - 1] !== "/") {
                this.applicationPath += "/";
            }

            this.postingToIframeService = postingToIframeService;
            this.systemTypes = SampleDataFactory.getSystemTypes();
            this.actorRoles = Actor.getAvailableRoles();
            this.languages = SampleDataFactory.getLanuages();

            this.changeActorRole();
        }

        public switchForm(state: boolean) {
            this.preventSavingSession = state;
            this.preventShowingData = !state;
        }

        public changeActorRole(): void {
            var selectedRole: Role = angular.isDefined(this.actor) ? this.actor.role : Role.company;
            var selectedSystemId: number = angular.isDefined(this.actor) ? this.actor.system : this.systemTypes[0].key;

            this.actor = SampleDataFactory.getActor(selectedRole, selectedSystemId);
            this.target = undefined;
            this.targetRoles = Target.getAvailableRoles(selectedRole);
            this.isMultipleUpdates = false;

            this.getTarget(selectedRole);
            this.switchForm(false);
        }

        public getTarget(actorRole: Role): void {
            var selectedRole: Role = angular.isDefined(this.target) ? this.target.role : this.targetRoles[0].key;
            var uplineRole: Role = actorRole;

            this.uplineRoles = undefined;

            if (selectedRole === Role.member) {
                if (actorRole === Role.company) {
                    this.uplineRoles = Target.getUplineRoles(selectedRole);
                    uplineRole = this.uplineRoles[0].key;
                } else {
                    uplineRole = actorRole;
                }
            } else {
                uplineRole = undefined;
            }

            if (this.isMultipleUpdates) {
                this.target = SampleDataFactory.getMultipleTarget(selectedRole, uplineRole);
            } else {
                this.target = SampleDataFactory.getSingleTarget(selectedRole, uplineRole);
            }
        }

        public saveSession(): void {
            this.$http.post(this.resolveUrl("home/saveSession"), this.actor).then(() => {
                this.switchForm(true);
            });
        }

        public showSetting(): void {
            var params: any = {
                language: this.actor.language,
                productId: this.target.productId,
                customerLevel: this.target.role,
                uplineLevel: this.target.uplineRole,
                path: this.resolveUrl("DCS/ViewCustomerSetting"),
                frameName: "setting-page"
            };

            if (this.isMultipleUpdates) {
                params.isMultiple = true;
                params.customerIds = (<MultipleTargets>this.target).customerIds;
                params.customerNames = (<MultipleTargets>this.target).customerNames;
            } else {
                params.isMultiple = false;
                params.customerId = (<SingleTarget>this.target).customerId;
            }

            this.postingToIframeService.submitToIframe(params);
        }

        public viewMatrix(): void {
            var customerName: string = "";

            if (this.isMultipleUpdates) {
                customerName = (<MultipleTargets>this.target).firstCustomerName;
            } else {
                customerName = (<SingleTarget>this.target).customerName;
            }

            this.$window.open(this.resolveUrl("Matrix/ViewCustomerSettingMatrix?showFilter=true&settingTypeId=1&sportTypeId=44&betTypeId=3&customerName=" + customerName), "_bank");
        }

        private resolveUrl(requestUrl: string): string {
            return this.applicationPath + requestUrl;
        }
    }

    app.controller("DemoFormController", DemoFormController);

    interface IPostingToIframeScope extends ng.IScope {
        language: string;
        productId: number;
        customerLevel: Role;
        uplineLevel: Role;
        customerId: number;
        isMultiple: boolean;
        customerIds: Array<number>;
        customerNames: Array<string>;
        path: string;
        frameName: string;
        submitToIframe: () => void;
    }

    class PostingToIframeDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "posting-to-iframe.htm";

        public link: ng.IDirectiveLinkFn = (scope: IPostingToIframeScope, element: ng.IAugmentedJQuery) => {
            var form: HTMLFormElement = <HTMLFormElement>element[0];

            this.postingToIframeService.setup(scope);

            scope.submitToIframe = () => {
                form.submit();

                return false;
            };
        };

        private postingToIframeService: PostingToIframeService;

        constructor(postingToIframeService: PostingToIframeService) {
            this.postingToIframeService = postingToIframeService;
        }
    }

    app.directive("postingToIframe", ["postingToIframeService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(PostingToIframeDirective, args));
    }]);

    class PostingToIframeService {
        public static $inject: string[] = ["$timeout"];

        private scope: IPostingToIframeScope;
        private $timeout: ng.ITimeoutService;

        constructor($timeout: ng.ITimeoutService) {
            this.$timeout = $timeout;
        }

        public setup(scope: IPostingToIframeScope): void {
            this.scope = scope;
        }

        public submitToIframe(params: any) {
            Object.keys(params).forEach((prop: string) => {
                this.scope[prop] = params[prop];
            });

            this.$timeout(() => {
                this.scope.submitToIframe();
            });
        }
    }

    app.service("postingToIframeService", PostingToIframeService);

    class SampleDataFactory {
        private static defaultProductId: number = 22;
        private static defaultLanguage: string = "en-US";

        public static getSystemTypes(): KeyValuePair<number, string>[] {
            return new Array<KeyValuePair<number, string>>(
                new KeyValuePair<number, string>(1, "accounting"),
                new KeyValuePair<number, string>(2, "agency"),
                new KeyValuePair<number, string>(3, "external accounting"));
        }

        public static getLanuages(): KeyValuePair<string, string>[] {
            return new Array<KeyValuePair<string, string>>(
                new KeyValuePair<string, string>("en-US", "English (en-US)"),
                new KeyValuePair<string, string>("zh-TW", "繁體中文 (zh-TW)"),
                new KeyValuePair<string, string>("zh-CN", "简体中文 (zh-CN)"),
                new KeyValuePair<string, string>("ja-JP", "日本語 (ja-JP)"),
                new KeyValuePair<string, string>("th-TH", "ภาษาไทย (th-TH)"),
                new KeyValuePair<string, string>("zh-TW", "한국어 (ko-KR)"),
                new KeyValuePair<string, string>("vi-VN", "Tiếng Việt (vi-VN)"));
        }

        public static getActor(selectedRole: Role, systemId: number): Actor {
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
                    actorId = 1176070;
                    name = "SA-RM";
                    subAccId = 3;
                    break;

                case Role.master:
                    actorId = 1176083;
                    name = "wsrm";
                    subAccId = 728497;
                    break;

                case Role.agent:
                    actorId = 1176086;
                    name = "428rm";
                    subAccId = 728498;
                    break;
            }

            return this.createActor(actorId, name, selectedRole, subAccId, systemId);
        }

        public static getSingleTarget(role: Role, uplineRole: Role): SingleTarget {
            var id: number = 0;
            var name: string = "";

            switch (role) {
                case Role.super:
                    id = 1176070;
                    name = "SuperTest1";
                    break;

                case Role.master:
                    id = 1176083;
                    name = "master1";
                    break;

                case Role.agent:
                    id = 1176086;
                    name = "agent01";
                    break;

                case Role.member:
                    id = 7890346;
                    name = "AGENT01000";
                    break;
            }

            return this.createSingleTarget(id, name, role, uplineRole);
        }

        public static getMultipleTarget(role: Role, uplineRole: Role): MultipleTargets {
            var ids: string = "";
            var names: string = "";

            switch (role) {
                case Role.super:
                    ids = "1176070,576313,3230178";
                    names = "SuperTest1,SuperTest2,SuperTest12";
                    break;

                case Role.master:
                    ids = "1176083,14167350,14373487";
                    names = "master1,mastertest01,masterkz";
                    break;

                case Role.agent:
                    ids = "1176086,14373905,14374140";
                    names = "agent01,agent03,agentlcs000";
                    break;

                case Role.member:
                    ids = "7890346,8252951,8252986";
                    names = "AGENT01000,AGENT01001,AGENT01002";
                    break;
            }

            return this.createMultipleTargetInfo(ids, names, role, uplineRole);
        }

        private static createActor(id: number, name: string, role: Role, subAccId: number, systemId: number): Actor {
            return new Actor(id, name, role, subAccId, this.defaultLanguage, systemId);
        }

        private static createSingleTarget(id: number, name: string, role: Role, uplineRole: Role): SingleTarget {
            return new SingleTarget(id, name, role, uplineRole, this.defaultProductId);
        }

        private static createMultipleTargetInfo(ids: string, names: string, role: Role, uplineRole: Role): MultipleTargets {
            return new MultipleTargets(ids, names, role, uplineRole, this.defaultProductId);
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
        public key: T;
        public value: K;

        constructor(key: T, value: K) {
            this.key = key;
            this.value = value;
        }
    }

    class Actor {
        public actorId: number;
        public actorName: string;
        public role: Role;
        public subAccountId: number;
        public language: string;
        public system: number;

        constructor(actorId: number, actorName: string, role: Role, subAccountId: number, language: string, system: number) {
            this.actorId = actorId;
            this.actorName = actorName;
            this.role = role;
            this.subAccountId = subAccountId;
            this.language = language;
            this.system = system;
        }

        public static getAvailableRoles(): KeyValuePair<number, string>[] {
            var roles = new Array<KeyValuePair<number, string>>();

            for (var item in Role) {
                if (item) {
                    var roleId: number = parseInt(item, 10);

                    if (roleId !== Role.member) {
                        roles.push(new KeyValuePair<number, string>(roleId, Role[roleId]));
                    }
                }
            }

            return roles.reverse();
        }
    }

    class Target {
        public productId: number;
        public role: Role;
        public uplineRole: Role;

        constructor(productId: number, role: Role, uplineRole: Role) {
            this.productId = productId;
            this.role = role;
            this.uplineRole = uplineRole;
        }

        public static getAvailableRoles(uplineRole: Role): KeyValuePair<number, string>[] {
            var roles = new Array<KeyValuePair<number, string>>();

            for (var item in Role) {
                if (item) {
                    var roleId: number = parseInt(item, 10);

                    if (roleId < uplineRole) {
                        roles.push(new KeyValuePair<number, string>(roleId, Role[roleId]));
                    }
                }
            }

            return roles.reverse();
        }

        public static getUplineRoles(downlineRole: Role): KeyValuePair<number, string>[] {
            var roles = new Array<KeyValuePair<number, string>>();

            for (var item in Role) {
                if (item) {
                    var roleId: number = parseInt(item, 10);

                    if (downlineRole < roleId && roleId < Role.company) {
                        roles.push(new KeyValuePair<number, string>(roleId, Role[roleId]));
                    }
                }
            }

            return roles.reverse();
        }
    }

    class SingleTarget extends Target {
        public customerId: number;
        public customerName: string;

        constructor(customerId: number, customerName: string, role: Role, uplineRole: Role, productId: number) {
            super(productId, role, uplineRole);
            this.customerId = customerId;
            this.customerName = customerName;
        }
    }

    class MultipleTargets extends Target {
        public customerIds: string;
        public customerNames: string;

        public get firstCustomerName(): string {
            if (this.customerNames) {
                var names: string[] = this.customerNames.split(",");

                if (names.length > 0) {
                    return names[0];
                }
            }

            return null;
        }

        constructor(customerIds: string, customerNames: string, role: Role, uplineRole: Role, productId: number) {
            super(productId, role, uplineRole);
            this.customerIds = customerIds;
            this.customerNames = customerNames;
        }
    }
}

/* tslint:disable */
function successfulUpdatingCallback(isPartialUpdate, isMultipleUpdate) {
    console.log("Be called after updating sucessfully!");
};
/* tslint:enable */