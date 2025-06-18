namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Criteria;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Dtos;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.Logging;
    using Models;
    using Resources;


    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules",
                 "SA1305:FieldNamesMustNotUseHungarianNotation",
                 Justification = "PT is abbreviation for Position Taking.")]
    internal class PTMatrixService : IPTMatrixService
    {
        private const string DefaultLanguage = "en-US";
        private readonly IResourceService _resourceService;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly ILogger _logger; // Added logger field  

        public PTMatrixService(
            IResourceService resourceService = null,
            IDynamicRepository dynamicRepository = null,
            ILogger logger = null) // Added ILogger parameter  
        {
            _resourceService = resourceService ?? new ResourceService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _logger = logger ?? new Logger(); // Initialize logger  
        }

        public PTMatrix GetPTMatrix(SettingMatrixQuery settingMatrixQuery, string masterName = null, DirectMember directMemberEnum = DirectMember.None)
        {
            _logger.Error("Error in GetPTMatrix");

            var isAllowAutoMasterPT = GetIsAllowMasterAutoPT(masterName);
            var pTItemMatrixDtos = GetPTItemMatrixDtos(settingMatrixQuery);
            var pTItemMatrices = GetPTItemMatricesFromDto(pTItemMatrixDtos, directMemberEnum);

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

        private IEnumerable<PTItemMatrix> GetPTItemMatricesFromDto(IEnumerable<PTItemMatrixDto> pTItemMatrixDtos, DirectMember directMemberEnum)
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
                PTItemMatrixRows = group.Select(ptItemMatrixRowDto => new PTItemMatrixRow(ptItemMatrixRowDto, directMemberEnum))
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