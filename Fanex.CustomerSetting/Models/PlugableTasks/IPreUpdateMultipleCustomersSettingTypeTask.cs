namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models.Enumerations;

    internal interface IPreUpdateMultipleCustomersSettingTypeTask
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