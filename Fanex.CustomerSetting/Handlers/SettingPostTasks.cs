namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Services;

    public class SettingPostTasks
    {
        protected readonly ISettingContext SettingContext;
        protected readonly IResourceService ResourceService;

        public SettingPostTasks(
            ISettingContext settingContext,
            IResourceService resourceService)
        {
            SettingContext = settingContext;
            ResourceService = resourceService;
        }

        public virtual SettingType PostGetCustomerSettingType(
            QuerySettingParams queryParams,
            SettingType settingType)
        {
            throw new NotImplementedException("Post-Get Customer Setting Type is not implemented");
        }

        protected SettingType MapSportTypeBetTypeName(SettingType settingType)
        {
            if (settingType != null && settingType.SettingItems.Any())
            {
                foreach (ISettingItem settingItem in settingType.SettingItems)
                {
                    if (settingItem is DefaultSettingItem)
                    {
                        var concreteSettingItem = settingItem as DefaultSettingItem;
                        concreteSettingItem.BetTypeName = ResourceService.GetBetTypeName(concreteSettingItem.BetTypeId, SettingContext.Language);
                        concreteSettingItem.SportTypeName = ResourceService.GetSportTypeName(concreteSettingItem.SportTypeId, SettingContext.Language);
                    }
                }
            }

            return settingType;
        }

        public virtual IResult PostUpdateCustomerSettingType(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Customer Setting Type is not implemented");
        }

        public virtual IResult PostUpdateMultipleCustomersSettingType(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Multiple Customers Setting is not implemented");
        }
    }
}