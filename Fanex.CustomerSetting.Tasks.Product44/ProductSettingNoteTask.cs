namespace Fanex.CustomerSetting.Tasks.Product41
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using System.Linq;

    public class ProductSettingNoteTask : IPostQueryCustomerSettingTask
    {
        private readonly int BBinProductId = 241;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            if (setting.SettingTypes.FirstOrDefault().SettingItems.Any(item => (item as SettingItemDefault)?.SportTypeId == BBinProductId))
            {
                setting.AddWarningMessages(Resource.BBinSettingNote);
            }
        }
    }
}
