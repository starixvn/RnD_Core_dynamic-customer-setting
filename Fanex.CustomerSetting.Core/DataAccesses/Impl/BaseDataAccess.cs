namespace Fanex.CustomerSetting.Core.DataAccesses.Impl
{
    using System;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Dtos.Results.Impl;
    using Fanex.CustomerSetting.Core.SPObjects.Commands;
    using Fanex.Data;

    public class BaseDataAccess
    {
        public IResult ExecuteNonQuery(BaseCommand command)
        {
            using (var objectDb = new ObjectDb(command.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(command.GetSpParams());
                var outputParams = objectDb.GetOuputParams();
                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    Convert.ToString(outputParams["errmsg"]));
            }
        }
    }
}