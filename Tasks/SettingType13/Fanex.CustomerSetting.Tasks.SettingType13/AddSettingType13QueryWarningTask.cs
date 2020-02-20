namespace Fanex.CustomerSetting.Tasks.SettingType13
{
    using Common.Helper;
    using Models;
    using Models.Enumerations;
    using Models.Requests;

    public class AddSettingType13QueryWarningTask : IPostQueryCustomerSettingTask
    {
        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if (CheckIfEditingCustomerIsMasterOrAgent(query) && SystemType.IsAlpha(settingContext.System))
            {
                setting.AddWarningMessages(Resources.ProductStatusNoteForExternalSites);
            }
        }

        private static bool CheckIfEditingCustomerIsMasterOrAgent(CustomerSettingQuery query)
        {
            return query.CustomerLevel.IsMasterLevel() || query.CustomerLevel.IsAgentLevel();
        }
    }
}