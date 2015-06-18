namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;

    public interface ICommandOnGroupedMembersRepository
    {
        void UpdatePositionTaking(PositionTakingCommandOnGroupedMembersCriteria criteria);
    }
}