namespace Fanex.CustomerSetting.DataAccesses.Impl
{
    using System.Collections;
    using Fanex.CustomerSetting.Dtos.Results;

    public class ValidationDataAccess : BaseDataAccess, IValidationDataAccess
    {
        public IResult ValidatePermission(int actorId, int customerId, byte customerLevelId)
        {
            var param = new Hashtable();
            param.Add("custid", actorId);
            param.Add("targetcustid", customerId);
            param.Add("targetroleid", customerLevelId);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            return ExecuteNonQuery("Acc_CustSetting_PermissionValidate", param);
        }

        public IResult ValidatePermissionMultiple(int actorId, string memberIds)
        {
            var param = new Hashtable();
            param.Add("custid", actorId);
            param.Add("targetcustidlst", memberIds);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            return ExecuteNonQuery("Acc_CustSetting_MultiCustPermissionValidate", param);
        }
    }
}