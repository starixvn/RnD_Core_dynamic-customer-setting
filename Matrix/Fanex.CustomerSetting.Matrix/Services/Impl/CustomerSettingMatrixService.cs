namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using Common.Helper;
    using Criteria;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Fanex.CustomerSetting.DataAccesses;
    using Models;
    using Models.Enumerations;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.NamingRules",
        "SA1305:FieldNamesMustNotUseHungarianNotation",
        Justification = "PT is abbriviation for Position Taking.")]
    public class CustomerSettingMatrixService : ICustomerSettingMatrixService
    {
        private const string SettingTypeLevelCacheKey = "Fanex.CustomerSetting.Matrix.SettingTypeLevelCacheKey.{0}.{1}.{2}";
        private const int CacheDuration = 1; //// 1 hour
        private const int SportsBookProductId = 1;
        private const int SportTypeSettingLevel = 2;

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

        public CustomerSettingMatrix GetCustomerSettingMatrix(SettingMatrixQuery settingMatrixQuery, byte SettingStatus)
        {
            Check.Argument.IsNotNull(settingMatrixQuery, nameof(settingMatrixQuery));
            Check.Argument.IsNotEmpty(settingMatrixQuery.CustomerName, nameof(settingMatrixQuery.CustomerName));
#pragma warning disable S109 // Magic numbers should not be used
            Check.Argument.IsNotOutOfRange(settingMatrixQuery.ViewMode, 1, 3, nameof(settingMatrixQuery.ViewMode));
#pragma warning restore S109 // Magic numbers should not be used
            var customerLine = _customerService.GetCustomerLine(settingMatrixQuery.CustomerName);
            var settingTypeMatrix = GetCustomerSettingTypeMatrix(settingMatrixQuery, customerLine, SettingStatus);
            settingTypeMatrix.SettingLevel = GetSettingTypeLevel(settingMatrixQuery, GetSettingTypeId(settingMatrixQuery));

            if (settingMatrixQuery.ProductId > 0)
            {
                settingTypeMatrix.ProductId = settingMatrixQuery.ProductId;
                settingTypeMatrix.ProductName = _resourceService.GetProductName(settingMatrixQuery.ProductId, null);
            }

            return new CustomerSettingMatrix
            {
                CustomerLine = customerLine,
                SettingTypeMatrix = settingTypeMatrix
            };
        }

        private SettingTypeMatrix GetCustomerSettingTypeMatrix(SettingMatrixQuery settingMatrixQuery, IEnumerable<Customer> customerLine, byte SettingStatus)
        {
            switch ((ViewMode)settingMatrixQuery.ViewMode)
            {
                case ViewMode.PositionTaking:
                    return _pTMatrixService.GetPTMatrix(settingMatrixQuery, GetMasterName(customerLine));

                case ViewMode.Commission:
                    return _commissionMatrixService.GetCommissionMatrix(settingMatrixQuery.ProductId, settingMatrixQuery.CustomerName, SettingStatus);

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

        private byte GetSettingTypeLevel(SettingMatrixQuery settingMatrixQuery, short settingTypeId)
        {
            if (settingMatrixQuery.ProductId == SportsBookProductId)
            {
                return SportTypeSettingLevel;
            }

            var settingTypeLevel = _cachingService.GetOrAdd(
                   string.Format(SettingTypeLevelCacheKey, settingMatrixQuery.ProductId, settingMatrixQuery.SportTypeId, settingTypeId),
                   () =>
                   {
                       return _dynamicRepository.Get<byte>(
                            new GetSettingTypeLevelCriteria
                            {
                                ProductId = settingMatrixQuery.ProductId,
                                SettingTypeId = settingTypeId,
                                SportTypeId = settingMatrixQuery.SportTypeId
                            });
                   },
                   new TimeSpan(CacheDuration, 0, 0));

            return settingTypeLevel;
        }

        private short GetSettingTypeId(SettingMatrixQuery settingMatrixQuery)
        {
            var criteria = new GetSettingTypeIdCriteria
            {
                ProductId = settingMatrixQuery.ProductId == 0 ? (byte?)null : settingMatrixQuery.ProductId,
                SportTypeId = settingMatrixQuery.SportTypeId == 0 ? (short?)null : settingMatrixQuery.SportTypeId,
                Mode = settingMatrixQuery.ViewMode
            };
            using (var objDb = ObjectDbFactory.ObjectDb(criteria.GetSettingKey()))
            {
                objDb.ExecuteNonQuery(criteria.GetSpParams());
                var output = objDb.GetOuputParams();
                return (short)output["settingtype"];
            }
        }
    }
}