namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}