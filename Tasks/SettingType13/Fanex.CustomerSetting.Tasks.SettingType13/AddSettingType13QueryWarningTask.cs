namespace Fanex.CustomerSetting.Tasks.SettingType13
{
    using Common.Helper;
    using Models;
    using Models.Constants;
    using Models.Requests;
    using Models.Results;

    public class AddSettingType13QueryWarningTask : AddSettingType13WarningTask, IPostQueryCustomerSettingTask
    {
        internal const int ProductStatusNoteForExternalSitesCode = -9102;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if ((query.CustomerLevel.IsMasterLevel() || query.CustomerLevel.IsAgentLevel())
                && FilterSettingType13OnExternalSites(settingContext, setting) != null)
            {
                setting.WarningMessage = new Result(ProductStatusNoteForExternalSitesCode, Resources.ProductStatusNoteForExternalSites);
            }
        }
    }
}