namespace Fanex.CustomerSetting.SettingType10.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class CommissionGroupSettingItem : SettingItemDefault
    {
        private const string CommissionSettingXmlFormat = "<Discount {0}>{1}</Discount>";
        private const string CommissionGroupXmlFormat = "<Group Id=\"{0}\" Discount=\"{1}\"/>";

        public CommissionGroupSettingItem()
        {
        }

        internal CommissionGroupSettingItem(
            short sportTypeId,
            short betTypeId,
            IEnumerable<CommissionDto> groupsRawData,
            Func<int, string> commissionGroupNameGetter)
        {
            Check.Argument.IsNotNull(groupsRawData, nameof(groupsRawData));
            Check.Argument.IsNotNull(commissionGroupNameGetter, nameof(commissionGroupNameGetter));

            SportTypeId = sportTypeId;
            BetTypeId = betTypeId;
            Groups = new List<CommissionGroup>();

            if (groupsRawData.Any())
            {
                Groups = groupsRawData.Select(dto => new CommissionGroup
                {
                    Id = dto.GroupId,
                    Value = dto.GroupValue,
                    DisplayingName = commissionGroupNameGetter(dto.GroupId),
                    Rate = dto.Rate,
                    MaxRate = dto.UplineRate
                }).ToList();
            }
        }

        public List<CommissionGroup> Groups { get; set; }

        public override IResult Validate()
        {
            foreach (CommissionGroup group in Groups)
            {
                if (group.Rate > Constants.MaxCommissionRate)
                {
                    return new Result(ErrorCode.CommissionExceedsMaxValue, Message.CommissionCannotExceedMaxValue);
                }

                if (group.Rate > group.MaxRate)
                {
                    return new Result(ErrorCode.CommissionMustBeLessThanValue, string.Format(Message.CommissionMustBeLessThan, group.Rate));
                }

                if (group.Rate < 0)
                {
                    return new Result(ErrorCode.CommissionIsNegative, Message.CommissionCannotBeNegative);
                }
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            var xmlBuilder = new StringBuilder();

            foreach (CommissionGroup group in Groups)
            {
                xmlBuilder.AppendFormat(CommissionGroupXmlFormat, group.Id, group.Rate);
            }

            return string.Format(
                CommissionSettingXmlFormat,
                base.ToXml(),
                xmlBuilder.ToString());
        }
    }
}