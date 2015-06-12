namespace Fanex.CS.Core.Models
{
    using Fanex.NAP.Framework.Data;

    public interface ISettingItem
    {
        byte SettingLevel { get; set; }

        ICommandResult IsValid();

        string ToXml();
    }
}
