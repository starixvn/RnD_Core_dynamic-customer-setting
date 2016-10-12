/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class SettingType12ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType12Resource", "parametersService"];

        constructor(settingType12Resource: any, parametersService: ParametersService) {
            super(settingType12Resource, parametersService);
        }
    }

    ModuleRegister.service("settingType12ResourceService", SettingType12ResourceService);

    function SettingType12LabelFilter(settingType12ResourceService: SettingType12ResourceService): Function {
        return (name: string) => {
            return settingType12ResourceService.getLabel(name);
        };
    }

    SettingType12LabelFilter.$inject = ["settingType12ResourceService"];
    ModuleRegister.filter("settingType12Label", SettingType12LabelFilter);

    function SettingType12MessageFilter(settingType12ResourceService: SettingType12ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType12ResourceService.getFormattedMessage.apply(settingType12ResourceService, replacements);
        };
    }

    SettingType12MessageFilter.$inject = ["settingType12ResourceService"];
    ModuleRegister.filter("settingType12Message", SettingType12MessageFilter);
}