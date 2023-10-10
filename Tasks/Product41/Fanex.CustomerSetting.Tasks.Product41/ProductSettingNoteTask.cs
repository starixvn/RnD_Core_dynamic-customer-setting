namespace Fanex.CustomerSetting.Tasks.Product41
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.SettingType7.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductSettingNoteTask : IPostQueryCustomerSettingTask
    {
        private readonly int SettingType17 = 17;

        private readonly int SettingType7 = 7;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            List<string> messages = new List<string>();
            messages.Add(Resource.BBinSettingNote);

            if (setting.CustomerLevel.IsAgentLevel() && setting.SettingTypes.Any(item => item.Id == SettingType17))
            {
                messages.Add(Resource.AgentStakeLevelWarning);
            }

            if (!SystemType.IsY7(settingContext.System) && setting.SettingTypes.Any(item => item.Id == SettingType7)
                && query.CustomerLevel == Level.Member
                && AppSettings.SitesAllowUncheckStakeLevelOnMember.Contains(setting.CustomerInfo.Site))
            {
                List<SettingItemDefault> settingItems = (List<SettingItemDefault>)setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == SettingType7).SettingItems;
                AddNoneOption(settingItems);
            }

            setting.AddWarningMessages(messages.ToArray());
        }

        private void AddNoneOption(List<SettingItemDefault> settingItems)
        {
            foreach (var settingItem in settingItems)
            {
                StakeLevelItem stakeLevelItem = (StakeLevelItem)settingItem;
                bool isNoneSelected = !stakeLevelItem.StakeRanges.Any(i => i.Selected);
                stakeLevelItem.StakeRanges.Insert(0, new StakeRange() { Selected = isNoneSelected, StakeLevelId = -1, StakeLevelRange = Resource.NoneOption });
            }
        }
    }
}
