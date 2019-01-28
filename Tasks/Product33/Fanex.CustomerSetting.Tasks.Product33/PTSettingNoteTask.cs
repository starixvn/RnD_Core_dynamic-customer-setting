namespace Fanex.CustomerSetting.Tasks.Product33
{
    using System.Linq;
    using Common.Helper;
    using Models;
    using Models.Requests;

    public class PTSettingNoteTask : IPostQueryCustomerSettingTask
    {
        public const short TargetSettingType = 1;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if (setting.SettingTypes.Any(settingType => settingType.Id == TargetSettingType))
            {
                setting.AddWarningMessages(Resources.PTAppliedForInsuranceAndGemsBought);
            }
        }
    }
}