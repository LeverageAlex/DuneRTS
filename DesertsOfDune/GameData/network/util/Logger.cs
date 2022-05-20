using System;
using Serilog;

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
        protected Logger()
        {
        }

        /// <summary>
        /// creates and configures logger with "default" settings, that means:
        /// <list type="bullet">
        /// <item> write the log information to the console</item>
        /// </list>  
        /// </summary>
        public static void CreateDefaultLogger()
        {
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .CreateLogger();
        }

        public static void CreateDebugLogger()
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.Console()
                 .CreateLogger();
        }
    }
}
