namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;

    public interface ICommandRepository
    {
        void UpdateDisabledStatus(DisabledStatusCommandCriteria criteria);

        void UpdateDiscount(DiscountCommandCriteria criteria);

        void UpdateBetLimit(BetLimitCommandCriteria criteria);

        void UpdatePositionTaking(PositionTakingCommandCriteria criteria);
    }
}