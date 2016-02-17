/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />
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

    function ProductStatusLabelFilter(productStatusResourceService: ProductStatusResourceService): Function {
        return (name: string) => {
            return productStatusResourceService.getLabel(name);
        };
    }

    ProductStatusLabelFilter.$inject = ["productStatusResourceService"];
    ModuleRegister.filter("productStatusLabel", ProductStatusLabelFilter);

    function ProductStatusMessageFilter(productStatusResourceService: ProductStatusResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return productStatusResourceService.getFormattedMessage.apply(productStatusResourceService, replacements);
        };
    }

    ProductStatusMessageFilter.$inject = ["productStatusResourceService"];
    ModuleRegister.filter("productStatusMessage", ProductStatusMessageFilter);
}