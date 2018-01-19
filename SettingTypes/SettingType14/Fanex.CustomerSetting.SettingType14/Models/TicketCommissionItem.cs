namespace Fanex.CustomerSetting.SettingType14.Models
{
    using System;
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class TicketCommissionItem : SettingItemDefault, IEquatable<TicketCommissionItem>
    {
        private const int MaxCommission = 1;
        private const string CommissionXmlFormat = "<Commission {0} Commission=\"{1}\"/>";

        public TicketCommissionItem()
        {
        }

        internal TicketCommissionItem(TicketCommissionDto commissionDto, Level customerLevel, Level uplineLevel)
        {
            Check.Argument.IsNotNull(commissionDto, nameof(commissionDto));
            Check.Argument.IsNotNull(customerLevel, nameof(customerLevel));

            SportTypeId = commissionDto.SportTypeId;
            BetTypeId = commissionDto.BetTypeId;

            Map(commissionDto, customerLevel, uplineLevel);
        }

        public decimal? Commission { get; set; }

        public decimal UplineCommission { get; set; }

        public decimal? NearestIndirectUplineCommission { get; set; }

        public override IResult Validate()
        {
            if (Commission != null)
            {
                if (Commission > MaxCommission)
                {
                    return new Result(
                        ErrorCode.CommissionCannotExceedMaxValue,
                        Message.CommissionCannotBeExceed100Percent);
                }

                if (Commission < 0)
                {
                    return new Result(
                        ErrorCode.CommisisonMustBePositive,
                        Message.CommissionMustBePositive);
                }

                if (Commission > UplineCommission)
                {
                    return new Result(
                        ErrorCode.CommissionCannotBeGreaterThanGivenCommissionError,
                        Message.CommissionMustBeLessThanOrEqualToGivenCommission);
                }
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            if (Commission == null)
            {
                return string.Empty;
            }

            return string.Format(CommissionXmlFormat, base.ToXml(), Commission);
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as TicketCommissionItem;

            if (otherObj == null)
            {
                return false;
            }

            return Equals(otherObj);
        }

        public override int GetHashCode()
        {
            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ (Commission?.GetHashCode() ?? 0)
                ^ UplineCommission.GetHashCode()
                ^ (NearestIndirectUplineCommission?.GetHashCode() ?? 0);
        }

        public bool Equals(TicketCommissionItem other)
        {
            return Equals(SportTypeId, other.SportTypeId)
                && Equals(BetTypeId, other.BetTypeId)
                && Equals(Commission, other.Commission)
                && Equals(UplineCommission, other.UplineCommission)
                && Equals(NearestIndirectUplineCommission, other.NearestIndirectUplineCommission);
        }

        private void Map(TicketCommissionDto commissionDto, Level customerLevel, Level uplineLevel)
        {
            if (customerLevel.IsSuperLevel())
            {
                MapSuperCommission(commissionDto);
            }
            else if (customerLevel.IsMasterLevel())
            {
                MapMasterCommission(commissionDto);
            }
            else if (customerLevel.IsAgentLevel())
            {
                MapAgentCommission(commissionDto);
            }
            else if (customerLevel.IsMemberLevel())
            {
                MapMemberCommission(commissionDto, uplineLevel);
            }
        }

        private void MapSuperCommission(TicketCommissionDto commissionDto)
        {
            UplineCommission = commissionDto.MaxGivenCommission.GetValueOrDefault();
            Commission = commissionDto.SuperCommission.GetValueOrDefault();
        }

        private void MapMasterCommission(TicketCommissionDto commissionDto)
        {
            UplineCommission = commissionDto.SuperCommission.GetValueOrDefault();
            Commission = commissionDto.MasterCommission.GetValueOrDefault();
        }

        private void MapAgentCommission(TicketCommissionDto commissionDto)
        {
            UplineCommission = commissionDto.MasterCommission.GetValueOrDefault();
            Commission = commissionDto.AgentCommission.GetValueOrDefault();
        }

        private void MapMemberCommission(TicketCommissionDto commissionDto, Level uplineLevel)
        {
            UplineCommission = commissionDto.AgentCommission.GetValueOrDefault();

            if (uplineLevel?.IsSuperLevel() == true)
            {
                NearestIndirectUplineCommission = commissionDto.MasterCommission;
            }
            else if (uplineLevel?.IsAgentLevel() == true)
            {
                Commission = commissionDto.MemberCommission;
            }
        }
    }
}