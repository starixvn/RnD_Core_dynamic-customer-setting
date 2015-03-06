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
                    return jsonObject.ToObject<CompanySelectSuperDiscountEntity>();

                case FormType.SuperEditMaster:
                    return jsonObject.ToObject<SuperSelectMasterDiscountEntity>();

                case FormType.MasterEditAgent:
                    return jsonObject.ToObject<MasterSelectAgentDiscountEntity>();

                case FormType.SuperEditMember:
                    return jsonObject.ToObject<SuperSelectMemberDiscountEntity>();

                case FormType.MasterEditMember:
                    return jsonObject.ToObject<MasterSelectMemberDiscountEntity>();

                case FormType.AgentEditMember:
                    return jsonObject.ToObject<AgentSelectMemberDiscountEntity>();
            }

            return null;
        }
    }
}