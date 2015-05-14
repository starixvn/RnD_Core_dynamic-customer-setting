namespace Fanex.CustomerSetting.Repositories
{
    using System;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.Data;

    public class ValidationRepository : IValidationRepository
    {
        public void ValidatePermission(PermissionValidateCriteria criteria)
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