namespace Fanex.CustomerSetting.Core.Models
{
    using Fanex.CustomerSetting.Core.Results;
    using Fanex.NAP.Framework.Data;

    public interface ISettingItem
    {
        byte SettingLevel { get; set; }

        IResult Validate();

        string ToXml();
    }
}
