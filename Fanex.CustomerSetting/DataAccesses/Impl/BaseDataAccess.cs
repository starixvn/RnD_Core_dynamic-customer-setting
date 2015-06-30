namespace Fanex.CustomerSetting.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.Data;

    public class BaseDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        protected IResult ExecuteNonQuery(string settingKey, dynamic storedProcedureParam)
        {
            using (var objectDb = new ObjectDb(settingKey))
            {
                objectDb.ExecuteNonQuery(storedProcedureParam);
                return ReadOutputWithMessageType(objectDb.GetOuputParams());
            }
        }

        protected IResult ReadOutputWithMessageType(Hashtable outputParams)
        {
            if (!outputParams.ContainsKey("errcode"))
            {
                return Result.SystemError;
            }

            int msgType = 0;
            if (outputParams.ContainsKey("msgtype"))
            {
                msgType = Convert.ToInt32(outputParams["msgtype"]);
            }

            return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    Convert.ToString(outputParams["errmsg"]),
                    msgType);
        }

        protected dynamic GetSPParamsForGettingSettingType(QuerySettingParam querySettingParam, byte settingType)
        {
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

        protected string GetSettingKeyForGettingSettingType(
            CustomerLevel customerLevel,
            string keySuffix)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperSettingSelect" + keySuffix;

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterSettingSelect" + keySuffix;

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentSettingSelect" + keySuffix;

                case CustomerLevel.Member:
                    return "Acc_CustSetting_MemberSettingSelect" + keySuffix;

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }
    }
}