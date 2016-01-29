namespace Fanex.CustomerSetting.DataAccesses.Impl
{
    using System.Collections;
    using Fanex.CustomerSetting.Models.Results;

    internal class ValidationDataAccess : BaseDataAccess, IValidationDataAccess
    {
        public IResult ValidatePermissionMultiple(int actorId, string memberIds)
        {
            var param = new Hashtable
            {
                {"custid", actorId},
                {"targetcustidlst", memberIds},
                {"errcode", 0},
                {"errmsg", string.Empty}
            };

            return ExecuteNonQuery("CustSetting_MultiCustPermissionValidate", param);
        }
    }
}