namespace Fanex.CustomerSetting.Tasks.SettingType22
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;

    public class SettingType22NoteTask : IPostQueryCustomerSettingTask
    {
        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            setting.AddWarningMessages(Resources.SettingType22WarningMsg);
        }
    }
}
