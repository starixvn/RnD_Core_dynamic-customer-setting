/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType9ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType9Resources", "parametersService"];

        constructor(settingType9Resources: any, parametersService: ParametersService) {
            super(settingType9Resources, parametersService);
        }
    }

    ModuleRegister.service("settingType9ResourceService", SettingType9ResourceService);

    function SettingType9LabelFilter(settingType9ResourceService: SettingType9ResourceService): Function {
        return (name: string) => {
            return settingType9ResourceService.getLabel(name);
        };
    }

    SettingType9LabelFilter.$inject = ["settingType9ResourceService"];
    ModuleRegister.filter("settingType9Label", SettingType9LabelFilter);

    function SettingType9MessageFilter(settingType9ResourceService: SettingType9ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType9ResourceService.getFormattedMessage.apply(SettingType9ResourceService, replacements);
        };
    }

    SettingType9MessageFilter.$inject = ["settingType9ResourceService"];
    ModuleRegister.filter("settingType9Message", SettingType9MessageFilter);
}