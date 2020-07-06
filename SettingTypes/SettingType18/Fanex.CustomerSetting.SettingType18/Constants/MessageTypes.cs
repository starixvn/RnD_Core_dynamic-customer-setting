namespace Fanex.CustomerSetting.SettingType18.Constants
{
    internal static class MessageTypes
    {
        /// <summary>
        /// <Root MsgID="10018" Msg="Wrong position taking.">
        ///     <Row SportType="1" BetType="1" IsLive="0" UserName="username1" />
        ///     <Row SportType="1" BetType="1" IsLive="0" UserName="username2" />
        /// </Root>
        /// </summary>
        public const int DownlinePTViolation = 1;

        /// <summary>
        /// <Root MsgID="10018"  Msg="Wrong position taking.">
        ///     <Row SportType="1" BetType="1" IsLive="0" MinPT="10"/>
        /// </Root>
        /// </summary>
        public const int MinPTViolation = 2;

        /// <summary>
        /// <Root MsgID="10018"  Msg="Wrong position taking.">
        ///     <Row SportType="1" BetType="1" IsLive="0" MaxPT="10"/>
        /// </Root>
        /// </summary>
        public const int MaxPTViolation = 3;

        /// <summary>
        /// <Root MsgID="10018" UserName="supertest1" Msg="Wrong position taking.">
        ///     <Row SportType="1" BetType="1" IsLive="0" MinPT="10" MaxPT="85" />
        /// </Root>
        /// </summary>
        public const int MinMaxPTViolation = 4;

        /// <summary>
        /// <Root MsgID="10018" Msg="Wrong position taking.">
        ///     <Row SportType="1" BetType="1" IsLive="0" />
        /// </Root>
        /// </summary>
        public const int WrongPT = 5;
    }
}