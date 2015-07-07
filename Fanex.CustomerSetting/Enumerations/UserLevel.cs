namespace Fanex.CustomerSetting.Enumerations
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Constants;

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed")]
    public enum UserLevel
    {
        Agent = Level.Agent,

        Master = Level.Master,

        Super = Level.Super,

        Company = Level.Company
    }
}