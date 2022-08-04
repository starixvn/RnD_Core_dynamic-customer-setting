/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType21ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType21Resources", "parametersService"];

        constructor(settingType21Resources: any, parametersService: ParametersService) {
            super(settingType21Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType21ResourceService", SettingType21ResourceService);

    function SettingType21LabelFilter(settingType21ResourceService: SettingType21ResourceService): Function {
        return (name: string) => {
            return settingType21ResourceService.getLabel(name);
        };
    }

    SettingType21LabelFilter.$inject = ["settingType21ResourceService"];
    ModuleRegister.filter("settingType21Label", SettingType21LabelFilter);

    function SettingType21MessageFilter(settingType21ResourceService: SettingType21ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType21ResourceService.getFormattedMessage.apply(settingType21ResourceService, replacements);
        };
    }

    SettingType21MessageFilter.$inject = ["settingType21ResourceService"];
    ModuleRegister.filter("settingType21Message", SettingType21MessageFilter);
}