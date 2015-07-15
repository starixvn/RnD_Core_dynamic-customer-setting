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

            return ExecuteNonQuery("CustSetting_MultiCustPermissionValidate", param);
        }

        public bool ValidateBelonging(int actorId, int customerId)
        {
            var param = new Hashtable();
            param.Add("custid", actorId);
            param.Add("targetcustid", customerId);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            IResult result = ExecuteNonQuery("Acc_CustSetting_SameLineCheck", param);
            //return ExecuteNonQuery("CustSetting_SameLineCheck", param);

            return result.IsSuccessful;
        }
    }
}