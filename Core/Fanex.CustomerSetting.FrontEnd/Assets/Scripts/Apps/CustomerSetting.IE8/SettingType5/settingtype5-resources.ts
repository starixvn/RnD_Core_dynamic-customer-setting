/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType5ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType5Resources", "parametersService"];

        constructor(settingType5Resources: any, parametersService: ParametersService) {
            super(settingType5Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType5ResourceService", SettingType5ResourceService);

    function SettingType5LabelFilter(settingType5ResourceService: SettingType5ResourceService): Function {
        return (name: string) => {
            return settingType5ResourceService.getLabel(name);
        };
    }

    SettingType5LabelFilter.$inject = ["settingType5ResourceService"];
    ModuleRegister.filter("settingType5Label", SettingType5LabelFilter);

    function SettingType5MessageFilter(settingType5ResourceService: SettingType5ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType5ResourceService.getFormattedMessage.apply(settingType5ResourceService, replacements);
        };
    }

    SettingType5MessageFilter.$inject = ["settingType5ResourceService"];
    ModuleRegister.filter("settingType5Message", SettingType5MessageFilter);
}