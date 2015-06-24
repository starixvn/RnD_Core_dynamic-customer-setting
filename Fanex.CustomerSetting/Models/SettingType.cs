namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Results;

    public class SettingType
    {
        private const string SettingTypeXmlFormat = "<ROOT>{0}</ROOT>";

        public byte Id { get; set; }

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

            return new Result(failedValidationResults);
        }
    }
}