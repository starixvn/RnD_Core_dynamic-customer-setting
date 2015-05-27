namespace Fanex.CustomerSetting.Matrix.Models
{
    using Fanex.CustomerSetting.Matrix.Enumerations;

    public class QueryInfo
    {
        public int ProductId { get; set; }

        public string UserName { get; set; }

        public SettingType SettingType { get; set; }
    }
}