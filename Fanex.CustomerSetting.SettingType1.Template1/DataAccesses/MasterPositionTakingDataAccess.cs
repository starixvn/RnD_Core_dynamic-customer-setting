namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Models;

    public class MasterPositionTakingDataAccess : BasePositionTakingDataAccess
    {
        public override SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery)
        {
            return base.GetCustomerSettingType(actor, settingQuery);
        }
    }
}