namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.NAP.Framework.Data;

    public interface ISettingItem
    {
        IResult Validate();

        string ToXml();
    }
}
