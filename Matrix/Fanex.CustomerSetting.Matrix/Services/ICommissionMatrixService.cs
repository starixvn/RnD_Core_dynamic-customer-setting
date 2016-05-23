namespace Fanex.CustomerSetting.Matrix.Services
{
    using Models;

    public interface ICommissionMatrixService
    {
        CommissionMatrix GetCommissionMatrix(byte productId, string customerName);
    }
}