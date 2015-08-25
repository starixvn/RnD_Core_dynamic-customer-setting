/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/parameters.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class BetSettingResourceService extends ResourceService {
        public static $inject: string[] = ["betSettingResources", "parametersService"];

        constructor(betSettingResources: any, parametersService: ParametersService) {
            super(betSettingResources, parametersService);
        }
    }

    ModuleRegister.service("betSettingResourceService", BetSettingResourceService);

    function BetSettingLabelFilter(betSettingResourceService: BetSettingResourceService) {
        return (name: string) => {
            return betSettingResourceService.getLabel(name);
        };
    }

    LabelFilter.$inject = ["betSettingResourceService"];
    ModuleRegister.filter("betSettingLabel", BetSettingLabelFilter);

    function BetSettingMessageFilter(betSettingResourceService: BetSettingResourceService) {
        return (name: string, ...replacements: any[]) => {
            return betSettingResourceService.getFormattedMessage.apply(betSettingResourceService, [name, replacements]);
        };
    }

    LabelFilter.$inject = ["betSettingResourceService"];
    ModuleRegister.filter("betSettingMessage", BetSettingMessageFilter);
}