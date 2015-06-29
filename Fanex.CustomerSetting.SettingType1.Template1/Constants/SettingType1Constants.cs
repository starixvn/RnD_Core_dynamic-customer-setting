namespace Fanex.CustomerSetting.SettingType1.Template1.Constants
{
    internal static class SettingType1Constants
    {
        public const byte SettingType = 1;
        public const int MaxPositionTakingValue = 90;

        public const int PositionTakingExceedMaxValueErrorCode = 1014;

        //////// Edit Super
        public const int MasterMaxPTIsGreaterThanSuperMaxPTErrorCode = 1001;
        public const int AgentMaxPTIsGreaterThanMasterMaxPTErrorCode = 1002;
        public const int SuperMinPTIsGreaterThanSuperMaxPTErrorCode = 1003;

        //// Edit Master
        public const int SuperPTIsGreaterThanZeroWhenIsAutoIsSetErrorCode = 1004;
        public const int SuperAutoPTIsGreaterThanZeroWhenIsAutoIsNotSetErrorCode = 1005;
        public const int MasterMinPTIsGreaterThanMasterMaxPTErrorCode = 1006;

        //// Edit Agent
        public const int MasterPTIsGreaterThanZeroWhenIsAutoIsSetErrorCode = 1007;
        public const int MasterAutoPTIsGreaterThanZeroWhenIsAutoIsNotSetErrorCode = 1008;
        public const int AgentMinPTIsGreaterThanAgentMaxPTErrorCode = 1009;

        //// Super Edit Member
        public const int SuperPTOnMemberIsGreaterThanZeroWhenIsAutoIsSetErrorCode = 1010;
        public const int SuperAutoPTOnMemberIsGreaterThanZeroWhenIsAutoIsNotSetErrorCode = 1011;

        //// Master Edit Member
        public const int MasterPTOnMemberIsGreaterThanZeroWhenIsAutoIsSetErrorCode = 1012;
        public const int MasterAutoPTOnMemberIsGreaterThanZeroWhenIsAutoIsNotSetErrorCode = 1013;
    }
}
