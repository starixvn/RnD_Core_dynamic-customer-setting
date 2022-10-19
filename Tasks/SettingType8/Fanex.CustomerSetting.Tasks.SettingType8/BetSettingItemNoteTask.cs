namespace Fanex.CustomerSetting.Tasks.SettingType8
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.SettingType8.Models;
    using System.Linq;

    public class BetSettingItemNoteTask : IPostQueryCustomerSettingTypeTask
    {
        private const string defaultMsg = "BetSettingNoteMessage";
        
        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(settingTypeData, nameof(settingTypeData));
            foreach (StakeLevelGroupItem stakeLevelGroupItem in settingTypeData.SettingItems.OfType<StakeLevelGroupItem>())
            {
                if (stakeLevelGroupItem.IsBuyBonus)
                {
                    stakeLevelGroupItem.ShowIconTooltipMessage = true;
                    stakeLevelGroupItem.TooltipMessage = GetToolTipMessageBySportTypeId(stakeLevelGroupItem.SportTypeId);
                }
            }
        }
        
        private string GetToolTipMessageBySportTypeId(int sportTypeId)
        {
            var msg= Resources.ResourceManager.GetString($"BetSettingNoteMessage_{sportTypeId}");
            return msg ?? Resources.ResourceManager.GetString(defaultMsg);
        }
    }
}