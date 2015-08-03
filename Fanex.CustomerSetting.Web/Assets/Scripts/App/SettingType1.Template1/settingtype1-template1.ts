/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    class Settingtype1Template1Item {
        public static get maxPTValue(): number {
            return 90;
        }

        public static factory(settingItem?: any, backup?: boolean, customerLevel?: Level, uplineLevel?: Level): SettingItem {
            if (customerLevel === Level.super) {
                return new SuperPositionTaking(settingItem, backup);
            } else if (customerLevel === Level.master) {
                return new MasterPositionTaking(settingItem, backup);
            } else if (customerLevel === Level.agent) {
                return new AgentPositionTaking(settingItem, backup);
            } else if (customerLevel === Level.member && uplineLevel === Level.super) {
                return new SuperPositionTakingOnMember(settingItem, backup);
            } else if (customerLevel === Level.member && uplineLevel === Level.master) {
                return new MasterPositionTakingOnMember(settingItem, backup);
            } else if (customerLevel === Level.member && uplineLevel === Level.agent) {
                return new AgentPositionTakingOnMember(settingItem, backup);
            }
        }

        public static fetch(rawData: any, settingItem: any): void {
            var addedProperties: string[] = ["sportTypeId", "sportTypeName", "betTypeId", "betTypeName", "typeName", "isLive"];

            if (rawData && settingItem) {
                Object.keys(settingItem).filter(propName => {
                    return addedProperties.indexOf(propName) < 0 && rawData[propName];
                }).forEach(prop => {
                    settingItem[prop] = rawData[prop];
                });
            }
        }

        public static detechChanges(settingItem: any): boolean {
            if (settingItem) {
                return Object.keys(settingItem).some(prop => {
                    return settingItem[prop] !== settingItem.oldSetting[prop];
                });
            }

            return false;
        }
    }

    class SuperPositionTaking extends DefaultSettingItem {
        public s4: number;
        public m4: number;
        public a4: number;
        public n4: number;
        public defaultMaxPT: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            Settingtype1Template1Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new SuperPositionTaking(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged() && Settingtype1Template1Item.detechChanges(this);

            return isChanged;
        }

        public validate(): boolean {
            if (this.s4 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "s4");

                return false;
            }

            if (this.m4 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "m4");

                return false;
            }

            if (this.a4 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "a4");

                return false;
            }

            if (this.n4 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "n4");

                return false;
            }

            if (this.m4 > this.s4) {
                this.validationState = new ValidationResult("MasterMaxPTCannotBeGreaterThanSuperMaxPT", "m4");

                return false;
            }

            if (this.a4 > this.m4) {
                this.validationState = new ValidationResult("AgentMaxPTCannotBeGreaterThanMasterMaxPT", "a4");

                return false;
            }

            if (this.n4 > this.s4) {
                this.validationState = new ValidationResult("SuperMinPTCannotBeGreaterThanSuperMaxPT", "n4");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }

    class MasterPositionTaking extends DefaultSettingItem {
        public s3: number;
        public us3: number;
        public is3: boolean;
        public m3: number;
        public n3: number;
        public s4: number;
        public m4: number;
        public n4: number;
        public superPTOnMembers: number;
        public isUpdateSuperPTOnMembers: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            Settingtype1Template1Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new SuperPositionTaking(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged() && Settingtype1Template1Item.detechChanges(this);

            return isChanged;
        }

        public validate(): boolean {
            if (!this.validateMaxValue()) {
                return false;
            }

            if (this.is3 && this.s3 > 0) {
                this.validationState = new ValidationResult("SuperPTCannotBeGreaterThanZeroWhenIsAutoIsSet", "s3");

                return false;
            }

            if (!this.is3 && this.us3 > 0) {
                this.validationState = new ValidationResult("SuperAutoPTCannotBeGreaterThanZeroWhenIsAutoIsNotSet", "us3");

                return false;
            }

            if (this.n3 > this.m3) {
                this.validationState = new ValidationResult("MasterMinPTCannotBeGreaterThanMasterMaxPT", "n3");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private validateMaxValue(): boolean {
            if (this.s3 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "s3");

                return false;
            }

            if (this.us3 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "us3");

                return false;
            }

            if (this.n3 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "n3");

                return false;
            }

            if (this.m3 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "m3");

                return false;
            }
        }
    }

    class AgentPositionTaking extends DefaultSettingItem {
        public m2: number;
        public um2: number;
        public im2: boolean;
        public a2: number;
        public n2: number;
        public m3: number;
        public n3: number;
        public a4: number;
        public masterPTOnMembers: number;
        public isUpdateMasterPTOnMembers: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged() && Settingtype1Template1Item.detechChanges(this);

            return isChanged;
        }

        public validate(): boolean {
            if (!this.validateMaxValue()) {
                return false;
            }

            if (this.im2 && this.m2 > 0) {
                this.validationState = new ValidationResult("MasterPTCannotBeGreaterThanZeroWhenIsAutoIsSet", "m2");

                return false;
            }

            if (!this.im2 && this.um2 > 0) {
                this.validationState = new ValidationResult("MasterAutoPTCannotBeGreaterThanZeroWhenIsAutoIsNotSet", "um2");

                return false;
            }

            if (this.n2 > this.a2) {
                this.validationState = new ValidationResult("AgentMinPTCannotBeGreaterThanAgentMaxPT", "n2");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private validateMaxValue(): boolean {
            if (this.m2 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "m2");

                return false;
            }

            if (this.um2 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "um2");

                return false;
            }

            if (this.n2 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "n2");

                return false;
            }

            if (this.a2 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "a2");

                return false;
            }
        }
    }

    class SuperPositionTakingOnMember extends DefaultSettingItem {
        public a1: number;
        public m1: number;
        public s1: number;
        public us1: number;
        public is1: boolean;
        public is3: boolean;
        public s4: number;
        public n4: number;
        public m3: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged() && Settingtype1Template1Item.detechChanges(this);

            return isChanged;
        }

        public validate(): boolean {
            if (!this.validateMaxValue()) {
                return false;
            }

            if (!this.is1 && this.us1 > 0) {
                this.validationState = new ValidationResult("SuperAutoPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsNotSet", "us1");

                return false;
            }

            if (this.is1 && this.s1 > 0) {
                this.validationState = new ValidationResult("SuperPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsSet", "s1");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private validateMaxValue(): boolean {
            if (this.s1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "s1");

                return false;
            }

            if (this.us1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "us1");

                return false;
            }
        }
    }

    class MasterPositionTakingOnMember extends DefaultSettingItem {
        public a1: number;
        public m1: number;
        public im1: boolean;
        public um1: number;
        public im2: boolean;
        public m3: number;
        public n3: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged() && Settingtype1Template1Item.detechChanges(this);

            return isChanged;
        }

        public validate(): boolean {
            if (!this.validateMaxValue()) {
                return false;
            }

            if (!this.im1 && this.um1 > 0) {
                this.validationState = new ValidationResult("MasterAutoPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsNotSet", "um1");

                return false;
            }

            if (this.im1 && this.m1 > 0) {
                this.validationState = new ValidationResult("MasterPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsSet", "m1");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private validateMaxValue(): boolean {
            if (this.m1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "m1");

                return false;
            }

            if (this.um1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "um1");

                return false;
            }
        }
    }

    class AgentPositionTakingOnMember extends DefaultSettingItem {
        public a1: number;
        public a2: number;
        public n2: boolean;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged() && Settingtype1Template1Item.detechChanges(this);

            return isChanged;
        }

        public validate(): boolean {
            if (this.a1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "a1");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }

    interface SettingType1Template1Scope extends SettingTypeDirectiveSharedScope {
        step: number;
        divisor: number;
    }

    class SettingType1Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype1.template1/settingtype1-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingType1Template1Scope) => {
            scope.headerTemplateUrl = "settingtyp1-template1-header.html";
            scope.valueTemplateUrl = "settingtyp1-template1-value.html";
            scope.settingTypeName = this.resourceService.getLabel("PositionTaking");
            scope.step = 0.0005;
            scope.divisor = 10000;
        }

        constructor(
            private appConfigService: AppConfigService,
            private resourceService: ResourceService) {
            super(Settingtype1Template1Item.factory);
        }
    }

    angular.module("customerSetting").directive("settingtype1Template1", ["appConfigService", "resourceService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType1Template1Directive, args));
    }]);
}