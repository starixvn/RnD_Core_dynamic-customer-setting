namespace Fanex.CustomerSetting.Core.DataAccesses.Impl
{
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public class ValidationDataAccess : BaseDataAccess, IValidationDataAccess
    {
        public IResult ValidatePermission(int actorId, int customerId, byte customerLevelId)
        {
            return ExecuteNonQuery(
                "Acc_CustSetting_PermissionValidate", 
                new 
                {
                    CustId = actorId,
                    TargetCustId = customerId,
                    TargetRoleId = customerLevelId,
                    ErrCode = 1,
                    ErrMsg = string.Empty
                },
                false);
        }

        public IResult ValidatePermissionMultiple(int actorId, string memberIds)
        {
            return ExecuteNonQuery(
                "Acc_CustSetting_MultiCustPermissionValidate", 
                new
                {
                    CustId = actorId,
                    TargetCustIDLst = memberIds,
                    ErrCode = 1,
                    ErrMsg = string.Empty
                },
                false);
        }
    }
}