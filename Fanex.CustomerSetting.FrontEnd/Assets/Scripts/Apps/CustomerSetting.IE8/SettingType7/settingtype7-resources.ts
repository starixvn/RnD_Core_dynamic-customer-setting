/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType7ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType7Resources", "parametersService"];

        constructor(settingType7Resources: any, parametersService: ParametersService) {
            super(settingType7Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType7ResourceService", SettingType7ResourceService);

    function SettingType7LabelFilter(settingType7ResourceService: SettingType7ResourceService): Function {
        return (name: string) => {
            return settingType7ResourceService.getLabel(name);
        };
    }

    SettingType7LabelFilter.$inject = ["settingType7ResourceService"];
    ModuleRegister.filter("settingType7Label", SettingType7LabelFilter);

    function SettingType7MessageFilter(settingType7ResourceService: SettingType7ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            return settingType7ResourceService.getFormattedMessage.apply(settingType7ResourceService, [name, replacements]);
        };
    }

    SettingType7MessageFilter.$inject = ["settingType7ResourceService"];
    ModuleRegister.filter("settingType7Message", SettingType7MessageFilter);
}