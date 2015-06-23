namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;

    public class MasterPositionTakingDataAccess : BasePositionTakingDataAccess
    {
        public override SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery)
        {
            var masterSettingType = base.GetCustomerSettingType(actor, settingQuery);

            return masterSettingType;
        }
    }
}