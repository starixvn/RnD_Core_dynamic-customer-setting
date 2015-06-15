namespace Fanex.CustomerSetting.Core.Results
{
    public interface IResult
    {
        int ErrCode { get; }

        string ErrMsg { get; }

        bool IsSuccessful { get; }
    }
}