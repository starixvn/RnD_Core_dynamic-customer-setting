namespace Fanex.CustomerSetting.Tests.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;

    public class SettingItemStub : ISettingItem
    {
        public int SportTypeId { get; set; }

        public byte SettingLevel { get; set; }

        public string TypeName
        {
            get { return "SettingItemStub"; }
        }

        public IResult Validate()
        {
            return Result.Success;
        }

        public string ToXml()
        {
            return string.Empty;
        }
    }
}