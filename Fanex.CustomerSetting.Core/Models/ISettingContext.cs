namespace Fanex.CustomerSetting.Core.Models
{
    public interface ISettingContext
    {
        User Actor { get; set; }

        User SubAccount { get; set; }

        string Language { get; set; }

        string IP { get; }

        byte System { get; }
    }
}