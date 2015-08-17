namespace Fanex.CustomerSetting.WebAPI.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.CustomerSetting.Models.Requests;

    public class QuerySettingRequest : SettingRequest
    {
        [Range(1, 255)]
        public byte ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Range(1, 4)]
        public byte CustomerLevel { get; set; }

        internal CustomerSettingQuery GetCustomerSettingQuery()
        {
            var query = new CustomerSettingQuery(
             this.CustomerId,
             this.CustomerLevel,
             this.ProductId,
             this.UplineLevel);

            return query;
        }
    }
}