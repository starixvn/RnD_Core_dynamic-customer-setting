using System.Collections.Generic;
using System.Linq;
using Fanex.Common.Extensions;
using Fanex.CustomerSetting.SettingType21.Models;

namespace Fanex.CustomerSetting.Tasks.SettingType21
{
    using Common.Helper;
    using Models;
    using Models.Enumerations;
    using Models.Requests;

    public class AddSettingType21QueryWarningTask : IPostQueryCustomerSettingTask
    {
        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            var settingType21Items = setting.SettingTypes.FirstOrDefault(e => e.Id == 21)?.SettingItems;
                       DisableChildrenByParentState(settingType21Items);
            if (CheckIfEditingCustomerIsMasterOrAgent(query) && SystemType.IsAlpha(settingContext.System))
            {
                setting.AddWarningMessages(Resources.ProductStatusNoteForExternalSites);
            }

           
        }

        private static bool CheckIfEditingCustomerIsMasterOrAgent(CustomerSettingQuery query)
        {
            return query.CustomerLevel.IsMasterLevel() || query.CustomerLevel.IsAgentLevel();
        }

        private static void DisableChildrenByParentState(IEnumerable<ISettingItem> settingType21Items)
        {
            if (settingType21Items == null)
            {
                return;
            }

            var parent = settingType21Items.FirstOrDefault(item =>
                ((ProductStatusItem)item).ParentBetTypeId == 0 && 
                ((ProductStatusItem)item).ParentSportTypeId == 0) as ProductStatusItem;
            var children = settingType21Items.Where(item =>
                ((ProductStatusItem)item).ParentBetTypeId == parent.BetTypeId &&
                ((ProductStatusItem)item).ParentSportTypeId == parent.SportTypeId);
            if (parent != null && parent.IsDisabled)
            {
                children.ForEach(item => ((ProductStatusItem)item).IsReadOnly = true);
            }
        }
    }
}