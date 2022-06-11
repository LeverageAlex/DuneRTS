using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace GameData.network.util
{
    /// <summary>
    /// The Logger-class offers the possibility to create and
    /// configure the global logger.
    /// </summary>
    /// <remarks>The logger is from the Serilog-Package and is used for logging informationen
    /// to the console or to files. It can be used with Log.<level>(...); \n
    /// Because this class only should create and configure the (global acessable) logger,
    /// there are static methods for configuring the logger on different ways.
    /// </remarks>
    public class Logger
    {
        private readonly LoggingLevelSwitch levelSwitch;

        /// <summary>
        /// create a new logger instance, that can be used to create and configure the global used serilog logger
        /// </summary>
        public Logger()
        {
            levelSwitch = new LoggingLevelSwitch();
        }

        /// <summary>
        /// creates and configures logger with "default" settings, that means:
        /// <list type="bullet">
        /// <item> write the log information to the console</item>
        /// </list>  
        /// </summary>
        public void CreateDefaultLogger()
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.ControlledBy(levelSwitch)
                 .WriteTo.Console()
                 .CreateLogger();
        }

        public void CreateDebugLogger()
        {
            levelSwitch.MinimumLevel = LogEventLevel.Debug;
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.ControlledBy(levelSwitch)
                 .WriteTo.Console()
                 .CreateLogger();
        }

        /// <summary>
        /// change the minimum level for logging to a given level (at runtime)
        /// </summary>
        /// <param name="level">the new minimum level, the logger should be set to</param>
        public void SetMinimumLevel(LogEventLevel level)
        {
            levelSwitch.MinimumLevel = level;
        } 
    }
}
