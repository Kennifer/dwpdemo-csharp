namespace DWP.Demo.Api.Logging
{
    public interface ILogger
    {
        void LogWarning(string message, params object[] paramsValues);
    }
}