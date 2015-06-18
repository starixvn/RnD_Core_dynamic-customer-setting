namespace Fanex.CustomerSetting.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public class SettingType
    {
        public byte Id { get; set; }

        public string Name { get; set; }

        public byte SettingTemplate { get; set; }

        public byte SettingLevel { get; set; }

        public IEnumerable<ISettingItem> SettingItems { get; set; }

        public virtual string ToXml()
        {
            return string.Empty;
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