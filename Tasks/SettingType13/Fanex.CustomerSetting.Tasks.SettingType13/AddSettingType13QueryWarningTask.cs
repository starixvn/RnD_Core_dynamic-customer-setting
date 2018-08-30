namespace Fanex.CustomerSetting.Tasks.SettingType13
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;

    public class AddSettingType13QueryWarningTask : IPostQueryCustomerSettingTask
    {
        internal const int ProductStatusNoteForExternalSitesCode = -9102;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if ((query.CustomerLevel.IsMasterLevel() || query.CustomerLevel.IsAgentLevel())
                && FilterSettingType13OnAlpha(settingContext, setting) != null)
            {
                var note = new Result(ProductStatusNoteForExternalSitesCode, Resources.ProductStatusNoteForExternalSites);

                if (setting.WarningMessage != null)
                {
                    var warningMessages = new List<IResult>
                    {
                        note,
                        setting.WarningMessage
                    };

                    setting.WarningMessage = new UpdateSettingResult(warningMessages);
                }
                else
                {
                    setting.WarningMessage = note;
                }
            }
        }

        private SettingType FilterSettingType13OnAlpha(ISettingContext settingContext, ISetting setting)
        {
            if (settingContext.System == (byte)SystemType.Alpha || settingContext.System == (byte)SystemType.InternalAlpha)
            {
                return setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == Constants.SettingType13Id);
            }

            return null;
        }
    }
}