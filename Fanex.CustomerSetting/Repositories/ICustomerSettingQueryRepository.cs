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

        SuperQueryMemberCustomerSettingEntity SuperQueryMember(QueryMemberCriteria criteria);

        MasterQueryMemberCustomerSettingEntity MasterQueryMember(QueryMemberCriteria criteria);

        AgentQueryMemberCustomerSettingEntity AgentQueryMember(QueryMemberCriteria criteria);
    }
}