namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Models.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermissionMultiple(int actorId, string memberIds);
    }
}