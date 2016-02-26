namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Models.Enumerations;
    using Fanex.CustomerSetting.Matrix.Resources;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.Data.Repository;

    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", 
        "SA1305:FieldNamesMustNotUseHungarianNotation", 
        Justification = "PT is abbriviation for Position Taking.")]
    public class CustomerSettingMatrixService : ICustomerSettingMatrixService
    {
        private const string SettingTypeLevelCacheKey = "Fanex.CustomerSetting.Matrix.SettingTypeLevelCacheKey.{0}.{1}.{2}";
        private const int CacheDuration = 1; //// 1 hour

        private readonly ICustomerService _customerService;
        private readonly IPTMatrixService _pTMatrixService;
        private readonly ICommissionMatrixService _commissionMatrixService;
        private readonly IResourceService _resourceService;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly ICachingService _cachingService;

        public CustomerSettingMatrixService(
            ICustomerService customerService = null,
            IPTMatrixService pTMatrixService = null,
            ICommissionMatrixService commissionMatrixService = null,
            IResourceService resourceService = null,
            ICachingService cachingService = null,
            IDynamicRepository dynamicRepository = null)
        {
            _resourceService = resourceService ?? new ResourceService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _cachingService = cachingService ?? new CachingService();

            _customerService = customerService ?? new CustomerService(_dynamicRepository);
            _pTMatrixService = pTMatrixService ?? new PTMatrixService(_resourceService, _dynamicRepository);
            _commissionMatrixService = commissionMatrixService ?? new CommissionMatrixService(_resourceService, _dynamicRepository);
        }

        public CustomerSettingMatrix GetCustomerSettingMatrix(SettingMatrixQuery settingMatrixQuery)
        {
            Check.Argument.IsNotNull(settingMatrixQuery, "settingMatrixQuery");
            Check.Argument.IsNotNegativeOrZero(settingMatrixQuery.SettingTypeId, "settingMatrixQuery.SettingTypeId");
            Check.Argument.IsNotEmpty(settingMatrixQuery.CustomerName, "settingMatrixQuery.CustomerName");

            var customerLine = _customerService.GetCustomerLine(settingMatrixQuery.CustomerName);
            var settingTypeMatrix = GetCustomerSettingTypeMatrix(settingMatrixQuery, customerLine);
            settingTypeMatrix.SettingLevel = GetSettingTypeLevel(settingMatrixQuery);

            return new CustomerSettingMatrix
            {
                CustomerLine = customerLine,
                SettingTypeMatrix = settingTypeMatrix
            };
        }

        private SettingTypeMatrix GetCustomerSettingTypeMatrix(SettingMatrixQuery settingMatrixQuery, IEnumerable<Customer> customerLine)
        {
            switch ((SettingType)settingMatrixQuery.SettingTypeId)
            {
                case SettingType.PositionTaking:
                    return _pTMatrixService.GetPTMatrix(settingMatrixQuery, GetMasterName(customerLine));

                case SettingType.Commission:
                case SettingType.ChargeCommission:
                    return _commissionMatrixService.GetCommissionMatrix(settingMatrixQuery.ProductId, settingMatrixQuery.CustomerName);

                default:
                    throw new InvalidOperationException(Message.InvalidSettingTypeId);
            }
        }

        private string GetMasterName(IEnumerable<Customer> customerLine)
        {
            foreach (var customer in customerLine)
            {
                if (customer.Level.IsMasterLevel())
                {
                    return customer.Name;
                }
            }

            return string.Empty;
        }

        private byte GetSettingTypeLevel(SettingMatrixQuery settingMatrixQuery)
        {
            var settingTypeLevel = _cachingService.GetOrAdd(
                   string.Format(SettingTypeLevelCacheKey, settingMatrixQuery.ProductId, settingMatrixQuery.SportTypeId, settingMatrixQuery.SettingTypeId),
                   () =>
                   {
                       return _dynamicRepository.Get<byte>(
                            new GetSettingTypeLevelCriteria
                            {
                                ProductId = settingMatrixQuery.ProductId,
                                SettingTypeId = settingMatrixQuery.SettingTypeId,
                                SportTypeId = settingMatrixQuery.SportTypeId
                            });
                   },
                   new TimeSpan(CacheDuration, 0, 0));

            return settingTypeLevel;
        }
    }
}