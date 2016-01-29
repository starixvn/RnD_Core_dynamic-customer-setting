namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Models;
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