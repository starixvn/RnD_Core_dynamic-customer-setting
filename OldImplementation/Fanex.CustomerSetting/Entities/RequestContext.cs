namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Enumerations;

    public class RequestContext
    {
        public int TargetCustId { get; set; }

        public FormType FormType { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Language { get; set; }
    }
}