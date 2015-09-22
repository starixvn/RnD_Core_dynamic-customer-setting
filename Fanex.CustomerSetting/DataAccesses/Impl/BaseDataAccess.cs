namespace Fanex.CustomerSetting.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.Data;

    public class BaseDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        protected IResult ExecuteNonQuery(string settingKey, Hashtable storedProcedureParam)
        {
            using (var objectDb = new ObjectDb(settingKey))
            {
                objectDb.ExecuteNonQuery(storedProcedureParam);

                return GetResult(storedProcedureParam);
            }
        }

        protected IResult GetResult(Hashtable param)
        {
            if (param != null
                && param.ContainsKey("errcode")
                && param.ContainsKey("errmsg"))
            {
                var errorCode = Convert.ToInt32(param["errcode"]);
                var errMsg = Convert.ToString(param["errmsg"]);
                if (param.ContainsKey("msgtype"))
                {
                    return new Result(
                             errorCode,
                             errMsg,
                             Convert.ToInt32(param["msgtype"]));
                }
                else
                {
                    return new Result(errorCode, errMsg);
                }
            }

            throw new ArgumentException("result output param is nulll or does not contain neither errcode nor errmsg.");
        }

        protected Hashtable GetSPParamsForGettingSettingType(CustomerSettingQuery querySettingParam, byte settingType)
        {
            if (querySettingParam == null)
            {
                throw new ArgumentNullException("querySettingParam", "querySettingParam cannot be null.");
            }

            Hashtable param = new Hashtable();
            param.Add("custid", querySettingParam.CustomerId);
            param.Add("productid", querySettingParam.ProductId);
            param.Add("settingtype", settingType);

            if (querySettingParam.CustomerLevel == Level.Member)
            {
                param.Add("roleid", (byte)querySettingParam.UplineLevel.Value);
            }

            return param;
        }
    }
}