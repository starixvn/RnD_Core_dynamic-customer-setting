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
            this.settingTypeId = this.querySettingMatrix.settingTypeId;

            if (this.querySettingMatrix.showFilter) {
                this.queryProductNames();
            } else {
                this.loadSettingMatrix();
            }
        }

        public updateSettingTypeView(settingTypeId: number) {
            if (this.querySettingMatrix.settingTypeId !== settingTypeId) {
                this.querySettingMatrix.settingTypeId = settingTypeId;
                this.settingTypeId = settingTypeId;
                this.customerLine = [];

                this.queryProductNames();
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

                        this.products.forEach(p => {
                            if (p.id == this.querySettingMatrix.productId) {
                                this.product = p;
                            }
                        });

                        if (this.product == null || this.product.id == 0) {
                            this.product = this.products[0];
                        }

                        this.querySettingMatrix.productId = this.product.id;
                        this.loadSettingMatrix();
                    }
                })
                .finally(() => {
                    this.isUIblocked = false;
                });
        }

        public loadSettingMatrix(): void {
            if (this.querySettingMatrix.customerName === "") {
                return;
            }

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
                        switch (this.querySettingMatrix.settingTypeId) {
                            case SettingType.PositionTaking.id:
                                this.settingTypeMatrix = new PTMatrix(customerSettingMatrix.settingTypeMatrix);
                                break;
                            case SettingType.Commission.id:
                            case SettingType.ChargeCommission.id:
                            case SettingType.TicketCommission.id:
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

                    if (!this.product) {
                        this.product = new Product(
                            {
                                id: this.settingTypeMatrix.productId,
                                name: this.settingTypeMatrix.productName
                            });
                    }
                });
        }

        private handleError(errorMessage: string) {
            this.errorMessage = errorMessage;
            this.customerLine = [];
        }
    }

    ModuleRegister.controller("customerSettingMatrixController", CustomerSettingMaxtrixController);
}