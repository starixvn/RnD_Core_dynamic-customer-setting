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

        public bool IsLive { get; set; }

        public string ToJson()
        {

            return string.Format(
                "{{\"settingTypeId\": {0}," +
                " \"productId\": {1}, \"customerName\": \"{2}\", \"sportTypeId\": {3}, \"betTypeId\": {4}, \"showFilter\": {5}, \"isLive\": {6} }}",
                SettingTypeId,
                ProductId,
                CustomerName,
                SportTypeId,
                BetTypeId,
                ShowFilter ? "true" : "false",
                IsLive ? "true" : "false");
        }
    }
}