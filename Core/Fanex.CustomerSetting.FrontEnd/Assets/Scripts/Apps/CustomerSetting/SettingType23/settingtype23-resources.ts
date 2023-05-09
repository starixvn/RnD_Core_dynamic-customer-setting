/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType23ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType23Resources", "parametersService"];

        constructor(settingType23Resources: any, parametersService: ParametersService) {
            super(settingType23Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType23ResourceService", SettingType23ResourceService);

    function SettingType23LabelFilter(settingType23ResourceService: SettingType23ResourceService): Function {
        return (name: string) => {
            return settingType23ResourceService.getLabel(name);
        };
    }

    SettingType23LabelFilter.$inject = ["settingType23ResourceService"];
    ModuleRegister.filter("settingType23Label", SettingType23LabelFilter);

    function SettingType23MessageFilter(settingType23ResourceService: SettingType23ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType23ResourceService.getFormattedMessage.apply(SettingType23ResourceService, replacements);
        };
    }

    SettingType23MessageFilter.$inject = ["settingType23ResourceService"];
    ModuleRegister.filter("settingType23Message", SettingType23MessageFilter);
}