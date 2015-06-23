namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermission(int actorId, int customerId, byte customerLevelId);

        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}