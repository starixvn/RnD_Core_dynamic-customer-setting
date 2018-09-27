namespace Fanex.CustomerSetting.Tasks.Product21
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Models;
    using Models.Requests;
    using Models.Results;

    public class AddSettingNoteTask : IPostQueryCustomerSettingTask
    {
        public const int SettingNoteCode = -131001;
        public static readonly short[] TargetSettingTypes = { 13, 7 };

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if (setting.SettingTypes.Any(settingType => TargetSettingTypes.Contains(settingType.Id)))
            {
                var notes = new List<IResult>
                {
                    new Result(SettingNoteCode, Resources.GeneralSettingNote),
                    new Result(SettingNoteCode, Resources.BetSettingAndProductStatusSettingNote)
                };

                if (setting.WarningMessage != null)
                {
                    notes.Insert(0, setting.WarningMessage);
                }

                setting.WarningMessage = new UpdateSettingResult(notes);
            }
        }
    }
}