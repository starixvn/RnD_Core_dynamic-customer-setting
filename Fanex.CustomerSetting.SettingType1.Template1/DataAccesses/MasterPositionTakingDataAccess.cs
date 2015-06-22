namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;

    public class MasterPositionTakingDataAccess : BasePositionTakingDataAccess
    {
        public override SettingType GetCustomerSettingType(GetCustomerSettingTypeParams getParams)
        {
            var masterSettingType = base.GetCustomerSettingType(getParams);

            return masterSettingType;
        }
    }
}