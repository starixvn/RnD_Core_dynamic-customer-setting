namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermission(int actorId, int customerId, byte customerLevelId);

        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}