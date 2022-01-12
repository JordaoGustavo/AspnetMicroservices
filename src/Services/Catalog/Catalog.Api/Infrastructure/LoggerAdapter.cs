using Catalog.Api.Infrastructure.Interfaces;

namespace Catalog.Api.Infrastructure
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void LogError(string message)
        {
            if (!IsLogLevelEnabled(LogLevel.Error))
            {
                return;
            }

            logger.LogError(message);
        }

        public void LogError<T0>(string message, T0 arg0)
        {
            if (!IsLogLevelEnabled(LogLevel.Error))
            {
                return;
            }

            logger.LogError(message, arg0);
        }

        public void LogError<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (!IsLogLevelEnabled(LogLevel.Error))
            {
                return;
            }

            logger.LogError(message, arg0, arg1);
        }

        private bool IsLogLevelEnabled(LogLevel logLevel) => logger.IsEnabled(logLevel);
    }
}
