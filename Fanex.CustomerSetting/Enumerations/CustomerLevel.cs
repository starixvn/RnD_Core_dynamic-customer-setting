namespace Fanex.CustomerSetting.Enumerations
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Constants;

    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Reviewed"), 
    SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed")]
    public enum CustomerLevel : byte
    {
        Member = Level.Member,

        Agent = Level.Agent,

        Master = Level.Master,

        Super = Level.Super,
    }
}