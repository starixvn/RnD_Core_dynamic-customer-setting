/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType15ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType15Resources", "parametersService"];

        constructor(settingType15Resources: any, parametersService: ParametersService) {
            super(settingType15Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType15ResourceService", SettingType15ResourceService);

    function SettingType15LabelFilter(settingType15ResourceService: SettingType15ResourceService): Function {
        return (name: string) => {
            return settingType15ResourceService.getLabel(name);
        };
    }

    SettingType15LabelFilter.$inject = ["settingType15ResourceService"];
    ModuleRegister.filter("settingType15Label", SettingType15LabelFilter);

    function SettingType15MessageFilter(settingType15ResourceService: SettingType15ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType15ResourceService.getFormattedMessage.apply(settingType15ResourceService, replacements);
        };
    }

    SettingType15MessageFilter.$inject = ["settingType15ResourceService"];
    ModuleRegister.filter("settingType15Message", SettingType15MessageFilter);
}