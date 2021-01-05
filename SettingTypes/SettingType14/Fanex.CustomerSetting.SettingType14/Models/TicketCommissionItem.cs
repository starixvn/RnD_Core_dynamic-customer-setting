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
                        ErrorCodes.CommissionCannotExceedMaxValue,
                        Message.CommissionCannotBeExceed100Percent);
                }

                if (Commission < 0)
                {
                    return new Result(
                        ErrorCodes.CommisisonMustBePositive,
                        Message.CommissionMustBePositive);
                }

                if (Commission > UplineCommission)
                {
                    return new Result(
                        ErrorCodes.CommissionCannotBeGreaterThanGivenCommissionError,
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

        public override int GetHashCode()
        {
            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ (Commission?.GetHashCode() ?? 0)
                ^ UplineCommission.GetHashCode()
                ^ (NearestIndirectUplineCommission?.GetHashCode() ?? 0);
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

#pragma warning disable S1067 // Expressions should not be too complex
        public bool Equals(TicketCommissionItem other) => Equals(SportTypeId, other.SportTypeId)
                && Equals(BetTypeId, other.BetTypeId)
                && Equals(Commission, other.Commission)
                && Equals(UplineCommission, other.UplineCommission)
                && Equals(NearestIndirectUplineCommission, other.NearestIndirectUplineCommission);
#pragma warning restore S1067 // Expressions should not be too complex

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
#pragma warning disable S126 // "if ... else if" constructs should end with "else" clauses
            else if (customerLevel.IsMemberLevel())
#pragma warning restore S126 // "if ... else if" constructs should end with "else" clauses
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
#pragma warning disable S126 // "if ... else if" constructs should end with "else" clauses
            else if (uplineLevel?.IsAgentLevel() == true)
#pragma warning restore S126 // "if ... else if" constructs should end with "else" clauses
            {
                Commission = commissionDto.MemberCommission;
            }
        }
    }
}