namespace Fanex.CustomerSetting.MatrixClient.Models.Requests
{
    public class QuerySettingMatrixRequest
    {
        public byte SettingTypeId { get; set; }

        public byte ProductId { get; set; }

        public string CustomerName { get; set; }
    }
}