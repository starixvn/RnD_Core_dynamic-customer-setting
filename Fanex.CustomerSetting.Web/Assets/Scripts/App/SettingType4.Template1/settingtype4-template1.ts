/// <reference path="../../typings/angularjs/angular.d.ts" />
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

        // Additional fields tracing the state of controls. I added those fields due to the state of controls can be cloned via copying function.
        public readonly: boolean;

        public get isChecked(): boolean {
            return !this.isDisabled;
        }

        public set isChecked(isChecked: boolean) {
            this.isDisabled = !isChecked;

            if (this.downline) {
                if (isChecked && !(<Settingtype4Template1Item>this.oldSetting).isChecked) {
                    this.downline.readonly = false;
                } else {
                    this.downline.isChecked = this.isChecked;
                    this.downline.readonly = true;
                }
            }
        }

        constructor();
        constructor(settingItem: any);
        constructor(settingItem: any, backup: boolean)
        constructor(settingItem?: any, backup: boolean = false) {
            super(settingItem);

            if (settingItem) {
                this.isDisabled = settingItem.isDisabled;

                if (settingItem.upline) {
                    this.upline = new Settingtype4Template1Item(settingItem.upline);
                    this.readonly = settingItem.upline.isDisabled;
                } else {
                    this.readonly = false;
                }

                if (settingItem.downline) {
                    this.downline = new Settingtype4Template1Item(settingItem.downline);
                    this.downline.readonly = true;
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
                var backupDisabled: boolean = this.isDisabled;
                var backupReadonly: boolean = this.readonly;
                var backupDisabledDownline: boolean;
                var backupReadonlyDownline: boolean;

                this.isDisabled = source.isDisabled;
                this.readonly = source.readonly;

                if (this.downline && source.downline) {
                    backupDisabledDownline = this.downline.isDisabled;
                    backupReadonlyDownline = this.downline.readonly;
                    this.downline.isDisabled = source.downline.isDisabled;
                    this.downline.readonly = source.downline.readonly;
                }

                isSuccessful = this.validate();

                if (!isSuccessful) {
                    this.isDisabled = backupDisabled;
                    this.readonly = backupReadonly;

                    if (this.downline && source.downline) {
                        this.downline.isDisabled = backupDisabledDownline;
                        this.downline.readonly = backupReadonlyDownline;
                    }
                }
            }

            return isSuccessful;
        }

        public validate(): boolean {
            if (this.isDisabled && this.downline && !this.downline.isDisabled) {
                this.validationState = new ValidationResult("DownlineStatusIsInvalid", "downline.isDisabled");

                return false;
            }

            if (this.downline
                && this.isDisabled === (<Settingtype4Template1Item>this.oldSetting).isDisabled
                && this.downline.isDisabled !== (<Settingtype4Template1Item>this.oldSetting).downline.isDisabled) {
                this.validationState = new ValidationResult("DownlineStatusCannotBeChanged", "downline.isDisabled");

                return false;
            }

            if (this.upline && this.upline.isDisabled && !this.isDisabled) {
                this.validationState = new ValidationResult("DownlineCannotBeEnabledWhenUplineIsDisabled", "isDisabled");

                return false;
            }

            this.validationState = <ValidationResult>Result.normal;

            return true;
        }

        public toJson(): {} {
            var settingItem: any = {
                sportTypeId: this.sportTypeId,
                sportTypeName: this.sportTypeName,
                betTypeId: this.betTypeId,
                betTypeName: this.betTypeName,
                typeName: this.typeName,
                isDisabled: this.isDisabled
            };

            if (this.upline) {
                settingItem.upline = this.upline.toJson();
            }

            if (this.downline) {
                settingItem.downline = this.downline.toJson();
            }

            return settingItem;
        }
    }

    class SettingType4Template1Directive extends SettingTypeDirective {
        public static $inject: string[] = ["productStatusResourceService"];

        public templateUrl: string = "settingtype4-template1.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtyp4-template1-value.htm";
            scope.settingTypeName = this.productStatusResourceService.getLabel("ProductStatus");
        };

        private productStatusResourceService: ProductStatusResourceService;

        constructor(productStatusResourceService: ProductStatusResourceService) {
            super((settingItem: any, backup?: boolean) => new Settingtype4Template1Item(settingItem, backup));

            this.productStatusResourceService = productStatusResourceService;
        }
    }

    ModuleRegister.directive("settingtype4Template1", SettingType4Template1Directive);
}