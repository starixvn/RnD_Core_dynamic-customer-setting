namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json.Linq;

    public class DiscountJsonConverter : JsonCreationConverter<DiscountEntity>
    {
        private FormType _formType;

        public DiscountJsonConverter(FormType formType)
        {
            _formType = formType;
        }

        protected override DiscountEntity Create(Type objectType, JObject jsonObject)
        {
            switch (_formType)
            {
                case FormType.CompanyEditSuper:
                    return jsonObject.ToObject<CompanyQuerySuperDiscountEntity>();

                case FormType.SuperEditMaster:
                    return jsonObject.ToObject<SuperQueryMasterDiscountEntity>();

                case FormType.MasterEditAgent:
                    return jsonObject.ToObject<MasterQueryAgentDiscountEntity>();

                case FormType.SuperEditMember:
                    return jsonObject.ToObject<SuperQueryMemberDiscountEntity>();

                case FormType.MasterEditMember:
                    return jsonObject.ToObject<MasterQueryMemberDiscountEntity>();

                case FormType.AgentEditMember:
                    return jsonObject.ToObject<AgentQueryMemberDiscountEntity>();
            }

            return null;
        }
    }
}