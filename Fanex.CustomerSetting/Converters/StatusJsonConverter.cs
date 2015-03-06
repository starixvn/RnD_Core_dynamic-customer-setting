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
                    return jsonObject.ToObject<CompanySelectSuperProductStatusEntity>();

                case FormType.SuperEditMaster:
                    return jsonObject.ToObject<SuperSelectMasterProductStatusEntity>();

                case FormType.MasterEditAgent:
                    return jsonObject.ToObject<MasterSelectAgentProductStatusEntity>();

                case FormType.SuperEditMember:
                    return jsonObject.ToObject<SuperSelectMemberProductStatusEntity>();

                case FormType.MasterEditMember:
                    return jsonObject.ToObject<MasterSelectMemberProductStatusEntity>();

                case FormType.AgentEditMember:
                    return jsonObject.ToObject<AgentSelectMemberProductStatusEntity>();
            }

            return null;
        }
    }
}