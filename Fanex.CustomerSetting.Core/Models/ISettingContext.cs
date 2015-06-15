namespace Fanex.CustomerSetting.Core.Models
{
    public interface ISettingContext
    {
        User Actor { get; set; }

        User SubAccount { get; set; }

        string Language { get; set; }

        string Ip { get; set; }

        byte System { get; set; }
    }
}