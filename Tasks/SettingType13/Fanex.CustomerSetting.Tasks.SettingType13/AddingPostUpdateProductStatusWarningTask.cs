namespace Fanex.CustomerSetting.Tasks.SettingType13
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using CustomerSetting.SettingType13.Models;
    using Models;
    using Models.Constants;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;

    public class AddingPostUpdateProductStatusWarningTask : IPostQueryCustomerSettingTask, IPostUpdateCustomerSettingTask, IPostUpdateMultipleCustomersSettingTask
    {
        internal const int SuccessfulProductStatusUpdateWarningCode = -9101;
        internal const int ProductStatusNoteForExternalSitesCode = -9102;

        private const short ProductStatusSettingType = 13;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            SettingType productStatusSettingType = setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == ProductStatusSettingType);

            if (productStatusSettingType != null
                && settingContext.System != (byte)SystemType.Nap
                && productStatusSettingType.DisplayMode == DisplayMode.Editable)
            {
                setting.WarningMessage = new Result(ProductStatusNoteForExternalSitesCode, Resources.ProductStatusNoteForExternalSites);
            }
        }

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