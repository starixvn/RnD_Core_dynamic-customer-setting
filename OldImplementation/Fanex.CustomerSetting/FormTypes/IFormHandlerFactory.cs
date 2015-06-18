namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;

    public interface IFormHandlerFactory
    {
        IFormHandler Create(FormType formType);
    }
}