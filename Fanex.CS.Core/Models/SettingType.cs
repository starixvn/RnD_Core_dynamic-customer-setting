namespace Fanex.CS.Core.Models
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;

    public class SettingType
    {
        public string Id { get; set; }

        public byte SettingTemplate { get; set; }

        public IList<ISettingItem> SettingItems { get; set; }

        public virtual string ToXml()
        {
            return string.Empty;
        }

        public ICommandResult IsValid()
        {
            return null;
        }
    }
}