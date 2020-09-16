namespace Fanex.CustomerSetting.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using Data;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;

    public class BaseDataAccess
    {
        protected IResult ExecuteNonQuery(string settingKey, Hashtable storedProcedureParam)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb(settingKey))
            {
                objectDb.ExecuteNonQuery(storedProcedureParam);

                return GetResult(storedProcedureParam);
            }
        }

        protected IResult GetResult(Hashtable param)
        {
            if (param == null || !param.ContainsKey("errcode") || !param.ContainsKey("errmsg"))
            {
                throw new ArgumentException(
                    "result output param is nulll or does not contain neither errcode nor errmsg.");
            }

            var errorCode = Convert.ToInt32(param["errcode"]);
            var errMsg = Convert.ToString(param["errmsg"]);
            if (param.ContainsKey("msgtype"))
            {
                return new Result(
                    errorCode,
                    errMsg,
                    Convert.ToInt32(param["msgtype"]));
            }

            return new Result(errorCode, errMsg);
        }

        protected Hashtable GetSPParamsForGettingSettingType(CustomerSettingQuery querySettingParam, byte settingType)
        {
            if (querySettingParam == null)
            {
#pragma warning disable S2302 // "nameof" should be used
                throw new ArgumentNullException(nameof(querySettingParam), "querySettingParam cannot be null.");
#pragma warning restore S2302 // "nameof" should be used
            }

            var param = new Hashtable
            {
                { "custid", querySettingParam.CustomerId },
                { "productid", querySettingParam.ProductId },
                { "settingtype", settingType },
                { "settingstatus", querySettingParam.SettingStatus }
            };

            if (querySettingParam.CustomerLevel == Level.Member)
            {
                param.Add("roleid", querySettingParam.UplineLevel.Value);
            }

            return param;
        }
    }
}