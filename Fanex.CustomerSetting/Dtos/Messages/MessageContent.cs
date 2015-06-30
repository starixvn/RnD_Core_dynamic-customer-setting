namespace Fanex.CustomerSetting.Dtos.Messages
{
    using System.Collections.Generic;

    public class MessageContent
    {
        public MessageContent(string mainMessage)
        {
            MainMessage = mainMessage;
        }

        public MessageContent(string mainMessage, IEnumerable<string> subMessages)
        {
            MainMessage = mainMessage;
            SubMessages = subMessages;
        }

        public string MainMessage { get; private set; }

        public IEnumerable<string> SubMessages { get; private set; }
    }
}