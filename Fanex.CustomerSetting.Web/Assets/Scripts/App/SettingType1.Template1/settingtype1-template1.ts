/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="adjust-member-pt-modal.ts" />
/// <reference path="position-taking-resources.ts" />

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
                Object.keys(rawData).filter(propName => {
                    return addedProperties.indexOf(propName) < 0;
                }).forEach(prop => {
                    settingItem[prop] = rawData[prop];
                });
            }
        }

        public static detechChanges(settingItem: any): boolean {
            var reservedProperties: string[] = ["oldSetting", "validationState"];

            if (settingItem) {
                return Object.keys(settingItem).some(prop => {
                    return reservedProperties.indexOf(prop) < 0
                        && settingItem.oldSetting[prop] !== undefined
                        && settingItem[prop] !== settingItem.oldSetting[prop];
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
            Settingtype1Template1Item.fetch(settingItem, <SuperPositionTaking>this);

            if (backup) {
                this.oldSetting = new SuperPositionTaking(this);
            }
        }

        public isChanged(): boolean {
            return Settingtype1Template1Item.detechChanges(this);
        }

        public validate(): boolean {
            if (!this.validateMaxValue()) {
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

        private validateMaxValue(): boolean {
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

            return true;
        }
    }

    class MasterPositionTaking extends DefaultSettingItem {
        public s3: number;
        public uS3: number;
        public iS3: boolean;
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
                this.oldSetting = new MasterPositionTaking(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = Settingtype1Template1Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public validate(): boolean {
            if (this.s3 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "s3");

                return false;
            }

            if (this.uS3 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "uS3");

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

            if (this.n3 > this.m3) {
                this.validationState = new ValidationResult("MasterMinPTCannotBeGreaterThanMasterMaxPT", "n3");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iS3 && this.s3 > 0) {
                this.s3 = 0;
            }

            if (!this.iS3 && this.uS3 > 0) {
                this.uS3 = 0;
            }
        }
    }

    class AgentPositionTaking extends DefaultSettingItem {
        public m2: number;
        public uM2: number;
        public iM2: boolean;
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
            Settingtype1Template1Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new AgentPositionTaking(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = Settingtype1Template1Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public validate(): boolean {
            if (this.m2 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "m2");

                return false;
            }

            if (this.uM2 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "uM2");

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

            if (this.n2 > this.a2) {
                this.validationState = new ValidationResult("AgentMinPTCannotBeGreaterThanAgentMaxPT", "n2");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iM2 && this.m2 > 0) {
                this.m2 = 0;
            }

            if (!this.iM2 && this.uM2 > 0) {
                this.uM2 = 0;
            }
        }
    }

    class SuperPositionTakingOnMember extends DefaultSettingItem {
        public a1: number;
        public m1: number;
        public s1: number;
        public uS1: number;
        public iS1: boolean;
        public iS3: boolean;
        public s4: number;
        public n4: number;
        public m3: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);
            Settingtype1Template1Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new SuperPositionTakingOnMember(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = Settingtype1Template1Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public validate(): boolean {
            if (this.s1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "s1");

                return false;
            }

            if (this.uS1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "uS1");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iS1 && this.s1 > 0) {
                this.s1 = 0;
            }

            if (!this.iS1 && this.uS1 > 0) {
                this.uS1 = 0;
            }
        }
    }

    class MasterPositionTakingOnMember extends DefaultSettingItem {
        public a1: number;
        public m1: number;
        public iM1: boolean;
        public uM1: number;
        public iM2: boolean;
        public m3: number;
        public n3: number;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);
            Settingtype1Template1Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new MasterPositionTakingOnMember(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = Settingtype1Template1Item.detechChanges(this);

            if (isChanged) {
                this.correctAutoPtValues();
            }

            return isChanged;
        }

        public validate(): boolean {
            if (this.m1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "m1");

                return false;
            }

            if (this.uM1 > Settingtype1Template1Item.maxPTValue) {
                this.validationState = new ValidationResult("PositionTakingCannotBeExceedMaxValue", "uM1");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        private correctAutoPtValues(): void {
            if (this.iM1 && this.m1 > 0) {
                this.m1 = 0;
            }

            if (!this.iM1 && this.uM1 > 0) {
                this.uM1 = 0;
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
            Settingtype1Template1Item.fetch(settingItem, this);

            if (backup) {
                this.oldSetting = new AgentPositionTakingOnMember(this);
            }
        }

        public isChanged(): boolean {
            return Settingtype1Template1Item.detechChanges(this);
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

    class SettingType1Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype1.template1/settingtype1-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope) => {
            scope.headerTemplateUrl = "settingtyp1-template1-header.html";
            scope.valueTemplateUrl = "settingtyp1-template1-value.html";
            scope.settingTypeName = this.positionTakingResourceService.getLabel("PositionTaking");
        }

        constructor(
            private appConfigService: AppConfigService,
            private positionTakingResourceService: PositionTakingResourceService) {
            super(Settingtype1Template1Item.factory);
        }
    }

    angular.module("customerSetting").directive("settingtype1Template1", ["appConfigService", "positionTakingResourceService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType1Template1Directive, args));
    }]);

    interface PositionTakingSelectScope extends ng.IScope {
        settingItem: SettingItem;
        fieldName: string;
        min: number;
        max: number;
        divisor: number;
        range: Array<KeyValuePair<number, number>>;
        validationError: string;
    }

    class PositionTakingSelectDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "=",
                fieldName: "@",
                min: "@",
                max: "@"
            }
        }

        public templateUrl: string = "settingtype1-template1-select.html";

        public controller: any = [
            "$scope",
            "valueRangeGenerator",
            "positionTakingResourceService",
            ($scope: PositionTakingSelectScope, valueRangeGenerator: ValueRangeGenerator, positionTakingResourceService: PositionTakingResourceService) => {
                $scope.divisor = Settingtype1Template1Config.roundingFactor;

                $scope.$watch("settingItem.validationState", (validationState: ValidationResult) => {
                    if ($scope.fieldName === validationState.fieldName && !validationState.isSuccessful) {
                        $scope.validationError = positionTakingResourceService.getFormattedMessage(validationState.message, validationState.msgParams);
                    } else {
                        $scope.validationError = undefined;
                    }
                });

                $scope.$watchGroup(["min", "max"], (newData, oldData) => {
                    if ($scope.settingItem) {
                        if (!angular.equals(oldData, newData)) {
                            $scope.settingItem[$scope.fieldName] = Math.max($scope.min, Math.min($scope.max, $scope.settingItem[$scope.fieldName]));
                        }

                        $scope.range = valueRangeGenerator.generateRanges(
                            $scope.settingItem[$scope.fieldName],
                            $scope.min,
                            $scope.max,
                            Settingtype1Template1Config.step,
                            Settingtype1Template1Config.roundingFactor);
                    }
                });
            }];

        public link: ng.IDirectiveLinkFn = (scope: PositionTakingSelectScope, element: ng.IAugmentedJQuery) => {
            var select = element.find("select");

            select.attr("data-ng-model", "settingItem." + scope.fieldName);
            element.attr("data-tooltip", "");
            element.attr("data-message", "validationError");
            this.$compile(element)(scope);
        }

        constructor(private $compile: ng.ICompileService) { }
    }

    angular.module("customerSetting").directive("positionTakingSelect", ["$compile", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(PositionTakingSelectDirective, args));
    }]);

    interface AutoPositionTakingScope extends ng.IScope {
        item: SettingItem;
        confirmUpdatingMemberPt: (isShown: boolean, max: number, updatingMarker: string, ptOnMemberField: string) => void;
    }

    class AutoPositionTakingDirective implements ng.IDirective {
        public restrict: string = "A";

        public controller: any = [
            "$scope",
            "adjustMemberPtModalService",
            ($scope: AutoPositionTakingScope, adjustMemberPtModalService: AdjustMemberPtModalService) => {
                $scope.confirmUpdatingMemberPt = (isShown: boolean, max: number, updatingMarker: string, ptOnMemberField: string) => {
                    if (isShown) {
                        adjustMemberPtModalService.show(max, (memberPt: number) => {
                            $scope.item[updatingMarker] = true;
                            $scope.item[ptOnMemberField] = memberPt;
                        });
                    }
                }
            }];
    }

    angular.module("customerSetting").directive("autoPositionTaking", [function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(AutoPositionTakingDirective, args));
    }]);

    export class Settingtype1Template1Config {
        public static get step(): number {
            return 1;
        }

        public static get roundingFactor(): number {
            return 100;
        }
    }
}