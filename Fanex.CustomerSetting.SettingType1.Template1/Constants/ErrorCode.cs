namespace Fanex.CustomerSetting.SettingType1.Constants
{
    internal static class ErrorCode
    {
        public const int PTExceedMaxValue = 1014;

        //////// Edit Super
        public const int MasterMaxPTIsGreaterThanSuperMaxPT = 1001;
        public const int AgentMaxPTIsGreaterThanMasterMaxPT = 1002;
        public const int SuperMinPTIsGreaterThanSuperMaxPTErrorCode = 1003;

        //// Edit Master
        public const int SuperPTIsGreaterThanZeroWhenIsAutoIsSet = 1004;
        public const int SuperAutoPTIsGreaterThanZeroWhenIsAutoIsNotSet = 1005;
        public const int MasterMinPTIsGreaterThanMasterMaxPT = 1006;

        //// Edit Agent
        public const int MasterPTIsGreaterThanZeroWhenIsAutoIsSet = 1007;
        public const int MasterAutoPTIsGreaterThanZeroWhenIsAutoIsNotSet = 1008;
        public const int AgentMinPTIsGreaterThanAgentMaxPT = 1009;

        //// Super Edit Member
        public const int SuperPTOnMemberIsGreaterThanZeroWhenIsAutoIsSet = 1010;
        public const int SuperAutoPTOnMemberIsGreaterThanZeroWhenIsAutoIsNotSet = 1011;

        //// Master Edit Member
        public const int MasterPTOnMemberIsGreaterThanZeroWhenIsAutoIsSet = 1012;
        public const int MasterAutoPTOnMemberIsGreaterThanZeroWhenIsAutoIsNotSet = 1013;
    }
}
