/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType13ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType13Resources", "parametersService"];

        constructor(settingType13Resources: any, parametersService: ParametersService) {
            super(settingType13Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType13ResourceService", SettingType13ResourceService);

    function SettingType13LabelFilter(settingType13ResourceService: SettingType13ResourceService): Function {
        return (name: string) => {
            return settingType13ResourceService.getLabel(name);
        };
    }

    SettingType13LabelFilter.$inject = ["settingType13ResourceService"];
    ModuleRegister.filter("settingType13Label", SettingType13LabelFilter);

    function SettingType13MessageFilter(settingType13ResourceService: SettingType13ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType13ResourceService.getFormattedMessage.apply(settingType13ResourceService, replacements);
        };
    }

    SettingType13MessageFilter.$inject = ["settingType13ResourceService"];
    ModuleRegister.filter("settingType13Message", SettingType13MessageFilter);
}