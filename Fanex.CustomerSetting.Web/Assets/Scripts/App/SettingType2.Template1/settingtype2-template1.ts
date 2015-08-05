/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="bet-setting-resources.ts" />

module CustomerSetting {
    class Settingtype2Template1Item extends DefaultSettingItem {
        public minBet: number;
        public maxBet: number;
        public maxPerMatch: number;
        public upline: Settingtype2Template1Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.minBet = settingItem.minBet;
                this.maxBet = settingItem.maxBet;
                this.maxPerMatch = settingItem.maxPerMatch;

                if (settingItem.upline) {
                    this.upline = new Settingtype2Template1Item(settingItem.upline);
                }
            }

            if (backup) {
                this.oldSetting = new Settingtype2Template1Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype2Template1Item = <Settingtype2Template1Item>this.oldSetting;

            return isChanged
                || this.minBet !== oldSetting.minBet
                || this.maxBet !== oldSetting.maxBet
                || this.maxPerMatch !== oldSetting.maxPerMatch;
        }

        public validate(): boolean {
            if (this.validateSpecialCase()) {
                return true;
            }

            if (!this.validateMinBet()) {
                return false;
            }

            if (!this.validateMaxBet()) {
                return false;
            }

            if (!this.validateMaxPerMatch()) {
                return false;
            }

            this.validationState = <ValidationResult> Result.normal;

            return true;
        }

        public validateSpecialCase(): boolean {
            if (this.minBet === this.maxBet && this.maxBet === this.maxPerMatch && this.maxPerMatch === 0) {
                this.validationState = <ValidationResult>Result.normal;

                return true;
            }
        }

        public validateMinBet(): boolean {
            if (this.minBet < this.upline.minBet) {
                this.validationState = new ValidationResult("MinBetMoreThanOrEqualTo", "minBet", [this.upline.minBet]);

                return false;
            }

            if (this.minBet > this.maxBet) {
                this.validationState = new ValidationResult("MinBetLessThanOrEqualMaxBet", "minBet");

                return false;
            }

            this.validationState = <ValidationResult> Result.normal;

            return true;
        }

        public validateMaxBet(): boolean {
            if (this.upline.maxBet > 0 && this.maxBet > this.upline.maxBet) {
                this.validationState = new ValidationResult("MaxBetLessThanOrEqualTo", "maxBet", [this.upline.maxBet]);

                return false;
            }

            if (this.maxBet > this.maxPerMatch) {
                this.validationState = new ValidationResult("MaxBetLessThanOrEqualMaxPerMatch", "maxBet");

                return false;
            }

            if (this.maxBet < this.minBet) {
                this.validationState = new ValidationResult("MaxBetMoreThanOrEqualMinBet", "maxBet");

                return false;
            }

            this.validationState = <ValidationResult> Result.normal;

            return true;
        }

