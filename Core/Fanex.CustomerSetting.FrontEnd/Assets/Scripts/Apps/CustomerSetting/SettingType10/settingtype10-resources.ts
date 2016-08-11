/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType10ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType11Resources", "parametersService"];

        constructor(settingType11Resources: any, parametersService: ParametersService) {
            super(settingType11Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType10ResourceService", SettingType10ResourceService);

    function SettingType10LabelFilter(settingType10ResourceService: SettingType10ResourceService): Function {
        return (name: string) => {
            return settingType10ResourceService.getLabel(name);
        };
    }

    SettingType10LabelFilter.$inject = ["settingType10ResourceService"];
    ModuleRegister.filter("settingType10Label", SettingType10LabelFilter);

    function SettingType10MessageFilter(settingType10ResourceService: SettingType10ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType10ResourceService.getFormattedMessage.apply(settingType10ResourceService, replacements);
        };
    }

    SettingType10MessageFilter.$inject = ["settingType10ResourceService"];
    ModuleRegister.filter("settingType10Message", SettingType10MessageFilter);
}