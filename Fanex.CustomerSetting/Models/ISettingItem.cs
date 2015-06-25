namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Dtos.Results;

    public interface ISettingItem
    {
        IResult Validate();

        string ToXml();
    }
}