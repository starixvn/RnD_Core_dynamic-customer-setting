/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType22ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType22Resource", "parametersService"];

        constructor(settingType22Resource: any, parametersService: ParametersService) {
            super(settingType22Resource, parametersService);
        }
    }

    ModuleRegister.service("settingType22ResourceService", SettingType22ResourceService);

    function SettingType22LabelFilter(settingType22ResourceService: SettingType22ResourceService): Function {
        return (name: string) => {
            return settingType22ResourceService.getLabel(name);
        };
    }

    SettingType22LabelFilter.$inject = ["settingType22ResourceService"];
    ModuleRegister.filter("settingType22Label", SettingType22LabelFilter);

    function SettingType22MessageFilter(settingType22ResourceService: SettingType22ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType22ResourceService.getFormattedMessage.apply(settingType22ResourceService, replacements);
        };
    }

    SettingType22MessageFilter.$inject = ["settingType22ResourceService"];
    ModuleRegister.filter("settingType22Message", SettingType22MessageFilter);
}