namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class SettingPreTasks
    {
        public virtual void PreGetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam)
        {
            throw new NotImplementedException("Pre-Get Customer Setting Type is not implemented");
        }

        public virtual SettingResult PreUpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            SettingType settingType,
            Level uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Customer Setting Type is not implemented");
        }

        public virtual PreUpdateMultipleCustomersSettingTypeResult PreUpdateMultipleCustomersSettingType(
            ISettingContext settingContext, 
            IEnumerable<int> customerIds,
            Level customerLevel,
            SettingType settingType,
            Level uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Multiple Customer Setting Type is not implemented");
        }
    }
}