namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System.Collections.Generic;
    using Enumerations;

    internal interface IPreMultipleUpdateSettingTypeTask
    {
        void Invoke(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingTypeData);
    }
}