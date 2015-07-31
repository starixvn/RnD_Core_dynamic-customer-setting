/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/models.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    class Settingtype4Template1Item extends DefaultSettingItem {
        public isDisabled: boolean;
        public upline: Settingtype4Template1Item;
        public downline: Settingtype4Template1Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = true) {
            super(settingItem);

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;

                if (settingItem.upline) {
                    this.upline = new Settingtype4Template1Item(settingItem.upline, false);
                }

                if (settingItem.downline) {
                    this.downline = new Settingtype4Template1Item(settingItem.downline, false);
                }
            }

            if (backup) {
                this.backupSettingtype4Template1();
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype4Template1Item = <Settingtype4Template1Item>this.oldSetting;

            return isChanged
                || this.isDisabled !== oldSetting.isDisabled
                || (this.downline && this.downline.isDisabled !== oldSetting.downline.isDisabled);
        }

        public get validation(): ValidationResult {
            if (this.isDisabled && !this.downline.isDisabled) {
                return new ValidationResult("DownlineStatusIsInvalid");
            }

            return <ValidationResult>Result.normal;
        }

        private backupSettingtype4Template1(): void {
            var oldSetting: Settingtype4Template1Item;

            this.oldSetting = new Settingtype4Template1Item(undefined, false);
            super.backup();
            oldSetting = <Settingtype4Template1Item>this.oldSetting;
            oldSetting.isDisabled = this.isDisabled;

            if (this.upline) {
                oldSetting.upline = new Settingtype4Template1Item(this.upline, false);
            }

            if (this.downline) {
                oldSetting.downline = new Settingtype4Template1Item(this.downline, false);
            }
        }
    }

    class SettingType4Template1Directive extends SettingTypeDirective {
        public templateUrl: string = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype4.template1/settingtype4-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtyp4-template1-value.html";
        }

        constructor(private appConfigService: AppConfigService) {
            super(Settingtype4Template1Item);
        }
    }

    angular.module("customerSetting").directive("settingtype4Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType4Template1Directive, args));
    }]);

    interface StatusCheckboxScope extends ng.IScope {
        settingItem: Settingtype4Template1Item;
        isChecked: boolean;
        isDisabled: boolean;
        isDownlineChecked: boolean;
        isDownlineDisabled: boolean;
        change(): void;
        changeDownline(): void;
    }

    class StatusCheckboxDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: "="
            }
        }

        public templateUrl: string = "status-checkbox.html";

        public controller: any = ["$scope", "$rootScope", ($scope: StatusCheckboxScope) => {
            $scope.$watch("settingItem", () => {
                $scope.isChecked = !$scope.settingItem.isDisabled;
                $scope.isDisabled = $scope.settingItem.upline && $scope.settingItem.upline.isDisabled;
                $scope.isDownlineChecked = !$scope.settingItem.downline.isDisabled;
                $scope.isDownlineDisabled = true;
            });

            $scope.change = () => {
                $scope.settingItem.isDisabled = !$scope.isChecked;
                $scope.isDownlineDisabled = !$scope.isChecked;

                if (!$scope.isChecked && $scope.isDownlineChecked) {
                    $scope.isDownlineChecked = false;
                    $scope.settingItem.downline.isDisabled = true;
                }
            }

            $scope.changeDownline = () => {
                $scope.settingItem.downline.isDisabled = !$scope.isDownlineChecked;
            }
        }];
    }

    angular.module("customerSetting").directive("statusCheckbox", [function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(StatusCheckboxDirective, args));
    }]);
}