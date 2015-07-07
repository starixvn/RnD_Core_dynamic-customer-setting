namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Results;

    [Serializable]
    public class SettingType
    {
        private const string SettingTypeXmlFormat = "<ROOT>{0}</ROOT>";

        public short Id { get; set; }

        public string Name { get; set; }

        public byte SettingTemplate { get; set; }

        public byte SettingLevel { get; set; }

        public IEnumerable<ISettingItem> SettingItems { get; set; }

        /// <summary>
        /// For Controller create exactly concrete class when get json from client.
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.GetType().AssemblyQualifiedName;
            }
        }

        public virtual string ToXml()
        {
            var settingItemsXml = string.Join(string.Empty, SettingItems.Select(settingItem => settingItem.ToXml()));

            return string.Format(SettingTypeXmlFormat, settingItemsXml);
        }

        public IResult Validate()
        {
            if (!SettingItems.Any())
            {
                return Result.NoChange;
            }

            var failedResult = GetFailedResult();

            return (failedResult == null) ? Result.Success : failedResult;
        }

        private SettingResult GetFailedResult()
        {
            var failedResults = SettingItems
                .Select(item => item.Validate())
                .Where(result => !result.IsSuccessful);

            if (!failedResults.Any())
            {
                return null;
            }

            var failedMessages = failedResults.Select(result => result.Message);

            return new SettingResult(Result.DataError.Code, Result.DataError.Message, 0, 0, Id, Name, failedMessages);
        }
    }
}