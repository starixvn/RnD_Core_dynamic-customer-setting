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

    class Actor {
        public roleType: RoleType;
        public actorId: number;
        public subAccountId: number;
        public language: string;
    }

    interface Target {
        productId: number;
    }

    class SingleTarget implements Target {
        public productId: number;
        public customerId: number;
    }

    class MultipleTargets implements Target {
        public productId: number;
        public customerIds: string;
        public customerNames: string;

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
        private static superInfo: SingleTarget;
        private static supersInfo: MultipleTargets;
        private static masterInfo: SingleTarget;
        private static mastersInfo: MultipleTargets;
        private static agentInfo: SingleTarget;
        private static agentsInfo: MultipleTargets;
        private static memberInfo: SingleTarget;
        private static membersInfo: MultipleTargets;
        private static languages: Language[];

        public static getLanuages(): Language[] {
            if (this.languages === undefined) {
                this.languages = new Array<Language>(
                    new Language("English (en-US)", "en-US"),
                    new Language("繁體中文 (zh-TW)", "zh-TW"),
                    new Language("简体中文 (zh-CN)", "zh-CN"),
                    new Language("日本語 (ja-JP)", "ja-JP"),
                    new Language("ภาษาไทย (th-TH)", "ko-KR"),
                    new Language("한국어 (ko-KR)", "zh-TW"),
                    new Language("Tiếng Việt (vi-VN)", "vi-VN"));
            }

            return this.languages;
        }

        public static getActorRoles(): { roleName: string; roleId: number }[] {
        }

        public static createSingleSuperInfo(): SingleTarget {
            if (this.superInfo === undefined) {
                this.createSingleTarget(2, this.superInfo);
            }

            return this.superInfo;
        }

        public static createMultipleSupersInfo(): MultipleTargets {
            if (this.supersInfo === undefined) {
                this.createMultipleTargetInfo("2,3,4", "SA-RM,SA-US$,SA-TB", this.supersInfo);
            }

            return this.supersInfo;
        }

        public static createSingleMasterInfo(): SingleTarget {
            if (this.masterInfo === undefined) {
                this.createSingleTarget(4174, this.masterInfo);
            }

            return this.masterInfo;
        }

        public static createMultipleMastersInfo(): MultipleTargets {
            if (this.mastersInfo === undefined) {
                this.createMultipleTargetInfo("4174,4300,4356", "mrm,mbrm,MYRM", this.mastersInfo);
            }

            return this.mastersInfo;
        }

        public static createSingleAgentInfo(): SingleTarget {
            if (this.agentInfo === undefined) {
                this.createSingleTarget(1262, this.agentInfo);
            }

            return SampleDataFactory.agentInfo;
        }

        public static createMultipleAgentsInfo(): MultipleTargets {
            if (this.agentsInfo === undefined) {
                this.createMultipleTargetInfo("1262,1653,2400", "wsrm,SKRM,216COM", this.agentsInfo);
            }

            return this.agentsInfo;
        }

        public static createSingleMemberInfo(): SingleTarget {
            if (this.memberInfo === undefined) {
                this.createSingleTarget(1277, this.memberInfo);
            }

            return SampleDataFactory.memberInfo;
        }

        public static createMultipleMembersInfo(): MultipleTargets {
            if (this.membersInfo === undefined) {
                this.createMultipleTargetInfo("1277,1284,1290", "428RM,305RM,302RM", this.membersInfo);
            }

            return this.membersInfo;
        }

        private static createSingleTarget(id: number, target: SingleTarget): void {
            target = target || new SingleTarget();
            target.customerId = id;
            target.productId = this.defaultProductId;
        }

        private static createMultipleTargetInfo(ids: string, names: string, targets: MultipleTargets): void {
            targets = targets || new MultipleTargets();
            targets.customerIds = ids;
            this.agentsInfo.customerNames = names;
            this.agentsInfo.productId = this.defaultProductId;
        }
    }

    export class ActorController {
        constructor(private $scope: ng.IScope, private $http: ng.IHttpService) {
            //this.actor = SampleDataFactory.de
        }

        actor: Actor;
        isMultiple: boolean;
        target: Target;
        language: Language[];
        roles: { roleName: string; roleId: number; }[];
    }
}