namespace Fanex.CustomerSetting.Client40.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class SettingFormQuery
    {
        public string Language { get; set; }

        public byte ProductId { get; set; }

        public byte CustomerLevel { get; set; }

        public byte UplineLevel { get; set; }

        public bool? IsMultiple { get; set; }

        public int? CustomerId { get; set; }

        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }

        public bool IsReadOnly { get; set; }

        internal byte System { get; set; }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Reviewed!")]
        internal string ToJson()
        {
            var properties = new List<string>();

            properties.Add("{");
            properties.Add(string.Format("\"system\": {0},", this.System));
            properties.Add(string.Format("\"language\": \"{0}\",", this.Language));
            properties.Add(string.Format("\"productId\": {0},", this.ProductId));
            properties.Add(string.Format("\"customerLevel\": {0},", this.CustomerLevel));
            properties.Add(string.Format("\"uplineLevel\": {0},", this.UplineLevel));

            if (this.IsMultiple.HasValue && this.IsMultiple.Value)
            {
                properties.Add(string.Format("\"isMultiple\": {0},", this.IsMultiple.ToString().ToLowerInvariant()));
                properties.Add(string.Format("\"customerIds\": \"{0}\",", this.CustomerIds));
                properties.Add(string.Format("\"customerNames\": \"{0}\"", this.CustomerNames));
            }
            else
            {
                properties.Add(string.Format("\"customerId\": {0}", this.CustomerId));
            }

            properties.Add("}");

            return string.Join(string.Empty, properties);
        }
    }
}