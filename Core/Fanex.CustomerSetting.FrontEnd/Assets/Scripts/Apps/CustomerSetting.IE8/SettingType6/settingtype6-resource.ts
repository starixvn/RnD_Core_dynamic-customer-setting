/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType6ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType6Resource", "parametersService"];

        constructor(settingType6Resource: any, parametersService: ParametersService) {
            super(settingType6Resource, parametersService);
        }
    }

    ModuleRegister.service("settingType6ResourceService", SettingType6ResourceService);

    function SettingType6LabelFilter(settingType6ResourceService: SettingType6ResourceService): Function {
        return (name: string) => {
            return settingType6ResourceService.getLabel(name);
        };
    }

    SettingType6LabelFilter.$inject = ["settingType6ResourceService"];
    ModuleRegister.filter("settingType6Label", SettingType6LabelFilter);

    function SettingType6MessageFilter(settingType6ResourceService: SettingType6ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType6ResourceService.getFormattedMessage.apply(settingType6ResourceService, replacements);
        };
    }

    SettingType6MessageFilter.$inject = ["settingType6ResourceService"];
    ModuleRegister.filter("settingType6Message", SettingType6MessageFilter);
}