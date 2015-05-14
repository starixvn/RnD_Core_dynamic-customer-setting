namespace Fanex.CustomerSetting.Criteria
{
    using System;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.Data.Repository;

    public class PositionTakingCommandOnGroupedMembersCriteria : CriteriaBase
    {
        public PositionTakingCommandOnGroupedMembersCriteria(FormType formType)
        {
            FormType = formType;
        }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        [SpParam(SqlParamName = "CustIDList")]
        public string CustIds { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int MsgType { get; set; }

        [SpParam(SqlParamName = "PT")]
        public string PositionTaking { get; set; }

        [SpParam(Ignored = true)]
        public FormType FormType { get; private set; }

        public override string GetSettingKey()
        {
            switch (FormType)
            {
                case FormType.SuperEditMember:
                    return "Acc_CustSetting_SuperPTOnMemberMultiUpd";

                case FormType.MasterEditMember:
                    return "Acc_CustSetting_MasterPTOnMemberMultiUpd";

                case FormType.AgentEditMember:
                    return "Acc_CustSetting_AgentPTOnMemberMultiUpd";

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }

        public override bool IsValid()
        {
            return UserId > 0 && !(UserId == 0 && SubAccId > 0) && !string.IsNullOrEmpty(CustIds) && !string.IsNullOrEmpty(PositionTaking);
        }
    }
}