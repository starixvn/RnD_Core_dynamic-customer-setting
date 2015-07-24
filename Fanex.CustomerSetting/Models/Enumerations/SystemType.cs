namespace Fanex.CustomerSetting.Models.Enumerations
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed !")]
    public enum SystemType : int
    {
        Accounting = 1,

        Agency = 2,

        ExternalAccounting = 3
    }
}