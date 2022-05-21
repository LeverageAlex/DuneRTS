using System;
using GameData.network.util.parser;
using GameData.parser;
using GameData.validation;
using Serilog;
using Server.Configuration;

namespace Server.parser.commandLineParser
{
    /// <summary>
    /// Parser for the command line arguments given, when the server is started.
    /// </summary>
    /// <remarks>
    /// Therefore this class inherits the <see cref="ACommandLineParser{T}"/> and use the <see cref="ServerOptions"/> as parameter,
    /// so the parser nows which options can / must be parsed. \n
    /// Addtionally this class provides a handler for the parsed arguments, which process and validates the arguments
    /// </remarks>
    public class CommandLineParser : ACommandLineParser<ServerOptions>
    {
        /// <summary>
        /// validator for the server configuration entries like the port or the file paths
        /// </summary>
        private readonly ServerConfigurationValidator validator;

        public ServerConfiguration configuration { get; }

        /// <summary>
        /// creates a command line parser for the server and initializes the parser
        /// </summary>
        public CommandLineParser() : base()
        {
            validator = new ServerConfigurationValidator();
            configuration = new ServerConfiguration();

            InitializeParser();
        }

        /// <summary>
        /// validate and process the given arguments as the server was started.
        /// </summary>
        /// <remarks>
        /// This means, for every argument (e.g. port) a validator checks, whether the argument is valid and print hints to
        /// user, if there any problems. If the argument is valid, the informationen is processed and the data set in the server configuration
        /// </remarks>
        /// <param name="options"></param>
        /// <returns>true, if the parsed arguments were valid and could be set in the configuration and false if there were any errors</returns>
        protected override bool HandleParsedArguments(ServerOptions options)
        {
            // handle all given argumentsx

            bool sucessfullySetMatchConfigFilePath = HandleMatchConfigurationFilePath(options.ConfigurationFileMatch);

            bool sucessfullySetScenarioConfigFilePath = HandleScenarioConfigurationFilePath(options.ConfigurationFileScenario);

            bool sucessfullySetPort = HandlePort(options.Port);

            return sucessfullySetMatchConfigFilePath && sucessfullySetScenarioConfigFilePath && sucessfullySetPort;
        }

        private bool HandleMatchConfigurationFilePath(string filepath)
        {

        }

        private bool HandleScenarioConfigurationFilePath(string filepath)
        {

        }

        /// <summary>
        /// handles the port, so if it is the default port set it in configuration file and if it a custom port, check this port and also set it to the configuration file
        /// if the the port is valid. 
        /// </summary>
        /// <param name="port">the port, which should be set to the configuration class</param>
        /// <returns>true, if the port was sucessfully set in the configuration class</returns>
        private bool HandlePort(int port)
        {
            if (port == ServerConfiguration.DEFAULT_PORT)
            {
                Log.Information("The default port " + ServerConfiguration.DEFAULT_PORT + " is used");
                configuration.port = ServerConfiguration.DEFAULT_PORT;
                return true;
            }
            else
            {
                // check, whether the port is valid
                if (validator.isPortValid(port))
                {
                    configuration.port = port;
                    return true;
                }
            }
            return false;
        }
    }
}
