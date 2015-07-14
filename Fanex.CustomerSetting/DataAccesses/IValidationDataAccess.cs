namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationDataAccess
    {
        IResult ValidateBelongIn(int actorId, int customerId);

        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}