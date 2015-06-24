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
        public SettingPreTasks(ISettingContext settingContext)
        {
            SettingContext = settingContext;
        }

        protected ISettingContext SettingContext { get; set; }

        public virtual void PreGetCustomerSettingType(QuerySettingParams queryParams)
        {
            throw new NotImplementedException("Pre-Get Customer Setting Type is not implemented");
        }

        public virtual IResult PreUpdateCustomerSettingType(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Customer Setting Type is not implemented");
        }

        public virtual PreUpdateMultipleCustomersSettingTypeResult PreUpdateMultipleCustomersSettingType(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Multiple Customer Setting Type is not implemented");
        }
    }
}