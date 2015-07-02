namespace Fanex.CustomerSetting.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.Data;

    public class BaseDataAccess
    {
        private const string SettingKeySuffixFormat = "_{0}_{1}";

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        protected IResult ExecuteNonQuery(string settingKey, dynamic storedProcedureParam)
        {
            using (var objectDb = new ObjectDb(settingKey))
            {
                objectDb.ExecuteNonQuery(storedProcedureParam);
                var param = objectDb.GetOuputParams();

                return GetResult(param);
            }
        }

        protected IResult GetResult(Hashtable param)
        {
            if (param != null
                && param.ContainsKey("errcode")
                && param.ContainsKey("errmsg"))
            {
                if (param.ContainsKey("msgtype"))
                {
                    return new Result(
                             Convert.ToInt32(param["errcode"]),
                             Convert.ToString(param["errmsg"]),
                             Convert.ToInt32(param["msgtype"]));
                }
                else
                {
                    return new Result(
                             Convert.ToInt32(param["errcode"]),
                             Convert.ToString(param["errmsg"]));
                }
            }

            throw new ArgumentException("result output param is nulll or does not contain neither errcode nor errmsg.");
        }

        protected dynamic GetSPParamsForGettingSettingType(QuerySettingParam querySettingParam, byte settingType)
        {
            if (querySettingParam == null)
            {
                throw new ArgumentNullException("querySettingParam", "querySettingParam cannot be null.");
            }

            switch (querySettingParam.CustomerLevel)
            {
                case CustomerLevel.Super:
                case CustomerLevel.Master:
                case CustomerLevel.Agent:
                    return new
                    {
                        CustId = querySettingParam.CustomerId,
                        ProductId = querySettingParam.ProductId,
                        SettingType = settingType
                    };

                case CustomerLevel.Member:
                    return new
                    {
                        CustId = querySettingParam.CustomerId,
                        ProductId = querySettingParam.ProductId,
                        RoleId = querySettingParam.UplineLevel.Value,
                        SettingType = settingType
                    };

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }

        protected string GetSettingKeyForGettingSettingType(QuerySettingParam queryParam, byte settingTypeId, byte settingTemplateId)
        {
            if (queryParam == null)
            {
                throw new ArgumentNullException("queryParam");
            }

            var keySuffix = string.Format(SettingKeySuffixFormat, settingTypeId, settingTemplateId);
            if (CustomerSettingSection.Settings.IsProductDoesNotUseDefaultStoredProcedures(settingTypeId, settingTemplateId, queryParam.ProductId))
            {
                keySuffix += string.Format("_{0}", queryParam.ProductId);
            }

            switch (queryParam.CustomerLevel)
            {
                case CustomerLevel.Super:
                    return "CustSetting_SuperSettingSelect" + keySuffix;

                case CustomerLevel.Master:
                    return "CustSetting_MasterSettingSelect" + keySuffix;

                case CustomerLevel.Agent:
                    return "CustSetting_AgentSettingSelect" + keySuffix;

                case CustomerLevel.Member:
                    return "CustSetting_MemberSettingSelect" + keySuffix;

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }
    }
}