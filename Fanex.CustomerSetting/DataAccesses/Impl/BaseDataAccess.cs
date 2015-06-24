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
        protected IResult ExecuteNonQuery(string settingKey, dynamic storedProcedureParams, bool isReadMessageType = true)
        {
            using (var objectDb = new ObjectDb(settingKey))
            {
                objectDb.ExecuteNonQuery(storedProcedureParams);
                var outputParams = objectDb.GetOuputParams();
                if (isReadMessageType)
                {
                    return ReadOutputWithMessageType(outputParams);
                }
                else
                {
                    return ReadOutputWithoutMessageType(outputParams);
                }
            }
        }

        protected IResult ReadOutputWithoutMessageType(Hashtable outputParams)
        {
            return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    Convert.ToString(outputParams["errmsg"]));
        }

        protected IResult ReadOutputWithMessageType(Hashtable outputParams)
        {
            return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    Convert.ToString(outputParams["errmsg"]),
                    Convert.ToInt32(outputParams["msgtype"]));
        }

        protected dynamic GetSPParamsForGettingSettingTypeSetting(SettingQuery settingQuery, byte settingType)
        {
            switch (settingQuery.CustomerLevel)
            {
                case CustomerLevel.Super:
                case CustomerLevel.Master:
                case CustomerLevel.Agent:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId,
                        SettingType = settingType
                    };

                case CustomerLevel.Member:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId,
                        RoleId = settingQuery.UplineLevel.Value,
                        SettingType = settingType
                    };

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }
    }
}