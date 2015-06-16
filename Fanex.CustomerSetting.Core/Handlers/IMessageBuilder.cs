namespace Fanex.CustomerSetting.Core.Handlers
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public interface IMessageBuilder
    {
        IResult Build(SettingType settingType, IResult result);
    }
}