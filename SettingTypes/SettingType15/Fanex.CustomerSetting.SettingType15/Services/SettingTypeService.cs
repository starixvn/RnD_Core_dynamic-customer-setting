namespace Fanex.CustomerSetting.SettingType15.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Constants;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using DataAccesses;
    using Dtos;
    using Logging;
    using Models;
    using Resources;

    public class SettingTypeService : SettingTypeServiceBase
    {
        private readonly IDataAccess _winningShareDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess winningShareDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _winningShareDataAccess = winningShareDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));

            var customerWinningShare = new SettingType
            {
                Name = Label.WinningShareCommission,
                DisplayMode = DisplayModes.ReadOnly
            };

            if (query.CustomerLevel.IsNotMemberLevel())
            {
                IEnumerable<WinningShareDto> winningShareDtos = _winningShareDataAccess.GetCustomerWinningShare(query);

                customerWinningShare.SettingItems = winningShareDtos.Select(winningShareDto => new WinningShareItem(winningShareDto));
            }

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerWinningShare, query);

            return SetSportTypeBetTypeName(customerWinningShare, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            throw new InvalidOperationException($"Setting Type 15 does not support {nameof(UpdateCustomerSettingType)}.");
        }
    }
}