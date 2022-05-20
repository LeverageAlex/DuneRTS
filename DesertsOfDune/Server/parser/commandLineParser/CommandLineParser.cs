using System;
using GameData.network.util.parser;
using GameData.parser;
using Serilog;

namespace Server.parser.commandLineParser
{
    public class CommandLineParser : ACommandLineParser<ServerOptions>
    {
        /// <summary>
        /// creates a command line parser for the server and initializes the parser
        /// </summary>
        public CommandLineParser() : base()
        {
            InitializeParser();
        }

        protected override void HandleParsedArguments(ServerOptions options)
        {
            Log.Debug("The given match path was: " + options.ConfigurationFileMatch);
            Log.Debug("The given scenario path was: " + options.ConfigurationFileScenario);
            if (options.Port == 10191)
            {
                Log.Debug("No port given, the default is used: " + 10191);
            } else
            {
                Log.Debug("The given port is: " + options.Port);
            }
        }
    }
}
