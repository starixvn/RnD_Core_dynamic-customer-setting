namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Discount JSON Converter class.
    /// </summary>
    public class DiscountJsonConverter : JsonCreationConverter<DiscountEntity>
    {
        private FormType _formType;

        public DiscountJsonConverter(FormType formType)
        {
            _formType = formType;
        }

        /// <summary>
        /// Creates the DiscountEntity object.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="jsonObject">The JSON object.</param>
        /// <returns>Returns DiscountEntity.</returns>
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