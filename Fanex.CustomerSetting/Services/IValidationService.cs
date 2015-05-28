namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface IValidationService
    {
        Result Validate(QueryInfo queryInfo);

        Result Validate(CommandInfo commandInfo);

        Result Validate(MultipleCommandInfo multipleCommandInfo);
    }
}