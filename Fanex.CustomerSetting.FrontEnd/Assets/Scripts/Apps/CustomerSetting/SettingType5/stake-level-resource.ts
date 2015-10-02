/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class StakeLevelResourceService extends ResourceService {
        public static $inject: string[] = ["stakeLevelResources", "parametersService"];

        constructor(stakeLevelResources: any, parametersService: ParametersService) {
            super(stakeLevelResources, parametersService);
        }
    }

    angular.module("customerSetting").service("stakeLevelResourceService", StakeLevelResourceService);

    function StakeLevelLabelFilter(stakeLevelResourceService: StakeLevelResourceService): Function {
        return (name: string) => {
            return stakeLevelResourceService.getLabel(name);
        };
    }

    StakeLevelLabelFilter.$inject = ["stakeLevelResourceService"];
    ModuleRegister.filter("stakeLevelLabel", StakeLevelLabelFilter);

    function StakeLevelMessageFilter(stakeLevelResourceService: StakeLevelResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            return stakeLevelResourceService.getFormattedMessage.apply(stakeLevelResourceService, [name, replacements]);
        };
    }

    StakeLevelMessageFilter.$inject = ["stakeLevelResourceService"];
    ModuleRegister.filter("stakeLevelMessage", StakeLevelMessageFilter);
}