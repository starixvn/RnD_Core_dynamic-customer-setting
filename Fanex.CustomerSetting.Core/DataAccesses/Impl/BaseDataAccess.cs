namespace Fanex.CustomerSetting.Core.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.Data;

    public class BaseDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        protected IResult ExecuteNonQuery(string settingKey, dynamic storedProcedureParams)
        {
            using (var objectDb = new ObjectDb(settingKey))
            {
                objectDb.ExecuteNonQuery(storedProcedureParams);
                var outputParams = objectDb.GetOuputParams();
                return ReadOutputWithoutMessageType(outputParams);
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
    }
}