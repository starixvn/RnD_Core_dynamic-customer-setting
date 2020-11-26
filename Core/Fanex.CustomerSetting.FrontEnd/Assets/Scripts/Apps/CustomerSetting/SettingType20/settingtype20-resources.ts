/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType20ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType20Resources", "parametersService"];

        constructor(settingType20Resources: any, parametersService: ParametersService) {
            super(settingType20Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType20ResourceService", SettingType20ResourceService);

    function SettingType20LabelFilter(settingType20ResourceService: SettingType20ResourceService): Function {
        return (name: string) => {
            return settingType20ResourceService.getLabel(name);
        };
    }

    SettingType20LabelFilter.$inject = ["settingType20ResourceService"];
    ModuleRegister.filter("settingType20Label", SettingType20LabelFilter);

    function SettingType20MessageFilter(settingType20ResourceService: SettingType20ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType20ResourceService.getFormattedMessage.apply(SettingType20ResourceService, replacements);
        };
    }

    SettingType20MessageFilter.$inject = ["settingType20ResourceService"];
    ModuleRegister.filter("settingType20Message", SettingType20MessageFilter);
}