namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// ProductStatusConverter class.
    /// </summary>
    public class StatusJsonConverter : JsonCreationConverter<ProductStatusEntity>
    {
        private FormType _formType;

        public StatusJsonConverter(FormType formType)
        {
            _formType = formType;
        }

        /// <summary>
        /// Creates ProductStatusEntity.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="jsonObject">The JSON object.</param>
        /// <returns>Returns ProductStatusEntity.</returns>
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

                case FormType.SuperEditMember:
                    return jsonObject.ToObject<SuperQueryMemberProductStatusEntity>();

                case FormType.MasterEditMember:
                    return jsonObject.ToObject<MasterQueryMemberProductStatusEntity>();

                case FormType.AgentEditMember:
                    return jsonObject.ToObject<AgentQueryMemberProductStatusEntity>();
            }

            return null;
        }
    }
}