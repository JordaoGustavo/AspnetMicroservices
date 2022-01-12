namespace Catalog.Api.Infrastructure.Interfaces
{
    public interface ILoggerAdapter<T>
    {
        void LogError(string message);

        void LogError<T0>(string message, T0 arg0);

        void LogError<T0, T1>(string message, T0 arg0, T1 arg1);
    }
}
