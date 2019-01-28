namespace Fanex.CustomerSetting.Tasks.Product21
{
    using System.Linq;
    using Common.Helper;
    using Models;
    using Models.Requests;

    public class AddSettingNoteTask : IPostQueryCustomerSettingTask
    {
        public static readonly short[] TargetSettingTypes = { 13, 7 };

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if (setting.SettingTypes.Any(settingType => TargetSettingTypes.Contains(settingType.Id)))
            {
                setting.AddWarningMessages(Resources.GeneralSettingNote, Resources.BetSettingAndProductStatusSettingNote);
            }
        }
    }
}