namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Collections.Generic;

    public class CustomerSettingMatrix
    {
        public IEnumerable<Customer> CustomerLine { get; set; }

        public SettingTypeMatrix SettingTypeMatrix { get; set; }
    }
}