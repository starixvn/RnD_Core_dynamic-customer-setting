namespace Fanex.CustomerSetting.Models
{
    public interface ISettingContext
    {
        User Actor { get; set; }

        string Language { get; set; }

        string IP { get; }

        byte System { get; set; }

        string Domain { get; }
    }
}