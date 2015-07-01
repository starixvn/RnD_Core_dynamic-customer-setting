namespace Fanex.CustomerSetting.Dtos.Messages
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Reviewed !"), XmlElement("Row")]
        public T[] SubMessages { get; set; }

        public IEnumerable<string> GetSubMessages(IResourceService resourceService, string language, byte settingLevel)
        {
            if (SubMessages != null && SubMessages.Any())
            {
                var subMessages = new List<string>();
                foreach (ISubMessage subMessage in SubMessages)
                {
                    subMessages.Add(subMessage.ToString(resourceService, language, settingLevel));
                }

                return subMessages;
            }

            return Enumerable.Empty<string>();
        }
    }
}