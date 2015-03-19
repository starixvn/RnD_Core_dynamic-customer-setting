namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IValidationService class.
    /// </summary>
    public interface IValidationService
    {
        Result Validate(QueryInfo model);

        Result Validate(CommandInfo model);
    }
}