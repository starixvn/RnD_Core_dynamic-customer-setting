namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerSettingQueryRepository
    {
        CompanyQuerySuperCustomerSettingEntity CompanyQuerySuper(CompanyQuerySuperCriteria criteria);

        SuperQueryMasterCustomerSettingEntity SuperQueryMaster(SuperQueryMasterCriteria criteria);

        MasterQueryAgentCustomerSettingEntity MasterQueryAgent(MasterQueryAgentCriteria criteria);

        SuperQueryMemberCustomerSettingEntity SuperQueryMember(SuperQueryMemberCriteria criteria);

        MasterQueryMemberCustomerSettingEntity MasterQueryMember(SuperQueryMemberCriteria criteria);

        AgentQueryMemberCustomerSettingEntity AgentQueryMember(SuperQueryMemberCriteria criteria);
    }
}