namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.WebApi.Configurations;

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
             CustomerId,
             CustomerLevel,
             ProductId,
             CustomerSettingApiConfiguration.Settings.SettingStatus,
             UplineLevel);

            return query;
        }
    }
}