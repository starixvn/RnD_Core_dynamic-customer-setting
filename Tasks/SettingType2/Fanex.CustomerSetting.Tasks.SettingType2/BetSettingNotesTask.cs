namespace Fanex.CustomerSetting.Tasks.SettingType2
{
    using System.Collections.Generic;
    using Common.Helper;
    using Models;
    using Models.Requests;
    using Models.Results;

    public class BetSettingNotesTask : IPostQueryCustomerSettingTask
    {
        public const int SMANoteCode = -7101;
        public const int MemberNoteCode = -7102;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            var note = query.CustomerLevel.IsMemberLevel()
                ? new Result(SMANoteCode, Resources.MemberNote)
                : new Result(SMANoteCode, Resources.SMANote);

            if (setting.WarningMessage != null)
            {
                var warningMessages = new List<IResult>
                    {
                        setting.WarningMessage,
                        note
                    };

                setting.WarningMessage = new UpdateSettingResult(warningMessages);
            }
            else
            {
                setting.WarningMessage = note;
            }
        }
    }
}