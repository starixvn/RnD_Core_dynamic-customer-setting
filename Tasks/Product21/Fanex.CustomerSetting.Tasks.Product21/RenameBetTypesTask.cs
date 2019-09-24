namespace Fanex.CustomerSetting.Tasks.Product21
{
    using System.Linq;
    using Common.Helper;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Models;
    using Models.Requests;
    using Tasks;

    public class RenameBetTypesTask : IPostQueryCustomerSettingTypeTask
    {
        private const short ProductStatusSettingType = 13;
        private const short StakeLevelSettingType = 7;
        private const short Baccarat = 2401;
        private const short VipBaccarat = 2402;

        private readonly IResourceService _resourceService;
        private readonly short[] _referencingBaccaratBetTypeIds = { Baccarat, 2403, 2404, 2405, 2406, 2407 };

        public RenameBetTypesTask() : this(null)
        {
        }

        public RenameBetTypesTask(IResourceService resourceService)
            => _resourceService = resourceService ?? new ResourceService();

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
                    baccarat.BetTypeName = BuildReferencingBaccaracBetTypeNames(settingContext);
                }
            }

            if (settingTypeData.Id == StakeLevelSettingType)
            {
                var baccarat = settingTypeData.SettingItems.FirstOrDefault(item => (item as SettingItemDefault)?.BetTypeId == Baccarat) as SettingItemDefault;
                var vipBaccarat = settingTypeData.SettingItems.FirstOrDefault(item => (item as SettingItemDefault)?.BetTypeId == VipBaccarat) as SettingItemDefault;

                if (baccarat != null)
                {
                    baccarat.BetTypeName = BuildReferencingBaccaracBetTypeNames(settingContext);
                }

                if (vipBaccarat != null)
                {
                    vipBaccarat.BetTypeName = Resources.OtherDisplayingTextForVipBaccarat;
                }
            }
        }

        private string BuildReferencingBaccaracBetTypeNames(ISettingContext settingContext)
        {
            var referencingBetTypeNames = _referencingBaccaratBetTypeIds
                .Select(betTypeId => _resourceService.GetBetTypeName(betTypeId, settingContext.Language))
                .ToArray();

            return string.Format(Resources.ReferencingBaccaratBetTypes, referencingBetTypeNames);
        }
    }
}