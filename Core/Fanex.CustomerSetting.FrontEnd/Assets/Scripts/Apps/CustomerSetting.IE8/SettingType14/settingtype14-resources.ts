/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType14ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType14Resources", "parametersService"];

        constructor(settingType14Resources: any, parametersService: ParametersService) {
            super(settingType14Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType14ResourceService", SettingType14ResourceService);

    function SettingType14LabelFilter(settingType14ResourceService: SettingType14ResourceService): Function {
        return (name: string) => {
            return settingType14ResourceService.getLabel(name);
        };
    }

    SettingType14LabelFilter.$inject = ["settingType14ResourceService"];
    ModuleRegister.filter("settingType14Label", SettingType14LabelFilter);

    function SettingType14MessageFilter(settingType14ResourceService: SettingType14ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType14ResourceService.getFormattedMessage.apply(settingType14ResourceService, replacements);
        };
    }

    SettingType14MessageFilter.$inject = ["settingType14ResourceService"];
    ModuleRegister.filter("settingType14Message", SettingType14MessageFilter);
}