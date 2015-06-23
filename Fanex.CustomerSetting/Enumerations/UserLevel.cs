namespace Fanex.CustomerSetting.Enumerations
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed")]
    public enum UserLevel : int
    {
        Agent = Constants.AgentLevel,

        Master = Constants.MasterLevel,

        Super = Constants.SuperLevel,

        Company = Constants.CompanyLevel
    }
}