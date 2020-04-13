/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class SettingType17ResourceService extends ResourceService {
        public static $inject: string[] = ["settingType17Resource", "parametersService"];

        constructor(settingType17Resource: any, parametersService: ParametersService) {
            super(settingType17Resource, parametersService);
        }
    }

    ModuleRegister.service("settingType17ResourceService", SettingType17ResourceService);

    function SettingType17LabelFilter(settingType17ResourceService: SettingType17ResourceService): Function {
        return (name: string) => {
            return settingType17ResourceService.getLabel(name);
        };
    }

    SettingType17LabelFilter.$inject = ["settingType17ResourceService"];
    ModuleRegister.filter("settingType17Label", SettingType17LabelFilter);

    function SettingType17MessageFilter(settingType17ResourceService: SettingType17ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return settingType17ResourceService.getFormattedMessage.apply(settingType17ResourceService, replacements);
        };
    }

    SettingType17MessageFilter.$inject = ["settingType17ResourceService"];
    ModuleRegister.filter("settingType17Message", SettingType17MessageFilter);
}