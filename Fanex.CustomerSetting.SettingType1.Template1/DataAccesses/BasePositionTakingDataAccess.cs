namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class BasePositionTakingDataAccess : ISettingDataAccess
    {
        public virtual SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery)
        {
            throw new NotImplementedException();
        }

        public virtual IResult UpdateCustomerSetting(UpdateCustomerSettingParams updateCustomerSettingParams)
        {
            throw new NotImplementedException();
        }
    }
}