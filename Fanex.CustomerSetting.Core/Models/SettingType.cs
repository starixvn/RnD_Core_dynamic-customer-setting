namespace Fanex.CustomerSetting.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Dtos.Results.Impl;

    public class SettingType
    {
        public byte Id { get; set; }

        public byte SettingTemplate { get; set; }

        private byte SettingLevel { get; set; }

        public IEnumerable<ISettingItem> SettingItems { get; set; }

        public virtual string ToXml()
        {
            return string.Empty;
        }

        public IResult Validate()
        {
            if (!SettingItems.Any())
            {
                return Result.Success;
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

            return GetFailedValidationResult(failedValidationResults);
        }

        private IResult GetFailedValidationResult(IEnumerable<IResult> failedValidationResults)
        {
            var firstResult = failedValidationResults.FirstOrDefault();
            return new Result(firstResult.Code, firstResult.Message, failedValidationResults.Select(result => result.Message));
        }
    }
}