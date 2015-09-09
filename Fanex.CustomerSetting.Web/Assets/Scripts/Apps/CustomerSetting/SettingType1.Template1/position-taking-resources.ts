/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/parameters.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class PositionTakingResourceService extends ResourceService {
        public static $inject: string[] = ["positionTakingResources", "parametersService"];

        constructor(positionTakingResources: any, parametersService: ParametersService) {
            super(positionTakingResources, parametersService);
        }
    }

    angular.module("customerSetting").service("positionTakingResourceService", PositionTakingResourceService);

    export function PositionTakingLabelFilter(positionTakingResourceService: PositionTakingResourceService) {
        return (name: string) => {
            return positionTakingResourceService.getLabel(name);
        };
    }

    PositionTakingLabelFilter.$inject = ["positionTakingResourceService"];
    ModuleRegister.filter("positionTakingLabel", PositionTakingLabelFilter);

    export function PositionTakingMessageFilter(positionTakingResourceService: PositionTakingResourceService) {
        return (name: string, ...replacements: any[]) => {
            return positionTakingResourceService.getFormattedMessage.apply(positionTakingResourceService, [name, replacements]);
        };
    }

    PositionTakingMessageFilter.$inject = ["positionTakingResourceService"];
    ModuleRegister.filter("positionTakingMessage", PositionTakingMessageFilter);
}