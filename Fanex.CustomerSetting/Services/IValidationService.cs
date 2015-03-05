namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IValidationService class.
    /// </summary>
    public interface IValidationService
    {
        Result Validate(SelectModel model);

        Result Validate(UpdateModel model);
    }
}