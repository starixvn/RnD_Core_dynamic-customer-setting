namespace Fanex.CustomerSetting.Dtos.Messages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Services;

    [XmlRoot("Root")]
    public class MessageRoot<T> : SimpleMessage
        where T : IMessageRow
    {
        [XmlElement("Row")]
        public T[] MessageRows { get; set; }

        public IEnumerable<string> ToSubMessage(IResourceService resourceService, string language, byte settingLevel)
        {
            if (MessageRows != null && MessageRows.Any())
            {
                IList<string> subMessages = new List<string>();
                foreach (IMessageRow messageRow in MessageRows)
                {
                    subMessages.Add(messageRow.ToString(resourceService, language, settingLevel));
                }

                return subMessages;
            }

            return Enumerable.Empty<string>();
        }
    }
}