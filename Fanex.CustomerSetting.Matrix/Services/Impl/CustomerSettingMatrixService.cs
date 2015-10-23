namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Models.Enumerations;
    using Fanex.CustomerSetting.Matrix.Resources;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.Data.Repository;

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

        public CustomerSettingMatrix GetCustomerSettingMatrix(SettingMatrixQuery settingMatrixQuery)
        {
            Check.Argument.IsNotNull(settingMatrixQuery, "settingMatrixQuery");
            Check.Argument.IsNotNegativeOrZero(settingMatrixQuery.SettingTypeId, "settingMatrixQuery.SettingTypeId");
            Check.Argument.IsNotEmpty(settingMatrixQuery.CustomerName, "settingMatrixQuery.CustomerName");

            var customerSettingMatrix = new CustomerSettingMatrix
            {
                CustomerLine = _customerService.GetCustomerLine(settingMatrixQuery.CustomerName),
                SettingTypeMatrix = GetCustomerSettingTypeMatrix(settingMatrixQuery)
            };

            return customerSettingMatrix;
        }

        private SettingTypeMatrix GetCustomerSettingTypeMatrix(SettingMatrixQuery settingMatrixQuery)
        {
            switch ((SettingType)settingMatrixQuery.SettingTypeId)
            {
                case SettingType.PositionTaking:
                    return _pTMatrixService.GetPTMatrix(settingMatrixQuery);

                case SettingType.Commission:
                    return _commissionMatrixService.GetCommissionMatrix(settingMatrixQuery.ProductId, settingMatrixQuery.CustomerName);

                default:
                    throw new InvalidOperationException(Message.InvalidSettingTypeId);
            }
        }
    }
}