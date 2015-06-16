namespace Fanex.CustomerSetting.Core.Messages
{
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Results;

    public interface IMessageBuilder
    {
        IResult Build(SettingType settingType, IResult result);
    }
}
