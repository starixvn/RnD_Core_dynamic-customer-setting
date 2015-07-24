namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Results;

    public interface IUpdateSettingService
    {
        IResult Update(UpdateCustomerSettingParam updateParam);

        IResult Update(UpdateMultipleCustomerSettingParam updateMultipleParam);
    }
}