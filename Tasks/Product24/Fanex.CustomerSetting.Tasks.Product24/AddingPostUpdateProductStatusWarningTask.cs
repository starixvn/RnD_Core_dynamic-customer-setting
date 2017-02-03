namespace Fanex.CustomerSetting.Tasks.Product24
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using SettingType13.Models;

    public class AddingPostUpdateProductStatusWarningTask : IPostUpdateCustomerSettingTask, IPostUpdateMultipleCustomersSettingTask
    {
        public const int SuccessfulProductStatusUpdateWarningCode = -9101;

        private const short ProductStatusSettingType = 13;

        public void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command, IResult result)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(result, nameof(result));

            AddWarningsForSuccessfulProductStatusUpdates(settingContext, command, result);
        }

        public void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult result)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(result, nameof(result));

            AddWarningsForSuccessfulProductStatusUpdates(settingContext, command, result);
        }

        private void AddWarningsForSuccessfulProductStatusUpdates(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult updateResult)
        {
            var updateSettingResult = updateResult as UpdateSettingResult;

            if (updateSettingResult == null || !DidUserEnableDownlineProductStatus(command))
            {
                return;
            }

            var successfulProductStatusUpdates = FilterSuccessfulProductStatusResults(settingContext, updateSettingResult);

            if (successfulProductStatusUpdates.Any())
            {
                var updateSettingSubResults = updateSettingResult.SubResults as List<SettingResult>;

                if (updateSettingSubResults != null)
                {
                    foreach (SettingResult successfulResult in successfulProductStatusUpdates)
                    {
                        updateSettingSubResults.Add(new SettingResult(
                            SuccessfulProductStatusUpdateWarningCode,
                            Resources.SuccessfulProductStatusUpdateWarning,
                            successfulResult.MsgType,
                            successfulResult.CustomerId,
                            successfulResult.SettingTypeId,
                            successfulResult.SettingTypeName,
                            successfulResult.SubMessages)
                        {
                            CustomerName = successfulResult.CustomerName
                        });
                    }
                }
            }
        }

        private bool DidUserEnableDownlineProductStatus(UpdateCustomerSettingCommand command)
        {
            SettingType productStatusSettingType = command.Setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == ProductStatusSettingType);

            if (productStatusSettingType?.SettingItems.Any() == true)
            {
                foreach (ISettingItem settingItem in productStatusSettingType.SettingItems)
                {
                    var productStatusItem = settingItem as ProductStatusItem;

                    if (productStatusItem?.Downline?.IsDisabled == false)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private List<SettingResult> FilterSuccessfulProductStatusResults(ISettingContext settingContext, UpdateSettingResult updateResult)
            => updateResult.SubResults
                .Where(subResult => subResult.SettingTypeId == ProductStatusSettingType && subResult.IsSuccessful && settingContext.System != (byte)SystemType.Nap)
                .ToList();
    }
}