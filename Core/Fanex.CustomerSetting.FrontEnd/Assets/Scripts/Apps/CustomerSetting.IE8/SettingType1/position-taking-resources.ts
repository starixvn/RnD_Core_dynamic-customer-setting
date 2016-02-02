/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class PositionTakingResourceService extends ResourceService {
        public static $inject: string[] = ["positionTakingResources", "parametersService"];

        constructor(positionTakingResources: any, parametersService: ParametersService) {
            super(positionTakingResources, parametersService);
        }
    }

    angular.module("customerSetting").service("positionTakingResourceService", PositionTakingResourceService);

    function PositionTakingLabelFilter(positionTakingResourceService: PositionTakingResourceService): Function {
        return (name: string) => {
            return positionTakingResourceService.getLabel(name);
        };
    }

    PositionTakingLabelFilter.$inject = ["positionTakingResourceService"];
    ModuleRegister.filter("positionTakingLabel", PositionTakingLabelFilter);

    function PositionTakingMessageFilter(positionTakingResourceService: PositionTakingResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            return positionTakingResourceService.getFormattedMessage.apply(positionTakingResourceService, [name, replacements]);
        };
    }

    PositionTakingMessageFilter.$inject = ["positionTakingResourceService"];
    ModuleRegister.filter("positionTakingMessage", PositionTakingMessageFilter);
}