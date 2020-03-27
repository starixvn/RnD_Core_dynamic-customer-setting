namespace Fanex.CustomerSetting.Tasks.SettingType9
{
    using Common.Helper;
    using Models;
    using Models.Enumerations;
    using Models.Requests;

    public class BetSettingNotesTask : IPostQueryCustomerSettingTask
    {
        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));

            if (SystemType.IsY7(settingContext.System))
            {
                return;
            }

            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            var note = query.CustomerLevel.IsMemberLevel() ? Resources.MemberNote : Resources.SMANote;

            setting.AddWarningMessages(note);
        }
    }
}