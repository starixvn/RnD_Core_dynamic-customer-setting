namespace Fanex.CustomerSetting.Matrix.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Matrix.Models;

    public interface IPTMatrixService
    {
        PTMatrix GetPTMatrix(byte productId, string customerName);
    }
}