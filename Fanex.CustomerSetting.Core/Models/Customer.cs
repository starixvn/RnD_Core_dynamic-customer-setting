namespace Fanex.CustomerSetting.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }

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