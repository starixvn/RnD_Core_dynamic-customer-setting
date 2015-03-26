namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Position Taking JSON Converter class.
    /// </summary>
    public class PositionTakingJsonConverter : JsonCreationConverter<PositionTakingEntity>
    {
        private FormType _formType;

        public PositionTakingJsonConverter(FormType formType)
        {
            _formType = formType;
        }

        /// <summary>
        /// Creates PositionTakingEntity.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="jsonObject">The JSON object.</param>
        /// <returns>Returns PositionTakingEntity.</returns>
        protected override PositionTakingEntity Create(Type objectType, JObject jsonObject)
        {
            switch (_formType)
            {
                case FormType.CompanyEditSuper:
                    return jsonObject.ToObject<CompanyQuerySuperPositionTakingEntity>();

                case FormType.SuperEditMaster:
                    return jsonObject.ToObject<SuperCommandMasterPositionTakingEntity>();

                case FormType.MasterEditAgent:
                    return jsonObject.ToObject<MasterCommandAgentPositionTakingEntity>();

                case FormType.SuperEditMember:
                    return jsonObject.ToObject<SuperCommandMemberPositionTakingEntity>();

                case FormType.MasterEditMember:
                    return jsonObject.ToObject<MasterQueryMemberPositionTakingEntity>();

                case FormType.AgentEditMember:
                    return jsonObject.ToObject<AgentQueryMemberPositionTakingEntity>();
            }

            return null;
        }
    }
}