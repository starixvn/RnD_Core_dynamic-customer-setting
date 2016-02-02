/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class CommissionResourceService extends ResourceService {
        public static $inject: string[] = ["commissionResources", "parametersService"];

        constructor(commissionResources: any, parametersService: ParametersService) {
            super(commissionResources, parametersService);
        }
    }

    ModuleRegister.service("commissionResourceService", CommissionResourceService);

    function CommissionLabelFilter(commissionResourceService: CommissionResourceService): Function {
        return (name: string) => {
            return commissionResourceService.getLabel(name);
        };
    }

    CommissionLabelFilter.$inject = ["commissionResourceService"];
    ModuleRegister.filter("commissionLabel", CommissionLabelFilter);

    function CommissionMessageFilter(commissionResourceService: CommissionResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            return commissionResourceService.getFormattedMessage.apply(commissionResourceService, [name, replacements]);
        };
    }

    CommissionMessageFilter.$inject = ["commissionResourceService"];
    ModuleRegister.filter("commissionMessage", CommissionMessageFilter);
}