/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="commission-resources.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    class SettingType19Directive extends SettingTypeDirective {
        public static $inject: string[] = ["$window", "commissionResourceService"];

        public templateUrl: string = "settingtype19.htm";

        public link: ng.IDirectiveLinkFn = (scope: IDefaultSettingTypeDirectiveScope) => {
            /* tslint:disable:no-string-literal */
            scope.isMultiple = this.$window["params"].isMultiple === undefined ? false : this.$window["params"].isMultiple;
            /* tslint:enable:no-string-literal */
            if (scope.customerLevel === Level.member && scope.uplineLevel !== Level.agent) {
                scope.headerTemplateUrl = "settingtype19-header.htm";
            }

            scope.valueTemplateUrl = "settingtype19-value.htm";
            scope.settingTypeName = this.commissionResourceService.getLabel("Commission");
        };

        private commissionResourceService: CommissionResourceService;
        private $window: ng.IWindowService;

        constructor($window: ng.IWindowService, commissionResourceService: CommissionResourceService) {
            super((settingItem: any,
                backup?: boolean,
                customerLevel?: Level,
                uplineLevel?: Level,
                extraData?: Array<KeyValuePair<string, any>>,
                multipleUpdates?: boolean) => new SettingType19Item(settingItem, backup, multipleUpdates));

            this.commissionResourceService = commissionResourceService;
            this.$window = $window;
        }
    }

    ModuleRegister.directive("settingtype19", SettingType19Directive);
}