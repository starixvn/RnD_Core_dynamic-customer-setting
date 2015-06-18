namespace Fanex.CustomerSetting.Core.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.NAP.Framework.Data;

    public interface ISettingItem
    {
        IResult Validate();

        string ToXml();
    }
}
