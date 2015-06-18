namespace Fanex.CustomerSetting.Enumerations
{
    public enum MsgType
    {
        None = 0,

        /// <summary>
        /// <Root MsgID="10018" Msg="Wrong position taking.">
        ///     <Row SportType="1" BetType="1" IsLive="0" UserName="username1" />
        ///     <Row SportType="1" BetType="1" IsLive="0" UserName="username2" />
        /// </Root>
        /// </summary>
        PositionTakingDownline = 1,

        /// <summary>
        /// <Root MsgID=""10018""  Msg=""Wrong position taking."">
        ///     <Row SportType="1" BetType="1" IsLive="0" MinPT="10"/>
        /// </Root>
        /// </summary>
        PositionTakingMin = 2,

        /// <summary>
        /// <Root MsgID=""10018""  Msg=""Wrong position taking."">
        ///     <Row SportType=""1"" BetType=""1"" IsLive=""0"" MaxPT=""10""/>
        /// </Root>
        /// </summary>
        PositionTakingMax = 3,

        /// <summary>
        /// <Root MsgID=""10018"" UserName=""supertest1"" Msg=""Wrong position taking."">
        ///     <Row SportType=""1"" BetType=""1"" IsLive=""0"" MinPT=""10"" MaxPT=""85"" />
        /// </Root>
        /// </summary>
        PositionTakingMinMax = 4,

        /// <summary>
        /// <Root MsgID=""10018"" Msg=""Wrong position taking."">
        ///     <Row SportType=""1"" BetType=""1"" IsLive=""0"" />
        /// </Root>
        /// </summary>
        PositionTaking = 5,

        /// <summary>
        /// <Root MsgID=""1"" Msg=""Invalid Comission Rate."">
        ///        <Row SportType=""121"" BetType=""30001""/>
        /// </Root>
        /// </summary>
        Discount = 51,

        /// <summary>
        /// <Root MsgID=""10003"" Msg=""Wrong Min bet, Max bet, Max per match."">
        ///        <Row SportType=""121"" BetType=""30001""/>
        /// </Root>
        /// </summary>
        BetLimit = 71,

        /// <summary>
        /// <Root MsgID=""60031"" Msg=""Account disabled for this product. Unable to edit downlines."">
        ///        <Row SportType=""121"" BetType=""30001""/>
        /// </Root>
        /// </summary>
        Status = 91,

        /// <summary>
        /// <Root MsgID="60020" Msg="This account was disabled!"/>
        /// </summary>
        Simple = 100,

        /// <summary>
        /// This account was disabled!
        /// </summary>
        Other = 101
    }
}