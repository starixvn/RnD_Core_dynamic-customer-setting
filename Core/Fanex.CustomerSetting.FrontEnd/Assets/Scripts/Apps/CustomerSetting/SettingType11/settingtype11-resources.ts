/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType11ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType11Resources", "parametersService"];

        constructor(settingType11Resources: any, parametersService: ParametersService) {
            super(settingType11Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType11ResourceService", SettingType11ResourceService);

    function SettingType11LabelFilter(settingType11ResourceService: SettingType11ResourceService): Function {
        return (name: string) => {
            return settingType11ResourceService.getLabel(name);
        };
    }

    SettingType11LabelFilter.$inject = ["settingType11ResourceService"];
    ModuleRegister.filter("settingType11Label", SettingType11LabelFilter);

    function SettingType11MessageFilter(settingType11ResourceService: SettingType11ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType11ResourceService.getFormattedMessage.apply(settingType11ResourceService, replacements);
        };
    }

    SettingType11MessageFilter.$inject = ["settingType11ResourceService"];
    ModuleRegister.filter("settingType11Message", SettingType11MessageFilter);
}