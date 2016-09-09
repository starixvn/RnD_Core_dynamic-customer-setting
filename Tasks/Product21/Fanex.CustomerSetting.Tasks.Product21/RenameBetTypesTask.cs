namespace Fanex.CustomerSetting.Tasks.Product21
{
    using Common.Helper;
    using Models;
    using Models.Requests;
    using Models.Results;
    using Tasks;

    public class RenameBetTypesTask : IPostQueryCustomerSettingTypeTask, IPostQueryCustomerSettingTask
    {
        private const short ProductStatusSettingType = 4;
        private const short StakeLevelSettingType = 7;
        private const int Baccarat = 2401;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(settingTypeData, nameof(settingTypeData));

            if (settingTypeData.Id == ProductStatusSettingType || settingTypeData.Id == StakeLevelSettingType)
            {
                foreach (ISettingItem item in settingTypeData.SettingItems)
                {
                    var settingItem = item as SettingItemDefault;

                    if (settingItem?.BetTypeId == Baccarat)
                    {
                        settingItem.BetTypeName = Resources.NormalGames;
                    }
                }
            }
        }

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            var subMessages = new string[]
            {
                Resources.NormalGamesSubMessage,
                Resources.VipBaccaratSubMessage
            };

            setting.WarningMessage = new UpdateSettingResult(new SettingResult[]
            {
                new SettingResult(0, Resources.MainInfoMessage, subMessages)
            });
        }
    }
}