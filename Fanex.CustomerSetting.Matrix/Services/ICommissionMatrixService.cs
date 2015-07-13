namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Matrix.Models;

    public interface ICommissionMatrixService
    {
        CommissionMatrix GetCommissionMatrix(byte productId, string customerName);
    }
}