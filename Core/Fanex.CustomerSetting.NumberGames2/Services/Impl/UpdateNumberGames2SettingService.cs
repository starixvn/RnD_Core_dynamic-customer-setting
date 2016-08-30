namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Exceptions;
    using Logging;
    using Models;
    using ONEbook.NG2.API.Interface;
    using Resources;
    using ApiModels = ONEbook.NG2.API.DTO;
    using ApiServices = ONEbook.NG2.API;

    public class UpdateNumberGames2SettingService : UpdateSettingService, IUpdateSettingService
    {
        private const short ProductStatusSettingType = 11;
        private const short CommissionGroupSettingType = 10;

        private readonly ICustomerSyncStatusService _customerSyncStatusService;
        private readonly IThirdPartyServices _apiService;

        public UpdateNumberGames2SettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null, null, null)
        {
        }

        public UpdateNumberGames2SettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            ITaskService pluggableTaskService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null,
            ICustomerSyncStatusService customerSyncStatusService = null,
            IThirdPartyServices apiService = null)
            : base(settingContext, validationService, pluggableTaskService, logger, settingTypeServiceFactory)
        {
            _customerSyncStatusService = customerSyncStatusService ?? new CustomerSyncStatusService();
            _apiService = apiService ?? new ApiServices.Services();
        }

        public override IResult Update(UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            CustomerSyncStatus syncStatus = _customerSyncStatusService.GetCustomerSyncStatus(
                command.Setting.CustomerId,
                command.Setting.ProductId);

            if (!syncStatus.IsSyncedUpline)
            {
                throw new CustomerSettingException(
                    $"Product {command.Setting.ProductId} has not been offered for Upline of Customer {command.Setting.CustomerId}.");
            }

            if (syncStatus.IsSynced)
            {
                return base.Update(command);
            }

            var settingTypeResults = new List<SettingResult>();

            SyncCustomer(command, settingTypeResults);
            UpdateCustomerSettingTypes(
                command.Setting.CustomerId,
                command.Setting.CustomerInfo,
                command.Setting.CustomerLevel,
                command.Setting.ProductId,
                command.UplineLevel,
                command.Setting.SettingTypes.Where(settingType => settingType.Id != ProductStatusSettingType),
                settingTypeResults);

            return new UpdateSettingResult(settingTypeResults);
        }

        public override IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));
            Check.Argument.IsNotNull(command.Setting, nameof(command.Setting));
            Check.Argument.IsNotNull(command.Setting.SettingTypes, nameof(command.Setting.SettingTypes));
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var results = new List<IResult>();

            foreach (var customerId in command.CustomerIds)
            {
                var updateOneCustomerSettingCommand = command.GetUpdateCustomerSettingCommand(customerId);
                var result = Update(updateOneCustomerSettingCommand);

                results.Add(result);
            }

            return new UpdateSettingResult(results);
        }

        private void SyncCustomer(UpdateCustomerSettingCommand command, List<SettingResult> settingTypeResults)
        {
            ApiModels.Request request = CreateApiRequest(command);
            ApiModels.IResponse response = _apiService.SyncCustomers(request);
            SettingType productStatusSettingType = command.Setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == ProductStatusSettingType);
            bool syncResult = response?.Result?.ResultDetail?.All(detail => detail.IsSuccess) == true;

            if (syncResult)
            {
                _customerSyncStatusService.UpdateCustomerSyncStatus(
                    command.Setting.CustomerId,
                    command.Setting.ProductId,
                    SettingContext.Actor.Id,
                    SettingContext.Actor.SubAccountId,
                    SettingContext.IP);

                if (productStatusSettingType != null)
                {
                    SettingResult updateProductStatusResult = UpdateCustomerSettingType(
                        command.Setting.CustomerId,
                        command.Setting.CustomerInfo,
                        command.Setting.CustomerLevel,
                        productStatusSettingType,
                        command.Setting.ProductId,
                        command.UplineLevel);

                    settingTypeResults.Add(updateProductStatusResult);
                }
            }
            else if (productStatusSettingType != null)
            {
                var updateProductStatusResult = new SettingResult(
                    ErrorCode.UnableToUpdateProductStatus,
                    Messages.UnableToUpdateProductStatus,
                    MessageType.ProductStatusViolation,
                    command.Setting.CustomerId,
                    productStatusSettingType.Id,
                    productStatusSettingType.Name);

                settingTypeResults.Add(updateProductStatusResult);
            }
        }

        private ApiModels.Request CreateApiRequest(UpdateCustomerSettingCommand command)
        {
            bool isSubAccount = SettingContext.Actor.SubAccountId > 0;
            var request = new ApiModels.Request
            {
                Auth = new ApiModels.Auth
                {
                    SiteId = MapSiteId()
                },
                Call = new ApiModels.Call
                {
                    UserId = SettingContext.Actor.Id,
                    IsSubAccount = isSubAccount,
                    Language = SettingContext.Language,
                    CustIds = new List<int> { command.Setting.CustomerId },
                    RoleId = command.Setting.CustomerLevel.Value
                    ////Currencies = new Dictionary<int, decimal> { { 0, 0 } },
                }
            };

            return request;
        }

        private void UpdateCustomerSettingTypes(
            int customerId,
            CustomerInfo customerInfo,
            Level customerLevel,
            byte productId,
            Level uplineLevel,
            IEnumerable<SettingType> settingTypes,
            List<SettingResult> settingTypeResults)
        {
            foreach (SettingType settingType in settingTypes)
            {
                var updateSettingTypeResult = UpdateCustomerSettingType(
                               customerId,
                               customerInfo,
                               customerLevel,
                               settingType,
                               productId,
                               uplineLevel);

                settingTypeResults.Add(updateSettingTypeResult);
            }
        }

        private string MapSiteId()
        {
            // Accept to hard-code the site ID here since the IDs take different values for both sides.
            string napSiteId = "2000";
            string alphaSiteId = "3000";

            return SettingContext.System == (byte)SystemType.Alpha || SettingContext.System == (byte)SystemType.InternalAlpha
                ? alphaSiteId
                : napSiteId;
        }
    }
}