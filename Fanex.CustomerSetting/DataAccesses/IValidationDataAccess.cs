namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationDataAccess
    {
        IResult ValidateActorEnabled(int actorId);

        IResult ValidateBelongIn(int actorId, int customerId);

        IResult ValidateLevel(int actorId, int customerId, byte customerLevelId);

        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}