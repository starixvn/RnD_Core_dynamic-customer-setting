namespace Fanex.CustomerSetting.Tasks.Product33
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Models;
    using Models.Requests;
    using Models.Results;

    public class PTSettingNoteTask : IPostQueryCustomerSettingTask
    {
        public const int PTSettingNoteCode = -1001;
        public const short AffectedSettingType = 1;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if (setting.SettingTypes.Any(settingType => settingType.Id == AffectedSettingType))
            {
                var note = new Result(PTSettingNoteCode, Resources.PTAppliedForInsuranceAndGemsBought);

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
}