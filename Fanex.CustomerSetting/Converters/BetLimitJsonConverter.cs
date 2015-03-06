namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Bet limit JSON converter class.
    /// </summary>
    public class BetLimitJsonConverter : JsonCreationConverter<BetLimitEntity>
    {
        private FormType _formType;

        public BetLimitJsonConverter(FormType formType)
        {
            _formType = formType;
        }

        /// <summary>
        /// Creates BetLimitEntity object.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="jsonObject">The JSON object.</param>
        /// <returns>Returns BetLimitEntity.</returns>
        protected override BetLimitEntity Create(Type objectType, JObject jsonObject)
        {
            switch (_formType)
            {
                case FormType.CompanyEditSuper:
                    return jsonObject.ToObject<CompanySelectSuperBetLimitEntity>();

                case FormType.SuperEditMaster:
                    return jsonObject.ToObject<SuperSelectMasterBetLimitEntity>();

                case FormType.MasterEditAgent:
                    return jsonObject.ToObject<MasterSelectAgentBetLimitEntity>();

                case FormType.SuperEditMember:
                    return jsonObject.ToObject<SuperSelectMemberBetLimitEntity>();

                case FormType.MasterEditMember:
                    return jsonObject.ToObject<MasterSelectMemberBetLimitEntity>();

                case FormType.AgentEditMember:
                    return jsonObject.ToObject<AgentSelectMemberBetLimitEntity>();
            }

            return null;
        }
    }
}