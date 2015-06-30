namespace Fanex.CustomerSetting.Dtos.Messages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Services;

    /// <summary>
    /// Contains Xml data of Raw Message,
    /// Has message id to get main message
    /// Has list of sub message for more detail about error.
    /// </summary>
    /// <typeparam name="T">Implement of sub message interface.</typeparam>
    [XmlRoot("Root")]
    public class MainMessage<T> : SimpleMessage
        where T : ISubMessage
    {
        [XmlElement("Row")]
        public T[] SubMessages { get; set; }

        public IEnumerable<string> GetSubMessages(IResourceService resourceService, string language, byte settingLevel)
        {
            if (SubMessages != null && SubMessages.Any())
            {
                IList<string> subMessages = new List<string>();
                foreach (ISubMessage messageRow in SubMessages)
                {
                    subMessages.Add(messageRow.ToString(resourceService, language, settingLevel));
                }

                return subMessages;
            }

            return Enumerable.Empty<string>();
        }
    }
}