        public validateMaxPerMatch(): boolean {
            if (this.upline.maxPerMatch > 0 && this.maxPerMatch > this.upline.maxPerMatch) {
                this.validationState = new ValidationResult("MaxPerMatchLessThanOrEqualTo", "maxPerMatch", [this.upline.maxPerMatch]);

                return false;
            }

            if (this.maxBet > this.maxPerMatch) {
                this.validationState = new ValidationResult("MaxPerMatchMoreThanOrEqualMaxBet", "maxPerMatch");

                return false;
            }

            this.validationState = <ValidationResult> Result.normal;

            return true;
        }
    }

    class SettingType2Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype2.template1/settingtype2-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.headerTemplateUrl = "settingtyp2-template1-header.html";
            scope.valueTemplateUrl = "settingtyp2-template1-value.html";
            scope.settingTypeName = this.betSettingResourceService.getLabel("BetSetting");
        }

        constructor(
            private appConfigService: AppConfigService,
            private betSettingResourceService: BetSettingResourceService) {
            super((settingItem: any, backup?: boolean) => new Settingtype2Template1Item(settingItem, backup));
        }
    }

    angular.module("customerSetting").directive("settingtype2Template1", ["appConfigService", "betSettingResourceService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType2Template1Directive, args));
    }]);

    interface BetSettingInputScope extends ng.IScope {
        settingItem: Settingtype2Template1Item;
        customerLevel: Level;
        uplineLevel: Level;
        fieldName: string;
        validationError: string;
        updateValue(): void;
    }

    class BetSettingInputDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "bet-setting-input.html";

        public get scope(): any {
            return {
                settingItem: "=",
                customerLevel: "=",
                uplineLevel: "=",
                fieldName: "@"
            }
        }

        public controller: any = [
            "$scope",
            "betSettingResourceService",
            ($scope: BetSettingInputScope, betSettingResourceService: BetSettingResourceService) => {
                $scope.$watch("settingItem.validationState", (validationState: ValidationResult) => {
                    if ($scope.fieldName === validationState.fieldName && !validationState.isSuccessful) {
                        $scope.validationError = betSettingResourceService.getFormattedMessage(validationState.message, validationState.msgParams);
                    } else {
                        $scope.validationError = undefined;
                    }
                });

                $scope.updateValue = () => {
                    if ($scope.fieldName === "minBet") {
                        $scope.settingItem.validateMinBet();
                    } else if ($scope.fieldName === "maxBet") {
                        $scope.settingItem.validateMaxBet();
                    } else if ($scope.fieldName === "maxPerMatch") {
                        $scope.settingItem.validateMaxPerMatch();
                    }
                };
            }];

        public link: ng.IDirectiveLinkFn = (scope: BetSettingInputScope, wrapElement: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            var element = angular.element(wrapElement.children()[0]);
            var modelCtrl: any;
            var constraintNode = "";

            element.attr("data-ng-model", "settingItem." + scope.fieldName);
            element.attr("data-ng-change", "updateValue()");
            element.attr("data-ng-class", "{ 'invalid': validationError }");
            element.attr("data-tooltip", "");
            element.attr("data-message", "validationError");
            //element.attr("data-show-on-focus", "true");

            if (scope.fieldName === "minBet") {
                if (scope.settingItem.upline.minBet > 0) {
                    element.addClass("has-constraint");
                    wrapElement.append("<span> >={{settingItem.upline." + scope.fieldName + "}}</span>");
                }
            } else {
                if (scope.customerLevel !== Level.super) {
                    element.addClass("has-constraint");
                    wrapElement.append("<span> <={{settingItem.upline." + scope.fieldName + "}}</span>");
                }
            }

            this.$compile(wrapElement)(scope);
            modelCtrl = element.controller("ngModel");

            element.on("mouseup", () => {
                var input: HTMLInputElement = <HTMLInputElement>element[0];
                var length = input.value.length;

                input.setSelectionRange(0, length);
            });

            element.on("keydown", function (event: JQueryEventObject) {
                if (modelCtrl.$modelValue === 0 && [46, 8].indexOf(event.keyCode) >= 0) {
                    event.preventDefault();

                    return;
                }

                if ([46, 8, 9, 27, 13, 110].indexOf(event.keyCode) >= 0 // Allow: backspace, delete, tab, escape, enter
                    || (event.keyCode === 65 && event.ctrlKey === true) // Allow: Ctrl+A
                    || (event.keyCode >= 35 && event.keyCode <= 40)) { // Allow: home, end, left, right, down, up
                    // let it happen, don't do anything
                    return;
                }

                if ((event.shiftKey || (event.keyCode < 48 || event.keyCode > 57))
                    && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();

                    return;
                }
            });

            modelCtrl.$formatters.unshift(() => {
                return this.$filter("number")(modelCtrl.$modelValue);
            });

            modelCtrl.$parsers.unshift((viewValue: string) => {
                var value: number = 0;

                if (viewValue) {
                    var plainNumber = viewValue.replace(/[\,\.]/g, "");

                    value = parseInt(plainNumber);
                }

                element.val(this.$filter("number")(value));

                return value;
            });
        }

        constructor(
            private $compile: ng.ICompileService,
            private $filter: ng.IFilterService) { }
    }

    angular.module("customerSetting").directive("betSettingInput", ["$compile", "$filter", "betSettingResourceService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(BetSettingInputDirective, args));
    }]);
}