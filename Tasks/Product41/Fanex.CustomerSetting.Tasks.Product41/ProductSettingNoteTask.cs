namespace Fanex.CustomerSetting.Tasks.Product41
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductSettingNoteTask : IPostQueryCustomerSettingTask
    {
        private readonly int BBinProductId = 241;

        private readonly int SettingType17 = 17;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            List<string> messages = new List<string>();
            if (setting.SettingTypes.FirstOrDefault().SettingItems.Any(item => (item as SettingItemDefault)?.SportTypeId == BBinProductId))
            {
                messages.Add(Resource.BBinSettingNote);
            }

            if (setting.CustomerLevel.IsAgentLevel() && setting.SettingTypes.Any(item => item.Id == SettingType17))
            {
                messages.Add(Resource.AgentStakeLevelWarning);
            }

            setting.AddWarningMessages(messages.ToArray());
        }
    }
}
