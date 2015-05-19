namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Matrix.Entities;
    using Fanex.CustomerSetting.Matrix.Models;

    public interface IPositionTakingMatrixService
    {
        PositionTakingMatrixEntity Query(QueryInfo queryInfo);
    }
}