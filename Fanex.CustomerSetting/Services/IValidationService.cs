namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface IValidationService
    {
        Result Validate(QueryInfo model);

        Result Validate(CommandInfo model);
    }
}