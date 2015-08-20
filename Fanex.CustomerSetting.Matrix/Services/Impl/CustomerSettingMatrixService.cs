namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Models.Enumerations;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "PT is abbriviation for Position Taking.")]
    public class CustomerSettingMatrixService : ICustomerSettingMatrixService
    {
        private readonly ICustomerService _customerService;
        private readonly IPTMatrixService _pTMatrixService;
        private readonly ICommissionMatrixService _commissionMatrixService;
        private readonly IResourceService _resourceService;
        private readonly IProductService _productService;
        private readonly IDynamicRepository _dynamicRepository;

        public CustomerSettingMatrixService(
            ICustomerService customerService = null,
            IPTMatrixService pTMatrixService = null,
            ICommissionMatrixService commissionMatrixService = null,
            IResourceService resourceService = null,
            IProductService productService = null,
            IDynamicRepository dynamicRepository = null)
        {
            _resourceService = resourceService ?? new ResourceService();
            _productService = productService ?? new ProductService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();

            _customerService = customerService ?? new CustomerService(_dynamicRepository);
            _pTMatrixService = pTMatrixService ?? new PTMatrixService(_resourceService, _productService, dynamicRepository);
            _commissionMatrixService = commissionMatrixService ?? new CommissionMatrixService(_resourceService, _productService, _dynamicRepository);
        }

        public CustomerSettingMatrix GetCustomerSettingMatrix(byte settingTypeId, byte productId, string customerName)
        {
            Check.Argument.IsNotNegativeOrZero(settingTypeId, "settingTypeId");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");
            Check.Argument.IsNotEmpty(customerName, "customerName");

            var customerSettingMatrix = new CustomerSettingMatrix
            {
                CustomerLine = _customerService.GetCustomerLine(customerName),
                SettingTypeMatrix = GetCustomerSettingTypeMatrix(settingTypeId, productId, customerName)
            };

            return customerSettingMatrix;
        }

        private SettingTypeMatrix GetCustomerSettingTypeMatrix(byte settingTypeId, byte productId, string customerName)
        {
            switch ((SettingType)settingTypeId)
            {
                case SettingType.PositionTaking:
                    return _pTMatrixService.GetPTMatrix(productId, customerName);

                case SettingType.Commission:
                    return _commissionMatrixService.GetCommissionMatrix(productId, customerName);

                default:
                    throw new InvalidOperationException("Invalid setting type id");
            }
        }
    }
}