namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Results;

    public interface IPostUpdateMultipleCustomersSettingTypeTask
    {
        void Invoke(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingTypeData,
            IEnumerable<SettingResult> updateResults);
    }
}