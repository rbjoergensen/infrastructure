using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Infrastructure
{
    public class Logging
    {
        public static LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();
        public static Logger log = new LoggerConfiguration().WriteTo.Console().MinimumLevel.ControlledBy(levelSwitch).CreateLogger();

        // Set the minimum log level that will be displayed
        // Eg. if log level set to information then debug and verbose will not be written to stdout
        // Defaults to logLevel information
        public static void SetLogLevel(string logLevel)
        {
            levelSwitch.MinimumLevel = logLevel switch
            {
                "Verbose" => LogEventLevel.Verbose,
                "Debug" => LogEventLevel.Debug,
                "Information" => LogEventLevel.Information,
                "Warning" => LogEventLevel.Warning,
                "Error" => LogEventLevel.Error,
                "Fatal" => LogEventLevel.Fatal,
                _ => LogEventLevel.Information,
            };
        }
        /*
        Logging.log.Information("{0}", new{jsonString});
        Logging.log.Information("Hardcore controller: {0}", new{jsonString});
        */
    }
}
