namespace Fanex.CustomerSetting.Repositories
{
    using System;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.Data;

    public abstract class ResultRepository
    {
        public void ExecuteNonQuery(ResultCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = objectDb.GetOuputParams();
                criteria.ErrCode = Convert.ToInt32(outputParams["errcode"]);
                criteria.ErrMsg = Convert.ToString(outputParams["errmsg"]);
            }
        }
    }
}