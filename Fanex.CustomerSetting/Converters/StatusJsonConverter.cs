namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json.Linq;

    public class StatusJsonConverter : JsonCreationConverter<ProductStatusEntity>
    {
        private FormType _formType;

        public StatusJsonConverter(FormType formType)
        {
            _formType = formType;
        }

        protected override ProductStatusEntity Create(Type objectType, JObject jsonObject)
        {
            switch (_formType)
            {
                case FormType.CompanyEditSuper:
                    return jsonObject.ToObject<CompanyQuerySuperProductStatusEntity>();

                case FormType.SuperEditMaster:
                    return jsonObject.ToObject<SuperQueryMasterProductStatusEntity>();

                case FormType.MasterEditAgent:
                    return jsonObject.ToObject<MasterQueryAgentProductStatusEntity>();

                case FormType.AgentEditMember:
                    return jsonObject.ToObject<AgentQueryMemberProductStatusEntity>();

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }
    }
}