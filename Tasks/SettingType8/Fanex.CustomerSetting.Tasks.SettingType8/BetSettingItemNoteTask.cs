namespace Fanex.CustomerSetting.Tasks.SettingType8
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.SettingType8.Models;
    using System;

    public class BetSettingItemNoteTask : IPostQueryCustomerSettingTypeTask
    {
        private readonly string msgTemplate = "BetSettingNoteMessage";

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            foreach (SettingItemDefault settingItem in settingTypeData.SettingItems)
            {
                var stakeLevelGroupItem = (StakeLevelGroupItem)settingItem;
                if (stakeLevelGroupItem.IsBuyBonus)
                {
                    settingItem.ShowIconTooltipMessage = true;
                    settingItem.TooltipMessage = Resources.ResourceManager.GetString(msgTemplate);
                }
            }
        }
    }
}
