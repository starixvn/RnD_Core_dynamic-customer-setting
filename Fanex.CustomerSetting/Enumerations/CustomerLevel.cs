namespace Fanex.CustomerSetting.Enumerations
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed")]
    public enum CustomerLevel : int
    {
        Member = Constants.MemberLevel,

        Agent = Constants.AgentLevel,

        Master = Constants.MasterLevel,

        Super = Constants.SuperLevel,
    }
}