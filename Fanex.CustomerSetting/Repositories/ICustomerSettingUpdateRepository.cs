namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;

    /// <summary>
    /// ICustomerSettingUpdateRepository class.
    /// </summary>
    public interface ICustomerSettingUpdateRepository
    {
        void UpdateDisabledStatus(UpdateDisabledStatusCriteria criteria);

        void UpdateDiscount(UpdateDiscountCriteria criteria);

        void UpdateBetLimit(UpdateBetLimitCriteria criteria);

        void UpdatePositionTaking(UpdatePositionTakingCriteria criteria);
    }
}