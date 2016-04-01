/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType8ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType8Resource", "parametersService"];

        constructor(settingType8Resource: any, parametersService: ParametersService) {
            super(settingType8Resource, parametersService);
        }
    }

    ModuleRegister.service("settingType8ResourceService", SettingType8ResourceService);

    function SettingType8LabelFilter(settingType8ResourceService: SettingType8ResourceService): Function {
        return (name: string) => {
            return settingType8ResourceService.getLabel(name);
        };
    }

    SettingType8LabelFilter.$inject = ["settingType8ResourceService"];
    ModuleRegister.filter("settingType8Label", SettingType8LabelFilter);

    function SettingType8MessageFilter(settingType8ResourceService: SettingType8ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType8ResourceService.getFormattedMessage.apply(settingType8ResourceService, replacements);
        };
    }

    SettingType8MessageFilter.$inject = ["settingType8ResourceService"];
    ModuleRegister.filter("settingType8Message", SettingType8MessageFilter);
}