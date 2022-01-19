namespace DWP.Demo.Api.Domain.Logging
{
    public interface ILogger
    {
        void LogWarning(string message, params object[] paramsValues);
    }
}