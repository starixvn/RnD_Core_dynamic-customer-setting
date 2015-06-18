namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface ICommandOnMultipleCustomerServiceFactory
    {
        ICommandOnMultipleCustomerService Create(MultipleCommandInfo multipleCommandInfo);
    }
}