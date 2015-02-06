namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// ICustomerSettingService class.
    /// </summary>
    public interface ICustomerSettingService
    {
        CompanySelectSuperResult CompanySelectSuper(SelectModel model);

        SuperSelectMasterResult SuperSelectMaster(SelectModel model);

        MasterSelectAgentResult MasterSelectAgent(SelectModel model);

        SuperSelectMemberResult SuperSelectMember(SelectModel model);

        MasterSelectMemberResult MasterSelectMember(SelectModel model);

        AgentSelectMemberResult AgentSelectMember(SelectModel model);
    }
}