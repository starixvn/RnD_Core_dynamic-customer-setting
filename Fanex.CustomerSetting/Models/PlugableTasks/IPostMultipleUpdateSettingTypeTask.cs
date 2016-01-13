namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System.Collections.Generic;
    using Enumerations;
    using Results;

    public interface IPostMultipleUpdateSettingTypeTask
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