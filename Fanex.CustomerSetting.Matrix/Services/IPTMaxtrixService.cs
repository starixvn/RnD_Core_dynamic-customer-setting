namespace Fanex.CustomerSetting.Matrix.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Matrix.Models;

    public interface IPTMaxtrixService
    {
        IEnumerable<PTMatrix> GetPTMatrices(string customerName, byte productId);
    }
}