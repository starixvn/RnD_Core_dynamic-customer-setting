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

        protected Hashtable GetSPParamsForGettingSettingType(QuerySettingParam querySettingParam, byte settingType)
        {
            if (querySettingParam == null)
            {
                throw new ArgumentNullException("querySettingParam", "querySettingParam cannot be null.");
            }

            Hashtable param = new Hashtable();
            param.Add("custid", querySettingParam.CustomerId);
            param.Add("productid", querySettingParam.ProductId);
            param.Add("settingtype", settingType);
            
            if (querySettingParam.CustomerLevel == UserLevel.Member)
            {
                param.Add("roleid", (byte)querySettingParam.UplineLevel.Value);
            }

            return param;
        }

        protected string GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(byte settingTypeId, byte settingTemplateId, byte productId)
        {
            if (CustomerSettingSection.Settings.IsProductNotUseDefaultSettingTypeStoredProcedures(settingTypeId, settingTemplateId, productId))
            {
                return string.Format("_{0}", productId);
            }

            return string.Empty;
        }
    }
}