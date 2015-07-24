namespace Fanex.CustomerSetting.SettingType3.Template1.Services
{
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType3.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.Logging;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService
    {
        private readonly IDataAccess _commissionDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        public SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess commissionDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _commissionDataAccess = commissionDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, QueryCustomerSettingParam queryParam, SettingType productSettingType)
        {
            var commissionItemsRawData = _commissionDataAccess.GetCustomerCommissionItemsRawData(queryParam);

            var customerCommission = new SettingType
            {
                Name = Label.Commission,
                SettingItems = commissionItemsRawData.Select<CommissionItemDto, CommissionItem>(commItemDto => new CommissionItem(commItemDto))
            };

            return SetSportTypeBetTypeName(customerCommission, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Enumerations.Level customerLevel, Level uplineLevel, byte productId, SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, "settingType");

            var result = _commissionDataAccess.UpdateCustomerCommission(settingContext, customerId, customerLevel, uplineLevel, productId, settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(settingResult, Message.UpdateCommissionSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}