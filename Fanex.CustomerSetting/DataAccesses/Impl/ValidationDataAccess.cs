namespace Fanex.CustomerSetting.DataAccesses.Impl
{
    using System.Collections;
    using Fanex.CustomerSetting.Dtos.Results;

    public class ValidationDataAccess : BaseDataAccess, IValidationDataAccess
    {
        public IResult ValidatePermissionMultiple(int actorId, string memberIds)
        {
            var param = new Hashtable();
            param.Add("custid", actorId);
            param.Add("targetcustidlst", memberIds);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            return ExecuteNonQuery("Acc_CustSetting_MultiCustPermissionValidate", param);
        }

        public IResult ValidateActorEnabled(int actorId)
        {
            var param = new Hashtable();
            param.Add("custid", actorId);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            return ExecuteNonQuery("Acc_CustSetting_EnabledAccountCheck", param);
        }

        public bool ValidateBelonging(int actorId, int customerId)
        {
            var param = new Hashtable();
            param.Add("custid", actorId);
            param.Add("targetcustid", customerId);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            IResult result = ExecuteNonQuery("Acc_CustSetting_SameLineCheck", param);

            return result.IsSuccessful;
        }

        public IResult ValidateCustomerAndActorLevel(int actorId, int customerId, byte customerLevelId)
        {
            var param = new Hashtable();
            param.Add("custid", actorId);
            param.Add("targetcustid", customerId);
            param.Add("targetroleid", customerLevelId);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            return ExecuteNonQuery("Acc_custSetting_RoleIDCheck", param);
        }
    }
}