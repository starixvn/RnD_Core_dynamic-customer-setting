/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/default-setting-block.ts" />
/// <reference path="../core/helpers.ts" />
/// <reference path="../core/models.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    class Settingtype4Item extends DefaultSettingItem {
        public isDisabled: boolean;
        public upline: Settingtype4Item;
        public downline: Settingtype4Item;

        constructor(settingItem: any) {
            super(settingItem);

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;

                if (settingItem.upline) {
                    this.upline = new Settingtype4Item(settingItem.upline);
                }

                if (settingItem.downline) {
                    this.downline = new Settingtype4Item(settingItem.downline);
                }
            }
        }

        public backup(): void {
            var oldSetting: Settingtype4Item;

            super.backup();
            oldSetting = <Settingtype4Item>this.oldSetting;

            oldSetting.isDisabled = this.isDisabled;

            if (this.upline) {
                oldSetting.upline = new Settingtype4Item(this.upline);
            }

            if (this.downline) {
                oldSetting.downline = new Settingtype4Item(this.downline);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype4Item = <Settingtype4Item>this.oldSetting;

            return isChanged
                || this.isDisabled !== oldSetting.isDisabled
                || (this.upline && this.upline.isDisabled !== oldSetting.upline.isDisabled)
                || (this.downline && this.downline.isDisabled !== oldSetting.downline.isDisabled);
        }
    }

    class SettingType4Template1Directive extends SettingTypeDirective {
        public templateUrl = this.appConfigService.resolveRootUrl("app/customerSetting/settingtype4.template1/settingtype4-template1.htm");

        public link: ng.IDirectiveLinkFn = (scope: SettingTypeDirectiveSharedScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.valueTemplateUrl = "settingtyp4-template1-value.html";

            if (angular.isDefined(scope.settingType) && angular.isArray(scope.settingType.settingItems)) {
                angular.forEach(scope.settingType.settingItems, (item, index) => {
                    if (angular.isDefined(item)) {
                        scope.settingType.settingItems[index] = new Settingtype4Item(item);
                        scope.settingType.settingItems[index].backup();
                    }
                });
            }
        }

        constructor(private appConfigService: AppConfigService) {
            super();
        }
    }

    angular.module("customerSetting").directive("settingtype4Template1", ["appConfigService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(SettingType4Template1Directive, args));
    }]);

    interface StatusCheckboxScope extends ng.IScope {
        settingItem: Settingtype4Item;
        uplineSettingItem: Settingtype4Item;
        isChecked: boolean;
        isEnabled: boolean;
        isDownlineChecked: boolean;
        isDownlineEnabled: boolean;
        change(): void;
        changeDownline(): void;
    }

    class StatusCheckboxDirective implements ng.IDirective {
        public get scope(): any {
            return {
                settingItem: "="
            }
        }

        public templateUrl: string = "status-checkbox.html";

        public controller: any = ["$scope", ($scope: StatusCheckboxScope) => {
            $scope.isChecked = !$scope.settingItem.isDisabled;
            $scope.isEnabled = !$scope.uplineSettingItem || ($scope.uplineSettingItem && !$scope.uplineSettingItem.isDisabled);
            $scope.isDownlineChecked = !$scope.settingItem.downline.isDisabled;
            $scope.isDownlineEnabled = $scope.isChecked;

            $scope.change = () => {
                $scope.isDownlineEnabled = $scope.isChecked;
                $scope.settingItem.downline.isDisabled = !$scope.isDownlineChecked;

                if (!$scope.isChecked && $scope.isDownlineChecked) {
                    $scope.isDownlineChecked = false;
                }
            }

            $scope.changeDownline = () => {
                $scope.settingItem.downline.isDisabled = !$scope.isDownlineChecked;
            }
        }];

        //public link: ng.IDirectiveLinkFn = (scope: StatusCheckboxScope) => {
        //    scope.isChecked = !scope.settingItem.isDisabled;
        //    scope.isEnabled = !scope.uplineSettingItem || (scope.uplineSettingItem && !scope.uplineSettingItem.isDisabled);
        //    scope.isDownlineChecked = !scope.settingItem.downline.isDisabled;
        //    scope.isDownlineEnabled = scope.isChecked;
        //}
    }

    angular.module("customerSetting").directive("statusCheckbox", [function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(StatusCheckboxDirective, args));
    }]);
}