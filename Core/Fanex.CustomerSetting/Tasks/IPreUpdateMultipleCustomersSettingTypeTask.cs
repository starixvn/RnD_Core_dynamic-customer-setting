namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;
    using Models;
    using Models.Enumerations;

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