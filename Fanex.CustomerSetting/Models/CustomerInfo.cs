namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Models.Enumerations;

    public class CustomerInfo
    {
        public string UserName { get; set; }

        /// <summary>
        /// Customer nick name for credit model or login name for deposit model.
        /// </summary>
        public string UserName2 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }
    }
}