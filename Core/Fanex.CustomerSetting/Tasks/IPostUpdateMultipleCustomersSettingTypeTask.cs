namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Models;

    public interface IPostUpdateMultipleCustomersSettingTypeTask
    {
        void Invoke(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingTypeData,
            IEnumerable<IResult> updateResults);
    }
}