namespace Fanex.CustomerSetting.Core.Models
{
    public class Product
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsHidden { get; set; }
    }
}
