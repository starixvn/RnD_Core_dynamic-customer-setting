namespace Fanex.CustomerSetting.DataAccesses
{
    using Models.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}