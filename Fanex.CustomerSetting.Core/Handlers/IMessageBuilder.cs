namespace Fanex.CustomerSetting.Core.Handlers
{
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public interface IMessageBuilder
    {
        IResult Build(SettingType settingType, IResult result);
    }
}
