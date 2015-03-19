namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;

    /// <summary>
    /// ICustomerSettingUpdateRepository class.
    /// </summary>
    public interface ICustomerSettingCommandRepository
    {
        void UpdateDisabledStatus(CommandDisabledStatusCriteria criteria);

        void UpdateDiscount(CommandDiscountCriteria criteria);

        void UpdateBetLimit(CommandBetLimitCriteria criteria);

        void UpdatePositionTaking(CommandPositionTakingCriteria criteria);
    }
}