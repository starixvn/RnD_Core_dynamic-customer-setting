namespace Fanex.CustomerSetting.Models.Enumerations
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed !")]
    public enum SystemType
    {
        Accounting = 1,

        Agency = 2,

        ExternalAccounting = 3
    }
}