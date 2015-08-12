/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/models.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="product-status-resources.ts" />

module CustomerSetting {
    "use strict";

    class Settingtype4Template1Item extends DefaultSettingItem {
        public isDisabled: boolean;
        public upline: Settingtype4Template1Item;
        public downline: Settingtype4Template1Item;

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;

                if (settingItem.upline) {
                    this.upline = new Settingtype4Template1Item(settingItem.upline);
                }

                if (settingItem.downline) {
                    this.downline = new Settingtype4Template1Item(settingItem.downline);
                }
            }

            if (backup) {
                this.oldSetting = new Settingtype4Template1Item(this);
            }
        }

        public isChanged(): boolean {
            var isChanged: boolean = super.isChanged();
            var oldSetting: Settingtype4Template1Item = <Settingtype4Template1Item>this.oldSetting;

            return isChanged
                || this.isDisabled !== oldSetting.isDisabled
                || (this.downline && this.downline.isDisabled !== oldSetting.downline.isDisabled);
        }

        public copySettingFrom(source: Settingtype4Template1Item): boolean {
            var isSuccessful: boolean = true;

            if (source) {
                this.isDisabled = source.isDisabled;

                if (this.downline && source.downline) {
                    this.downline.isDisabled = source.downline.isDisabled;
                }

                isSuccessful = this.validate();
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.isDisabled && !this.downline.isDisabled) {
                this.validationState = new ValidationResult("DownlineStatusIsInvalid", "downline.isDisabled");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }
    }

    class SettingType4Template1Directive extends SettingTypeDirective {
        public static $inject: string[] = ["appConfigService", "productStatusResourceService"];

        public templateUrl: Function = () => {
            return this.appConfigService.resolveRootUrl("app/customerSetting/settingtype4.template1/settingtype4-template1.htm");
        };

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtyp4-template1-value.html";
            scope.settingTypeName = this.productStatusResourceService.getLabel("ProductStatus");
        };

        private appConfigService: AppConfigService;
        private productStatusResourceService: ProductStatusResourceService;

        constructor(appConfigService: AppConfigService, productStatusResourceService: ProductStatusResourceService) {
            super((settingItem: any, backup?: boolean) => new Settingtype4Template1Item(settingItem, backup));

            this.appConfigService = appConfigService;
            this.productStatusResourceService = productStatusResourceService;
        }
    }

    ModuleRegister.directive("settingtype4Template1", SettingType4Template1Directive);

    interface IStatusCheckboxScope extends ng.IScope {
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
                customerLevel: "=",
                settingItem: "=",
                avaiCustomerLevels: "="
            };
        }

        public templateUrl: string = "status-checkbox.html";

        public controller: any = ["$scope", ($scope: IStatusCheckboxScope) => {
            $scope.$watch("settingItem.isDisabled", () => {
                $scope.isChecked = !$scope.settingItem.isDisabled;
                $scope.isDisabled = $scope.settingItem.upline && $scope.settingItem.upline.isDisabled;

                if (!$scope.settingItem.isDisabled && (<Settingtype4Template1Item>$scope.settingItem.oldSetting).isDisabled) {
                    $scope.isDownlineDisabled = false;
                } else {
                    $scope.isDownlineDisabled = true;
                }

                if ($scope.settingItem.isDisabled && !$scope.settingItem.downline.isDisabled) {
                    $scope.isDownlineChecked = false;

                    if ($scope.settingItem.downline) {
                        $scope.settingItem.downline.isDisabled = true;
                    }
                }
            });

            $scope.$watch("settingItem.downline.isDisabled", () => {
                if ($scope.settingItem.downline) {
                    $scope.isDownlineChecked = !$scope.settingItem.downline.isDisabled;
                }
            });

            $scope.change = () => {
                $scope.settingItem.isDisabled = !$scope.isChecked;
            };

            $scope.changeDownline = () => {
                $scope.settingItem.downline.isDisabled = !$scope.isDownlineChecked;
            };
        }];
    }

    ModuleRegister.directive("statusCheckbox", StatusCheckboxDirective);
}