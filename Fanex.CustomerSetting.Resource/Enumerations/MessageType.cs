namespace Fanex.CustomerSetting.Resource.Enumerations
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed")]
    public enum MessageType : int
    {
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