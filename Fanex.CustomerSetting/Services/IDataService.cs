namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// ICustomerSettingService class.
    /// </summary>
    public interface IDataService
    {
        CompanySelectSuperResult CompanySelectSuper(SelectInfo model);

        SuperSelectMasterResult SuperSelectMaster(SelectInfo model);

        MasterSelectAgentResult MasterSelectAgent(SelectInfo model);

        SuperSelectMemberResult SuperSelectMember(SelectInfo model);

        MasterSelectMemberResult MasterSelectMember(SelectInfo model);

        AgentSelectMemberResult AgentSelectMember(SelectInfo model);

        UpdateResult Update(UpdateInfo model);
    }
}