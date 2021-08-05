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
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            string note = string.Empty;

            if (SystemType.IsY7(settingContext.System))
            {
                if (query.CustomerLevel.IsNotMemberLevel())
                {
                    note = Resources.Y7SMANote;
                }

                setting.AddWarningMessages(note);
                return;
            }

            note = query.CustomerLevel.IsMemberLevel() ? Resources.MemberNote : Resources.SMANote;

            setting.AddWarningMessages(note);
        }
    }
}