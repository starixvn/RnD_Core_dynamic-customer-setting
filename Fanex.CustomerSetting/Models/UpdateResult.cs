namespace Fanex.CustomerSetting.Models
{
    /// <summary>
    /// UpdateResult class.
    /// </summary>
    public class UpdateResult : Result
    {
        public UpdateResult(int errCode, string errMsg)
            : base(errCode, errMsg)
        {
        }
    }
}