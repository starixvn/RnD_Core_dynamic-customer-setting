namespace Fanex.CustomerSetting.Tasks.SettingType2
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using System;
    using System.Linq;

    public class BetSettingItemNoteTask : IPostQueryCustomerSettingTypeTask
    {
        private readonly int[] listSporttypeShowNoteIcon = new[] { 169 };

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(settingTypeData, nameof(settingTypeData));
            foreach (SettingItemDefault settingItem in settingTypeData.SettingItems.OfType<SettingItemDefault>())
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
