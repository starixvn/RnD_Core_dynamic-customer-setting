namespace Fanex.CustomerSetting.SettingType1.Models
{
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType1.Constants;
    using Fanex.CustomerSetting.SettingType1.Resources;

    public class SuperPTOnMemberItem : PTItem
    {
        private const string PTXmlFormat = "<PT {0} SuperPT=\"{1}\" AutoSuperPT=\"{2}\" IsAutoSuperPT=\"{3}\"/>";

        /// <summary>
        /// Agent PT on Member
        /// </summary>
        public byte A1 { get; set; }

        /// <summary>
        /// Master PT On member
        /// </summary>
        public byte M1 { get; set; }

        /// <summary>
        /// Super PT On Member
        /// </summary>
        public byte S1 { get; set; }

        /// <summary>
        /// Super auto PT On Member
        /// </summary>
        public byte US1 { get; set; }

        /// <summary>
        /// Is Super Auto on Member.
        /// </summary>
        public bool IS1 { get; set; }

        /// <summary>
        /// Is Auto on Master.
        /// </summary>
        public bool IS3 { get; set; }

        /// <summary>
        /// Super Max PT
        /// </summary>
        public byte S4 { get; set; }

        /// <summary>
        /// Super Min PT
        /// </summary>
        public byte N4 { get; set; }

        /// <summary>
        /// Master Max PT
        /// </summary>
        public byte M3 { get; set; }

        public override IResult Validate()
        {
            if (!IS1 && US1 > 0)
            {
                return new Result(
                    ErrorCode.SuperAutoPTOnMemberIsGreaterThanZeroWhenIsAutoIsNotSet,
                    Message.SuperAutoPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsNotSet);
            }

            if (IS1 && S1 > 0)
            {
                return new Result(
                    ErrorCode.SuperPTOnMemberIsGreaterThanZeroWhenIsAutoIsSet,
                    Message.SuperPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsSet);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                PTXmlFormat,
                base.ToXml(),
                S1,
                US1,
                IS1 ? 1 : 0);
        }
    }
}