namespace Fanex.CustomerSetting.Tests.Models
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;

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
            return "SettingItemStub";
        }
    }
}