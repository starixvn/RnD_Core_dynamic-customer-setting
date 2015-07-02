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

            return GetValidationResult();
        }

        private IResult GetValidationResult()
        {
            var failedValidationResults = SettingItems
                .Select(item => item.Validate())
                .Where(result => !result.IsSuccessful);
            if (!failedValidationResults.Any())
            {
                return Result.Success;
            }

            var subMessages = failedValidationResults.Select(result => result.Message);
            return new SettingResult(Result.DataError.Code, Result.DataError.Message, 0, 0, string.Empty, Id, Name, subMessages);
        }
    }
}