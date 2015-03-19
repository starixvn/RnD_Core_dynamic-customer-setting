namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// ICustomerSettingService class.
    /// </summary>
    public interface ICustomerSettingQueryService
    {
        CompanyQuerySuperResult CompanyQuerySuper(QueryInfo model);

        SuperQueryMasterResult SuperQueryMaster(QueryInfo model);

        MasterQueryAgentResult MasterQueryAgent(QueryInfo model);

        SuperQueryMemberResult SuperQueryMember(QueryInfo model);

        MasterQueryMemberResult MasterQueryMember(QueryInfo model);

        AgentQueryMemberResult AgentQueryMember(QueryInfo model);
    }
}