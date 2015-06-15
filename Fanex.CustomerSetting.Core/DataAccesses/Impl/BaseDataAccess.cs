namespace Fanex.CustomerSetting.Core.DataAccesses.Impl
{
    using System;
    using Fanex.CustomerSetting.Core.Results;
    using Fanex.CustomerSetting.Core.Results.Impl;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class BaseDataAccess
    {
        public IResult ExecuteNonQuery(ICriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = objectDb.GetOuputParams();
                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    Convert.ToString(outputParams["errmsg"]));
            }
        }
    }
}