namespace Fanex.CustomerSetting.Criteria
{
    using System;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.Data.Repository;

    public class PositionTakingCommandCriteria : CommandCriteria
    {
        public PositionTakingCommandCriteria(FormType formType)
        {
            FormType = formType;
        }

        [SpParam(SqlParamName = "PT")]
        public string PositionTaking { get; set; }

        [SpParam(Ignored = true)]
        public FormType FormType { get; private set; }

        public override string GetSettingKey()
        {
            switch (FormType)
            {
                case FormType.CompanyEditSuper:
                    return "Acc_CustSetting_SuperPTUpd";

                case FormType.SuperEditMaster:
                    return "Acc_CustSetting_MasterPTUpd";

                case FormType.MasterEditAgent:
                    return "Acc_CustSetting_AgentPTUpd";

                case FormType.SuperEditMember:
                    return "Acc_CustSetting_SuperPTOnMemberUpd";

                case FormType.MasterEditMember:
                    return "Acc_CustSetting_MasterPTOnMemberUpd";

                case FormType.AgentEditMember:
                    return "Acc_CustSetting_AgentPTOnMemberUpd";

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(PositionTaking);
        }
    }
}