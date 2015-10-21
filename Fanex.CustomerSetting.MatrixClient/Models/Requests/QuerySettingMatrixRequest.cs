namespace Fanex.CustomerSetting.MatrixClient.Models.Requests
{
    public class QuerySettingMatrixRequest
    {
        public byte SettingTypeId { get; set; }

        public byte ProductId { get; set; }

        public string CustomerName { get; set; }

        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public bool ShowFilter { get; set; }

        public string ToJson()
        {
            string objTpl = "{{\"settingTypeId\": {0}, \"productId\": {1}, \"customerName\": \"{2}\", \"sportTypeId\": {3}, \"betTypeId\": {4}, \"showFilter\": {5} }}";

            return string.Format(
                objTpl,
                this.SettingTypeId,
                this.ProductId,
                this.CustomerName,
                this.SportTypeId,
                this.BetTypeId,
                this.ShowFilter ? "true" : "false");
        }
    }
}