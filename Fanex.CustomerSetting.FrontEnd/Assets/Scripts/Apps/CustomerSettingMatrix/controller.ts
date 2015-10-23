/// <reference path="../../typings/angularjs/angular.d.ts" />
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
        public querySettingMatrix: QuerySettingMatrix;

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
            /* tslint:disable:no-string-literal */
            this.customerName = this.$window["customerName"];
            var querySettingMatrix: any = this.$window["querySettingMatrix"];
            this.querySettingMatrix = new QuerySettingMatrix(
                querySettingMatrix.settingTypeId,
                querySettingMatrix.productId,
                querySettingMatrix.customerName,
                querySettingMatrix.sportTypeId,
                querySettingMatrix.betTypeId,
                querySettingMatrix.isLive,
                querySettingMatrix.showFilter);
            /* tslint:enable:no-string-literal */
            this.queryProductNames();
        }

        public updateSettingTypeView(settingTypeId: number) {
            if (this.querySettingMatrix.settingTypeId !== settingTypeId) {
                this.querySettingMatrix.settingTypeId = settingTypeId;
                this.customerLine = [];

                this.queryProductNames();
            }
        }

        public queryProductNames(): void {
            if (this.querySettingMatrix.settingTypeId == SettingType.Commission.id) {
                this.isUIblocked = true;

                this.$http
                    .post(this.appConfigService.resolveRootUrl(this.queryAllProducts), {})
                    .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                    var products = response.data;

                    if (products && products.length > 0) {
                        this.products = products.map((product: any) => new Product(product));
                        this.product = this.products[0];
                        this.loadSettingMatrix();
                    }
                })
                    .finally(() => {
                    this.isUIblocked = false;
                });
            }
            else {
                this.product = new Product({
                    id: 0,
                    name: ""
                });
                this.loadSettingMatrix();
            }
        }

        public loadSettingMatrix(): void {
            if (this.querySettingMatrix.customerName === "") {
                return;
            }

            this.querySettingMatrix.productId = this.product.id;
            this.errorMessage = "";
            this.isUIblocked = true;

            this.$http
                .post(this.appConfigService.resolveRootUrl(this.querySettingMatrixPath), this.querySettingMatrix)
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                var customerSettingMatrix = response.data;

                if (customerSettingMatrix.message != null) {
                    this.handleError(customerSettingMatrix.message);
                    return;
                }

                try {
                    this.customerLine = customerSettingMatrix.customerLine.map((customer: any) => new Customer(customer)).reverse();
                    switch (this.settingTypeId) {
                        case SettingType.PositionTaking.id:
                            this.settingTypeMatrix = new PTMatrix(customerSettingMatrix.settingTypeMatrix);
                            break;
                        case SettingType.Commission.id:
                            this.settingTypeMatrix = new CommissionMatrix(customerSettingMatrix.settingTypeMatrix);
                            break;
                    }
                    this.settingTypeMatrix.groupDataBySettingLevel();
                } /* tslint:disable:one-line */
                catch (e) {
                    this.handleError(this.resourceService.getMessage("SettingMatrixIsError"));
                    this.$window.console.log(e);
                }
                /* tslint:enable:one-line */
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