/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="../core/parameters.ts" />
/// <reference path="../resource/resource-translation.ts" />

module CustomerSetting {
    "use strict";

    export class ProductStatusResourceService extends ResourceService {
        public static $inject: string[] = ["productStatusResources", "parametersService"];

        constructor(productStatusResources: any, parametersService: ParametersService) {
            super(productStatusResources, parametersService);
        }
    }

    ModuleRegister.service("productStatusResourceService", ProductStatusResourceService);

    function ProductStatusLabelFilter(productStatusResourceService: ProductStatusResourceService) {
        return (name: string) => {
            return productStatusResourceService.getLabel(name);
        };
    }

    LabelFilter.$inject = ["productStatusResourceService"];
    ModuleRegister.filter("productStatusLabel", ProductStatusLabelFilter);

    function ProductStatusMessageFilter(productStatusResourceService: ProductStatusResourceService) {
        return (name: string, ...replacements: any[]) => {
            return productStatusResourceService.getFormattedMessage.apply(productStatusResourceService, [name, replacements]);
        };
    }

    LabelFilter.$inject = ["productStatusResourceService"];
    ModuleRegister.filter("productStatusMessage", ProductStatusMessageFilter);
}