/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType16ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType16Resources", "parametersService"];

        constructor(settingType16Resources: any, parametersService: ParametersService) {
            super(settingType16Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType16ResourceService", SettingType16ResourceService);

    function SettingType16LabelFilter(settingType16ResourceService: SettingType16ResourceService): Function {
        return (name: string) => {
            return settingType16ResourceService.getLabel(name);
        };
    }

    SettingType16LabelFilter.$inject = ["settingType16ResourceService"];
    ModuleRegister.filter("settingType16Label", SettingType16LabelFilter);

    function SettingType16MessageFilter(settingType16ResourceService: SettingType16ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType16ResourceService.getFormattedMessage.apply(SettingType16ResourceService, replacements);
        };
    }

    SettingType16MessageFilter.$inject = ["settingType16ResourceService"];
    ModuleRegister.filter("settingType16Message", SettingType16MessageFilter);
}