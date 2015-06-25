namespace Fanex.CustomerSetting.SettingType1.Template1.Enumerations
{
    internal enum PositionTakingMessageType : int
    {
        /// <summary>
        /// <Root MsgID="10018" Msg="Wrong position taking.">
        ///     <Row SportType="1" BetType="1" IsLive="0" UserName="username1" />
        ///     <Row SportType="1" BetType="1" IsLive="0" UserName="username2" />
        /// </Root>
        /// </summary>
        DownlinePTViolation = 1,

        /// <summary>
        /// <Root MsgID=""10018""  Msg=""Wrong position taking."">
        ///     <Row SportType="1" BetType="1" IsLive="0" MinPT="10"/>
        /// </Root>
        /// </summary>
        MinPTViolation = 2,

        /// <summary>
        /// <Root MsgID=""10018""  Msg=""Wrong position taking."">
        ///     <Row SportType=""1"" BetType=""1"" IsLive=""0"" MaxPT=""10""/>
        /// </Root>
        /// </summary>
        MaxPTViolation = 3,

        /// <summary>
        /// <Root MsgID=""10018"" UserName=""supertest1"" Msg=""Wrong position taking."">
        ///     <Row SportType=""1"" BetType=""1"" IsLive=""0"" MinPT=""10"" MaxPT=""85"" />
        /// </Root>
        /// </summary>
        MinMaxPTViolation = 4,

        /// <summary>
        /// <Root MsgID=""10018"" Msg=""Wrong position taking."">
        ///     <Row SportType=""1"" BetType=""1"" IsLive=""0"" />
        /// </Root>
        /// </summary>
        WrongPositionTaking = 5,
    }
}