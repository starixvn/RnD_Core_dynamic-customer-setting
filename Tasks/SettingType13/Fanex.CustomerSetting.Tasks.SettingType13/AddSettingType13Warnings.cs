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

    public class AddSettingType13Warnings : IPostQueryCustomerSettingTask, IPostUpdateCustomerSettingTask, IPostUpdateMultipleCustomersSettingTask
    {
        internal const short SettingType13Id = 13;
        internal const int SuccessfulProductStatusUpdateWarningCode = -9101;
        internal const int ProductStatusNoteForExternalSitesCode = -9102;

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));

            if (FilterSettingType13OnExternalSites(settingContext, setting)?.DisplayMode == DisplayMode.Editable)
            {
                setting.WarningMessage = new Result(ProductStatusNoteForExternalSitesCode, Resources.ProductStatusNoteForExternalSites);
            }
        }

        public void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command, IResult result)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(result, nameof(result));

            AddWarningsForSuccessfulSettingType13Updates(settingContext, command, result);
        }

        public void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult result)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(result, nameof(result));

            AddWarningsForSuccessfulSettingType13Updates(settingContext, command, result);
        }

        private void AddWarningsForSuccessfulSettingType13Updates(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult updateResult)
        {
            var updateSettingResult = updateResult as UpdateSettingResult;
            SettingType settingType13OnExternalSite = FilterSettingType13OnExternalSites(settingContext, command.Setting);

            if (updateSettingResult == null
                || settingType13OnExternalSite == null
                || !DidUserEnableDownlineProductStatus(settingType13OnExternalSite))
            {
                return;
            }

            var successfulSettingType13Updates = FilterSuccessfulProductStatusResults(updateSettingResult);

            if (successfulSettingType13Updates.Any())
            {
                var updateSettingSubResults = updateSettingResult.SubResults as List<SettingResult>;

                if (updateSettingSubResults != null)
                {
                    foreach (SettingResult successfulResult in successfulSettingType13Updates)
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

        private SettingType FilterSettingType13OnExternalSites(ISettingContext settingContext, ISetting setting)
        {
            if (settingContext.System == (byte)SystemType.Nap)
            {
                return null;
            }

            return setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == SettingType13Id);
        }

        private bool DidUserEnableDownlineProductStatus(SettingType settingType)
        {
            if (settingType.SettingItems.Any() == true)
            {
                foreach (ISettingItem settingItem in settingType.SettingItems)
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

        private List<SettingResult> FilterSuccessfulProductStatusResults(UpdateSettingResult updateResult)
            => updateResult.SubResults
                .Where(subResult => subResult.SettingTypeId == SettingType13Id && subResult.IsSuccessful)
                .ToList();
    }
}