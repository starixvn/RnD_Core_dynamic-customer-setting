namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Resources;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.Data.Repository;

    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", 
        "SA1305:FieldNamesMustNotUseHungarianNotation", 
        Justification = "PT is abbriviation for Position Taking.")]
    internal class PTMatrixService : IPTMatrixService
    {
        private const string DefaultLanguage = "en-US";
        private readonly IResourceService _resourceService;
        private readonly IDynamicRepository _dynamicRepository;

        public PTMatrixService(
            IResourceService resourceService = null,
            IDynamicRepository dynamicRepository = null)
        {
            _resourceService = resourceService ?? new ResourceService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public PTMatrix GetPTMatrix(SettingMatrixQuery settingMatrixQuery, string masterName = null)
        {
            var isAllowAutoMasterPT = GetIsAllowMasterAutoPT(masterName);
            var pTItemMatrixDtos = GetPTItemMatrixDtos(settingMatrixQuery);
            var pTItemMatrices = GetPTItemMatricesFromDto(pTItemMatrixDtos);

            return new PTMatrix
                {
                    IsAllowMasterAutoPT = isAllowAutoMasterPT,
                    PTItemMatrices = pTItemMatrices
                };
        }

        private bool GetIsAllowMasterAutoPT(string masterName)
        {
            if (string.IsNullOrWhiteSpace(masterName))
            {
                return false;
            }

            return !_dynamicRepository.Get<bool>(new IsDisableMasterAutoPTQueryCriteria
                {
                    UserName = masterName
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

        private IEnumerable<PTItemMatrixDto> GetPTItemMatrixDtos(SettingMatrixQuery settingMatrixQuery)
        {
            var pTItemMatrixDtos = _dynamicRepository.Fetch<PTItemMatrixDto>(
                new PTMatrixQueryCriteria
            {
                ProductId = settingMatrixQuery.ProductId,
                UserName = settingMatrixQuery.CustomerName,
                IsLive = settingMatrixQuery.IsLive,
                SportType = settingMatrixQuery.SportTypeId,
                BetType = settingMatrixQuery.BetTypeId
            });

            if (!pTItemMatrixDtos.Any())
            {
                throw new InvalidOperationException(Message.ThereIsNoData);
            }

            return pTItemMatrixDtos;
        }
    }
}