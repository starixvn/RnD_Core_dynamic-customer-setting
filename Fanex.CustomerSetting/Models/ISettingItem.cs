namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Dtos.Results;

    public interface ISettingItem
    {
        /// <summary>
        /// For Controller create exactly concrete class when get json from client.
        /// </summary>
        string TypeName { get; }

        IResult Validate();

        string ToXml();
    }
}