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

        #endregion Variables

        #region Constructors

        public ValidationService()
            : this(null, null)
        {
        }

        public ValidationService(
            IProductProfileService productProfileService,
            IValidationRepository validationRepository)
        {
            _productProfileService = productProfileService ?? new ProductProfileService();
            _validationRepository = validationRepository ?? new ValidationRepository();
        }

        #endregion Constructors

        public Result Validate(QueryInfo model)
        {
            var result = ValidateRole(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidatePermission(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result.Success;
        }

        public Result Validate(CommandInfo model)
        {
            var result = ValidateForm(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateRole(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidatePermission(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateProductProfile(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result.Success;
        }

        private Result ValidateForm(CommandInfo model)
        {
            return (model.FormType == FormType.SuperEditMember || model.FormType == FormType.MasterEditMember)
                && model.Discounts != null
                && model.Discounts.Count() > 0
                ? Result.AccessDenied
                : Result.Success;
        }

        private Result ValidateRole(QueryInfo model)
        {
            var formHandler = FormHandlerFactory.Create(model.FormType);
            return formHandler.HasPermission(model)
                ? Result.Success
                : Result.AccessDenied;
        }

        private Result ValidatePermission(QueryInfo model)
        {
            var criteria = new PermissionValidateCriteria
            {
                CustId = model.CustId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = (int)model.TargetRoleId
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
        private Result ValidateProductProfile(CommandInfo model)
        {
            var productProfile = _productProfileService.GetProductProfile(model.ProductId);
            switch (model.CommandType)
            {
                case CommandType.All:
                    IEnumerable<BaseEntity>[] dataList =
                    {
                        model.PositionTakings,
                        model.BetLimits,
                        model.Discounts,
                        model.Statuses
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
                    if (model.PositionTakings == null || model.PositionTakings.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.PositionTakings, productProfile);

                case CommandType.BetLimit:
                    if (model.BetLimits == null || model.BetLimits.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.BetLimits, productProfile);

                case CommandType.Discount:
                    if (model.Discounts == null || model.Discounts.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.Discounts, productProfile);

                case CommandType.Status:
                    if (model.Statuses == null || model.Statuses.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.Statuses, productProfile);
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