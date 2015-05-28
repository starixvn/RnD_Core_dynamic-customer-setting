namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.FormTypes;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;

    public class ValidationService : IValidationService
    {
        #region Variables

        private readonly IProductProfileService _productProfileService;
        private readonly IValidationRepository _validationRepository;
        private readonly FormHandlerFactory _formHandlerFactory;

        #endregion Variables

        #region Constructors

        public ValidationService()
            : this(null, null, null)
        {
        }

        public ValidationService(
            IProductProfileService productProfileService,
            IValidationRepository validationRepository,
            FormHandlerFactory formHandlerFactory)
        {
            _productProfileService = productProfileService ?? new ProductProfileService();
            _validationRepository = validationRepository ?? new ValidationRepository();
            _formHandlerFactory = formHandlerFactory ?? new FormHandlerFactory();
        }

        #endregion Constructors

        public Result Validate(QueryInfo queryInfo)
        {
            var result = ValidateRole(queryInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidatePermission(queryInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result.Success;
        }

        public Result Validate(CommandInfo commandInfo)
        {
            var result = ValidateForm(commandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateRole(commandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidatePermission(commandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateProductProfile(commandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result.Success;
        }

        public Result Validate(MultipleCommandInfo multipleCommandInfo)
        {
            var commandInfo = new CommandInfo(multipleCommandInfo);
            commandInfo.TargetCustId = multipleCommandInfo.TargetCustIds.First();
            var result = ValidateForm(commandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateRole(commandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidatePermission(multipleCommandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateProductProfile(commandInfo);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result.Success;
        }

        private Result ValidateForm(CommandInfo commandInfo)
        {
            return (commandInfo.FormType == FormType.SuperEditMember || commandInfo.FormType == FormType.MasterEditMember)
                && commandInfo.Discounts != null
                && commandInfo.Discounts.Count() > 0
                ? Result.AccessDenied
                : Result.Success;
        }

        private Result ValidateRole(QueryInfo queryInfo)
        {
            var formHandler = _formHandlerFactory.Create(queryInfo.FormType);
            return formHandler.HasValidRole(queryInfo)
                ? Result.Success
                : Result.AccessDenied;
        }

        private Result ValidatePermission(QueryInfo queryInfo)
        {
            var criteria = new PermissionValidateCriteria
            {
                CustId = queryInfo.CustId,
                TargetCustId = queryInfo.TargetCustId,
                TargetRoleId = (int)queryInfo.TargetRoleId
            };

            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _validationRepository.ValidatePermission(criteria);
            return new Result(criteria.ErrCode, criteria.ErrMsg);
        }

        private Result ValidatePermission(MultipleCommandInfo multipleCommandInfo)
        {
            var criteria = new PermissionValidateMultipleCriteria
            {
                CustId = multipleCommandInfo.CustId,
                TargetCustIds = string.Join(",", multipleCommandInfo.TargetCustIds),
                TargetRoleId = (int)multipleCommandInfo.TargetRoleId
            };

            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _validationRepository.ValidatePermission(criteria);
            return new Result(criteria.ErrCode, criteria.ErrMsg);
        }

        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Switch has multiple cases.")]
        private Result ValidateProductProfile(CommandInfo commandInfo)
        {
            var productProfile = _productProfileService.GetProductProfile(commandInfo.ProductId);
            switch (commandInfo.CommandType)
            {
                case CommandType.All:
                    IEnumerable<BaseEntity>[] dataList =
                    {
                        commandInfo.PositionTakings,
                        commandInfo.BetLimits,
                        commandInfo.Discounts,
                        commandInfo.Statuses
                    };

                    var hasData = false;
                    foreach (var data in dataList)
                    {
                        if (data != null && data.Count() > 0)
                        {
                            hasData = true;
                            break;
                        }
                    }

                    if (!hasData)
                    {
                        return Result.NoChange;
                    }

                    foreach (var data in dataList)
                    {
                        var result = ValidateProductProfile(data, productProfile);
                        if (!result.IsSuccess)
                        {
                            return result;
                        }
                    }

                    return Result.Success;

                case CommandType.PositionTaking:
                    if (commandInfo.PositionTakings == null || commandInfo.PositionTakings.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(commandInfo.PositionTakings, productProfile);

                case CommandType.BetLimit:
                    if (commandInfo.BetLimits == null || commandInfo.BetLimits.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(commandInfo.BetLimits, productProfile);

                case CommandType.Discount:
                    if (commandInfo.Discounts == null || commandInfo.Discounts.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(commandInfo.Discounts, productProfile);

                case CommandType.Status:
                    if (commandInfo.Statuses == null || commandInfo.Statuses.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(commandInfo.Statuses, productProfile);
            }

            return Result.Success;
        }

        private Result ValidateProductProfile(IEnumerable<BaseEntity> entities, ProductProfileEntity productProfile)
        {
            if (productProfile == null || productProfile.Items.Count() == 0)
            {
                return Result.Error;
            }

            if (entities == null)
            {
                return Result.Success;
            }

            foreach (var entity in entities)
            {
                if (!entity.IsValid() || !productProfile.Items.Where(profileItem => profileItem.SportType == entity.SportType && profileItem.BetType == entity.BetType).Any())
                {
                    return Result.AccessDenied;
                }
            }

            return Result.Success;
        }
    }
}