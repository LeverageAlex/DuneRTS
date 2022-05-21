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

        /// <summary>
        /// handles the filepath to the match configuration file, so checks whether is filepath is valid and set the filepath in the server configuration if so
        /// </summary>
        /// <param name="filepath">path to the match configuration file</param>
        /// <returns>true, if the filepath is valid and was set to the server configuration</returns>
        private bool HandleMatchConfigurationFilePath(string filepath)
        {
            if (validator.IsMatchConfigFilePathValid(filepath))
            {
                configuration.FilePathMatchConfiguration = filepath;

                Log.Debug("The filepath " + filepath + " is valid and the match config will tried to be load.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// handles the filepath to the scenario configuration file, so checks whether is filepath is valid and set the filepath in the server configuration if so
        /// </summary>
        /// <param name="filepath">path to the scenario configuration file</param>
        /// <returns>true, if the filepath is valid and was set to the server configuration</returns>
        private bool HandleScenarioConfigurationFilePath(string filepath)
        {
            if (validator.IsScenarioConfigFilePathValid(filepath))
            {
                configuration.FilePathScenarioConfiguration = filepath;

                Log.Debug("The filepath " + filepath + " is valid and the scenario config will tried to be load.");
                return true;
            }
            return false;
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
                configuration.Port = ServerConfiguration.DEFAULT_PORT;
                return true;
            }
            else
            {
                // check, whether the port is valid
                if (validator.IsPortValid(port))
                {
                    Log.Debug("The port " + port + " is valid and will be used to start the websocket server");
                    configuration.Port = port;
                    return true;
                }
            }
            return false;
        }
    }
}
