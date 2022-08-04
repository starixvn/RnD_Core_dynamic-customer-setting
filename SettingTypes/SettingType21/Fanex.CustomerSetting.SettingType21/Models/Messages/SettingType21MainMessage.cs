namespace Fanex.CustomerSetting.SettingType21.Models.Messages
{
    using CustomerSetting.Models.Messages;
    using Fanex.CustomerSetting.Services;
    using System.Xml.Serialization;
    using System.Linq;
    using System.Collections.Generic;

    [XmlRoot("Root")]
    public class SettingType21MainMessage : MainMessage<ProductStatusViolationSubMessage>
    {
        public IEnumerable<string> TranslateSubMessages(IResourceService resourceService, string language, string templateMessage)
        {
            if (SubMessages == null || !SubMessages.Any()) return Enumerable.Empty<string>();

            var messages = new List<string>();
            var parentSportTypeGroups = SubMessages.GroupBy(row => new
            {
                row.ParentSportType,
                row.ParentBetType,
            });

            foreach (var parentSportType in parentSportTypeGroups)
            {
                var parentSportTypeName = resourceService.GetSportTypeName(parentSportType.Key.ParentSportType, language);
                var childSportTypeNames = parentSportType
                    .Select(subMessage => resourceService.GetSportTypeName(subMessage.SportType, language)).ToList();

                var builtMessage = string.Format(templateMessage, string.Join(", ", childSportTypeNames), parentSportTypeName);
                
                messages.Add(builtMessage);
            }

            return messages;
        }
    }
}