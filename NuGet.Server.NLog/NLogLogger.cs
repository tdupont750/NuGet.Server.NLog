using System;
using NuGet.Server.Logging;

namespace NuGet.Server.NLog
{
    public class NLogLogger : Logging.ILogger, ILogger
    {
        private readonly global::NLog.ILogger _logger;

        public NLogLogger(global::NLog.ILogger logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel level, string message, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    _logger.Trace(message, args);
                    break;

                case LogLevel.Info:
                    _logger.Info(message, args);
                    break;

                case LogLevel.Warning:
                    _logger.Warn(message, args);
                    break;

                case LogLevel.Error:
                    _logger.Error(message, args);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public FileConflictResolution ResolveFileConflict(string message)
        {
            _logger.Warn(message);

            // This is what the default NullLogger returns.
            return FileConflictResolution.Ignore;
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            switch (level)
            {
                case MessageLevel.Info:
                    _logger.Info(message, args);
                    break;

                case MessageLevel.Warning:
                    _logger.Warn(message, args);
                    break;

                case MessageLevel.Debug:
                    _logger.Debug(message, args);
                    break;

                case MessageLevel.Error:
                    _logger.Error(message, args);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}