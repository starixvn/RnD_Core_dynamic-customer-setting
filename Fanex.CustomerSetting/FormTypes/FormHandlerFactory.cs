namespace Fanex.CustomerSetting.FormTypes
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    public sealed class FormHandlerFactory
    {
        public static IFormHandler Create(FormType formType)
        {
            switch (formType)
            {
                case FormType.CompanyEditSuper:
                    return new CompanyEditSuperFormHandler();

                case FormType.SuperEditMaster:
                    return new SuperEditMasterFormHandler();

                case FormType.MasterEditAgent:
                    return new MasterEditAgentFormHandler();

                case FormType.SuperEditMember:
                    return new SuperEditMemberFormHandler();

                case FormType.MasterEditMember:
                    return new MasterEditMemberFormHandler();

                case FormType.AgentEditMember:
                    return new AgentEditMemberFormHandler();

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }
    }
}