/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="models.ts" />
/// <reference path="product-status-resources.ts" />

module CustomerSetting {
    "use strict";

    class SettingType4Directive extends SettingTypeDirective {
        public static $inject: string[] = ["productStatusResourceService"];

        public templateUrl: string = "settingtype4.htm";

        public link: ng.IDirectiveLinkFn = (scope: ISettingTypeDirectiveSharedScope) => {
            scope.valueTemplateUrl = "settingtype4-value.htm";
            scope.settingTypeName = this.productStatusResourceService.getLabel("ProductStatus");
        };

        private productStatusResourceService: ProductStatusResourceService;

        constructor(productStatusResourceService: ProductStatusResourceService) {
            super((settingItem: any, backup?: boolean) => new SettingType4Item(settingItem, backup));

            this.productStatusResourceService = productStatusResourceService;
        }
    }

    ModuleRegister.directive("settingtype4", SettingType4Directive);
}