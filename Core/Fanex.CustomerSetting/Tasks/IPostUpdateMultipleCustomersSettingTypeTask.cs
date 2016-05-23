namespace Fanex.CustomerSetting.Tasks
{
    using System.Collections.Generic;
    using Models;
    using Models.Enumerations;
    using Models.Results;

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