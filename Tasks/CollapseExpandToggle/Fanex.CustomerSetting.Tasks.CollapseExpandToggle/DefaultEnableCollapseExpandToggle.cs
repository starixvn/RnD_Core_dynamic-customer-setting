namespace Fanex.CustomerSetting.Tasks.CollapseExpandToggle
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using System.Linq;

    public class DefaultEnableCollapseExpandToggle : IPostQueryCustomerSettingTask
    {
        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            setting.CustomerInfo.IsEnableCollapseExpand = true;
            foreach (var settingType in setting.SettingTypes)
            {
                settingType.IsEnableCollapseExpand = true;
            }
        }
    }
}
