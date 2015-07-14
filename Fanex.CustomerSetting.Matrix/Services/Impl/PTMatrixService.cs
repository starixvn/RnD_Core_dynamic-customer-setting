namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "PT is abbriviation for Position Taking.")]
    public class PTMatrixService : IPTMatrixService
    {
        private const string DefaultLanguage = "en-US";
        private const byte PTSettingTypeId = 1;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResourceService _resourceService;
        private readonly IProductService _productService;

        public PTMatrixService(
            IDynamicRepository dynamicRepository = null,
            IResourceService resourceService = null,
            IProductService productService = null)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _resourceService = resourceService ?? new ResourceService();
            _productService = productService ?? new ProductService();
        }

        public PTMatrix GetPTMatrix(byte productId, string customerName)
        {
            var pTItemMatrixDtos = GetPTItemMatrixDtos(customerName, productId);
            var pTItemMatrices = GetPTItemMatricesFromDto(pTItemMatrixDtos);
            var settingType = GetPTSettingType(productId);
            var isAllowAutoMasterPT = GetIsAllowMasterAutoPT(customerName);

            return new PTMatrix
                {
                    SettingLevel = settingType.SettingLevel,
                    IsAllowMasterAutoPT = isAllowAutoMasterPT,
                    PTItemMatrices = pTItemMatrices
                };
        }

        private SettingType GetPTSettingType(byte productId)
        {
            var ptSettingType = _productService.GetProductSettingType(productId, PTSettingTypeId);

            if (ptSettingType == null)
            {
                throw new ArgumentNullException("ptSettingType", "Position Taking Setting Type information cannot be null");
            }

            return ptSettingType;
        }

        private bool GetIsAllowMasterAutoPT(string customerName)
        {
            return _dynamicRepository.Get<bool>(new IsAllowMasterAutoPTQueryCriteria
                {
                    UserName = customerName
                });
        }

        private IEnumerable<PTItemMatrix> GetPTItemMatricesFromDto(IEnumerable<PTItemMatrixDto> pTMatrixDtos)
        {
            var pTItemMatrices = pTMatrixDtos.GroupBy(ptRow => new
            {
                ptRow.SportType,
                ptRow.BetType,
                ptRow.IsLive
            }).Select(group => new PTItemMatrix
            {
                BetTypeId = group.Key.BetType,
                BetTypeName = _resourceService.GetBetTypeName(group.Key.BetType, DefaultLanguage),
                SportTypeId = group.Key.SportType,
                SportTypeName = _resourceService.GetSportTypeName(group.Key.BetType, DefaultLanguage),
                IsLive = group.Key.IsLive,
                PTItemMatrixRows = group.ToList().Select(ptItemMatrixRow => new PTItemMatrixRow(ptItemMatrixRow))
            });

            return pTItemMatrices;
        }

        private IEnumerable<PTItemMatrixDto> GetPTItemMatrixDtos(string customerName, byte productId)
        {
            var pTMatrixDtos = _dynamicRepository.Fetch<PTItemMatrixDto>(
                new PTMatrixQueryCriteria
            {
                ProductId = productId,
                UserName = customerName
            });

            return pTMatrixDtos;
        }
    }
}