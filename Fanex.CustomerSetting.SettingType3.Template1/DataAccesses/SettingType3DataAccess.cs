namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class SettingType3DataAccess : BaseDataAccess, ISettingDataAccess
    {
        public SettingType GetCustomerSettingType(
            User actor, 
            SettingQuery settingQuery)
        {
            throw new NotImplementedException();
        }

        public IResult UpdateCustomerSetting(
            ISettingContext settingContext, 
            Customer customer, 
            SettingType settingType, 
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException();
        }
    }
}
