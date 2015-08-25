namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Resources;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "PT is abbriviation for Position Taking.")]
    internal class PTMatrixService : IPTMatrixService
    {
        private const string DefaultLanguage = "en-US";
        private const byte SettingTypeId = 1;
        private readonly IResourceService _resourceService;
        private readonly IProductService _productService;
        private readonly IDynamicRepository _dynamicRepository;

        public PTMatrixService(
            IResourceService resourceService = null,
            IProductService productService = null,
            IDynamicRepository dynamicRepository = null)
        {
            _resourceService = resourceService ?? new ResourceService();
            _productService = productService ?? new ProductService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public PTMatrix GetPTMatrix(byte productId, string customerName)
        {
            var settingType = GetSettingType(productId);
            var isAllowAutoMasterPT = GetIsAllowMasterAutoPT(customerName);
            var pTItemMatrixDtos = GetPTItemMatrixDtos(customerName, productId);
            var pTItemMatrices = GetPTItemMatricesFromDto(pTItemMatrixDtos);

            return new PTMatrix
                {
                    SettingLevel = settingType.SettingLevel,
                    IsAllowMasterAutoPT = isAllowAutoMasterPT,
                    PTItemMatrices = pTItemMatrices
                };
        }

        private SettingType GetSettingType(byte productId)
        {
            var ptSettingType = _productService.GetProductSettingType(productId, SettingTypeId);

            if (ptSettingType == null)
            {
                throw new InvalidOperationException(Message.ProductDoesNotHavePTSetting);
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

        private IEnumerable<PTItemMatrix> GetPTItemMatricesFromDto(IEnumerable<PTItemMatrixDto> pTItemMatrixDtos)
        {
            var pTItemMatrices = pTItemMatrixDtos.GroupBy(dtoRow => new
            {
                dtoRow.SportType,
                dtoRow.BetType,
                dtoRow.IsLive
            }).Select(group => new PTItemMatrix
            {
                BetTypeId = group.Key.BetType,
                BetTypeName = _resourceService.GetBetTypeName(group.Key.BetType, DefaultLanguage),
                SportTypeId = group.Key.SportType,
                SportTypeName = _resourceService.GetSportTypeName(group.Key.SportType, DefaultLanguage),
                IsLive = group.Key.IsLive,
                PTItemMatrixRows = group.ToList().Select(ptItemMatrixRowDto => new PTItemMatrixRow(ptItemMatrixRowDto))
            });

            return pTItemMatrices;
        }

        private IEnumerable<PTItemMatrixDto> GetPTItemMatrixDtos(
            string customerName, 
            byte productId)
        {
            var pTItemMatrixDtos = _dynamicRepository.Fetch<PTItemMatrixDto>(
                new PTMatrixQueryCriteria
            {
                ProductId = productId,
                UserName = customerName
            });

            return pTItemMatrixDtos;
        }
    }
}