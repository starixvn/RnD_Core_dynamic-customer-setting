namespace Fanex.CustomerSetting.Tasks.Product21
{
    using System.Linq;
    using Common.Helper;
    using Models;
    using Models.Requests;
    using Tasks;

    public class RenameBetTypesTask : IPostQueryCustomerSettingTypeTask
    {
        private const short ProductStatusSettingType = 4;
        private const short StakeLevelSettingType = 7;
        private const int Baccarat = 2401;
        private const int VipBaccarat = 2402;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(settingTypeData, nameof(settingTypeData));

            if (settingTypeData.Id == ProductStatusSettingType)
            {
                var baccarat = settingTypeData.SettingItems.FirstOrDefault(item => (item as SettingItemDefault)?.BetTypeId == Baccarat) as SettingItemDefault;

                if (baccarat != null)
                {
                    baccarat.BetTypeName = Resources.OtherDisplayingTextForBaccarat;
                }
            }

            if (settingTypeData.Id == StakeLevelSettingType)
            {
                var baccarat = settingTypeData.SettingItems.FirstOrDefault(item => (item as SettingItemDefault)?.BetTypeId == Baccarat) as SettingItemDefault;
                var vipBaccarat = settingTypeData.SettingItems.FirstOrDefault(item => (item as SettingItemDefault)?.BetTypeId == VipBaccarat) as SettingItemDefault;

                if (baccarat != null)
                {
                    baccarat.BetTypeName = Resources.OtherDisplayingTextForBaccarat;
                }

                if (vipBaccarat != null)
                {
                    vipBaccarat.BetTypeName = Resources.OtherDisplayingTextForVipBaccarat;
                }
            }
        }
    }
}