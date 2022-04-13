namespace Fanex.CustomerSetting.Tasks.SettingType7
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using System;

    public class BetSettingItemNoteTask : IPostQueryCustomerSettingTypeTask
    {
        private readonly int[] listSporttypeShowNoteIcon = new int[] { 245 };

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            foreach (SettingItemDefault settingItem in settingTypeData.SettingItems)
            {
                if (Array.IndexOf(listSporttypeShowNoteIcon, settingItem.SportTypeId) > -1)
                {
                    settingItem.ShowIconTooltipMessage = true;
                    settingItem.TooltipMessage = Resources.BetSettingNoteMessage;
                }
            }
        }
    }
}
