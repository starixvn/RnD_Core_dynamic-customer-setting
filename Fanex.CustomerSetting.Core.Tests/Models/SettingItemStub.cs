namespace Fanex.CustomerSetting.Core.Tests.Models
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class SettingItemStub : ISettingItem
    {
        public int SportTypeId { get; set; }

        public byte SettingLevel { get; set; }

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