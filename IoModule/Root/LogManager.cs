using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File;

namespace IoModule.Root
{
    public class LogManager : IDisposable
    {
        private ILogger _logger;
        /// <summary>
        /// This class is used to manage the log file.
        /// logPath: The path of the log file.
        /// rollInterval: The frequency at which the log file should roll. 0 : Infinite, 1: Year, 2: Month, 3: Day, 4: Hour, 5: Minute
        /// rollOnFileSize: If true, the log file will roll based on the file size limit. If false, the log file will roll based on the time limit.
        /// limitFileBytes: The file size limit in bytes.
        /// retained: The number of days to retain the log files.
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="rollInterval"></param>
        /// <param name="rollOnFileSize"></param>
        /// <param name="limitFileBytes"></param>
        /// <param name="retained"></param>
 
        public LogManager(string logPath = "Log\\TraceLog.txt", int rollInterval = 3, bool rollOnFileSize = true, long limitFileBytes = 10000000, double retained = 180)
        {
            SettingLogger(logPath, (RollingInterval)rollInterval, rollOnFileSize, limitFileBytes, retained);
        }
        ~LogManager()
        {
            Dispose();
        }
        private void SettingLogger(string path, RollingInterval rollingInterval, bool rollOnFileSize, long limitFileBytes, double retained)
        {
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.File(path, // Add this line on 5-31
                rollingInterval: rollingInterval,
                outputTemplate: "{Message}{NewLine}",
                rollOnFileSizeLimit: rollOnFileSize,
                fileSizeLimitBytes: limitFileBytes,
                retainedFileTimeLimit: TimeSpan.FromDays(retained)).
                CreateLogger();
        }
        public void Dispose()
        {
            Log.CloseAndFlush();
        }
        public void Trace(LogEventLevel logEventLevel, string message)
        {
            try
            {
                message = string.Format($"[{DateTime.Now.ToString("yyyy/MM/dd/HH mm:ss.fff")}] - ") + message;
                switch (logEventLevel)
                {
                    case LogEventLevel.Debug:
                        _logger.Debug(message);
                        break;
                    case LogEventLevel.Error:
                        _logger.Error(message);
                        break;
                    case LogEventLevel.Fatal:
                        _logger.Fatal(message);
                        break;
                    case LogEventLevel.Information:
                        _logger.Information(message);
                        break;
                    case LogEventLevel.Verbose:
                        _logger.Verbose(message);
                        break;
                    case LogEventLevel.Warning:
                        _logger.Warning(message);
                        break;
                }
            }
            catch (Exception)
            {
                _logger.Error("Trace Error");
            }
        }
    }
}
