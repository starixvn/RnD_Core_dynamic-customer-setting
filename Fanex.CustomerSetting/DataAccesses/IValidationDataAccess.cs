namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}