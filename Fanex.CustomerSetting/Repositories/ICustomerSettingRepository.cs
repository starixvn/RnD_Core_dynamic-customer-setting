namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerSettingRepository
    {
        CompanySelectSuperCustomerSettingEntity CompanySelectSuper(CompanySelectSuperCriteria criteria);

        SuperSelectMasterCustomerSettingEntity SuperSelectMaster(SuperSelectMasterCriteria criteria);

        MasterSelectAgentCustomerSettingEntity MasterSelectAgent(MasterSelectAgentCriteria criteria);

        SuperSelectMemberCustomerSettingEntity SuperSelectMember(SuperSelectMemberCriteria criteria);

        MasterSelectMemberCustomerSettingEntity MasterSelectMember(SuperSelectMemberCriteria criteria);

        AgentSelectMemberCustomerSettingEntity AgentSelectMember(SuperSelectMemberCriteria criteria);
    }
}