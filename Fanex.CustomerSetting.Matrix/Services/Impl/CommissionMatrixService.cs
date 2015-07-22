namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.NAP.Framework.Data;

    public class CommissionMatrixService : ICommissionMatrixService
    {
        private const string DefaultLanguage = "en-US";
        private const byte SettingTypeId = 3;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResourceService _resourceService;
        private readonly IProductService _productService;

        public CommissionMatrixService(
            IResourceService resourceService = null,
            IProductService productService = null,
            IDynamicRepository dynamicRepository = null)
        {
            _resourceService = resourceService ?? new ResourceService();
            _productService = productService ?? new ProductService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public CommissionMatrix GetCommissionMatrix(byte productId, string customerName)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");
            Check.Argument.IsNotEmpty(customerName, "customerName");

            var settingType = GetSettingType(productId);
            var commissionItemMaxtrixDtos = GetCommissionItemMaxtrixDtos(productId, customerName);
            var commissionItemMatrices = GetCommissionItemMatricesFromDto(commissionItemMaxtrixDtos);

            return new CommissionMatrix
            {
                CommissionItemMatrices = commissionItemMatrices,
                SettingLevel = settingType.SettingLevel
            };
        }

        private IEnumerable<CommissionItemMatrix> GetCommissionItemMatricesFromDto(
            IEnumerable<CommissionItemMatrixDto> commissionItemMaxtrixDtos)
        {
            var commissionItemMatrices = commissionItemMaxtrixDtos.GroupBy(dto => new
            {
                dto.BetType,
                dto.SportType
            }).Select(groupedDto => new CommissionItemMatrix
            {
                BetTypeId = groupedDto.Key.BetType,
                BetTypeName = _resourceService.GetBetTypeName(groupedDto.Key.BetType, DefaultLanguage),
                SportTypeId = groupedDto.Key.SportType,
                SportTypeName = _resourceService.GetSportTypeName(groupedDto.Key.SportType, DefaultLanguage),
                CommissionItemMatrixRows = groupedDto.ToList().Select(commissionItemMatrixDto => new CommissionItemMatrixRow(commissionItemMatrixDto))
            });

            return commissionItemMatrices;
        }

        private SettingType GetSettingType(byte productId)
        {
            var commissionSettingType = _productService.GetProductSettingType(productId, SettingTypeId);

            if (commissionSettingType == null)
            {
                throw new InvalidOperationException("Position Taking Setting Type information cannot be null");
            }

            return commissionSettingType;
        }

        private IEnumerable<CommissionItemMatrixDto> GetCommissionItemMaxtrixDtos(
            byte productId, 
            string customerName)
        {
            var commissionItemMaxtrixDtos = _dynamicRepository.Fetch<CommissionItemMatrixDto>(
                new CommissionMatrixQueryCriteria
                {
                    ProductId = productId,
                    UserName = customerName
                });

            return commissionItemMaxtrixDtos;
        }
    }
}