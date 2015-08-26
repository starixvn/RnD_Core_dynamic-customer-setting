/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="module-register.ts" />
/// <reference path="parameters.ts" />
/// <reference path="directives/ptmatrix/pt-matrix.ts" />
/// <reference path="directives/commissionmatrix/commission-matrix.ts" />
/// <reference path="resource/resource-translation.ts" />


module CustomerSettingMatrix {
    "use strict";

    class CustomerSettingMaxtrixController {
        public static $inject: string[] = [
            "$scope",
            "$http",
            "$timeout",
            "$window",
            "appConfigService",
            "resourceService"];

        public isUIblocked: boolean;
        public errorMessage: string;
        
        public product: Product;
        public customerName: string;
        public settingTypeId: number;

        public products: Array<Product>;
        public settingTypes: Array<SettingType>;

        public customerLine: Array<Customer>;
        public settingTypeMatrix: SettingTypeMatrix;
        

        private get querySettingMatrixPath(): string {
            return "ViewCustomerSettingMatrix/QuerySettingMatrix?";
        }

        private get queryAllProducts(): string {
            return "ViewCustomerSettingMatrix/QueryAllProducts";
        }

        private $scope: ng.IScope;
        private $http: ng.IHttpService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;
        private $window: ng.IWindowService;
        private resourceService: ResourceService;

        constructor(
            $scope: ng.IScope,
            $http: ng.IHttpService,
            $timeout: ng.ITimeoutService,
            $window: ng.IWindowService,
            appConfigService: AppConfigService,
            resourceService: ResourceService) {

            this.$scope = $scope;
            this.$http = $http;
            this.$timeout = $timeout;
            this.$window = $window;
            this.appConfigService = appConfigService;
            this.resourceService = resourceService;

            this.settingTypes = [SettingType.PositionTaking, SettingType.Commission];
            this.settingTypeId = SettingType.PositionTaking.id;
            this.customerName = this.$window["customerName"];
            this.queryProductNames();
        }

        public updateSettingTypeView(settingTypeId: number) {
            if (this.settingTypeId != settingTypeId) {
                this.settingTypeId = settingTypeId;
                this.customerLine = [];
            }
        }

        public queryProductNames(): void {
            this.isUIblocked = true;

            this.$http
                .post(this.appConfigService.resolveRootUrl(this.queryAllProducts), {})
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                var products = response.data;

                if (products && products.length > 0) {
                    this.products = products.map((product: any) => new Product(product));
                    this.product = this.products[0];
                    this.querySettingMatrix(this.customerName);
                }
            })
                .finally(() => {
                this.isUIblocked = false;
            });
        }

        public querySettingMatrix(customerName: string): void {
            if (customerName == "") {
                return;
            }

            var querySettingMatrix = new QuerySettingMatrix(
                this.settingTypeId,
                this.product.id,
                customerName);

            this.errorMessage = "";
            this.isUIblocked = true;

            this.$http
                .post(this.appConfigService.resolveRootUrl(this.querySettingMatrixPath), querySettingMatrix)
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                var customerSettingMatrix = response.data;

                if (customerSettingMatrix.message != null) {
                    this.handleError(customerSettingMatrix.message);
                    return;
                }

                try
                {
                    this.customerLine = customerSettingMatrix.customerLine.map((customer: any) => new Customer(customer));
                    switch (this.settingTypeId) {
                        case SettingType.PositionTaking.id:
                            this.settingTypeMatrix = new PTMatrix(customerSettingMatrix.settingTypeMatrix);
                            break;
                        case SettingType.Commission.id:
                            this.settingTypeMatrix = new CommissionMatrix(customerSettingMatrix.settingTypeMatrix);
                            break;
                    }
                    this.settingTypeMatrix.groupDataBySettingLevel();
                }
                catch (e) {
                    this.handleError(this.resourceService.getMessage("SettingMatrixIsError"));
                }
            })
                .finally(() => {
                this.isUIblocked = false;
            });
        }

        private handleError(errorMessage: string) {
            this.errorMessage = errorMessage;
            this.customerLine = [];
        }
    }

    ModuleRegister.controller("customerSettingMatrixController", CustomerSettingMaxtrixController);
}