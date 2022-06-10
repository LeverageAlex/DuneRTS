using System;
using AIClient.Configuration;
using GameData.network.util.parser;
using GameData.validation;
using Serilog;

namespace AIClient.parser.commandLineParser
{
    /// <summary>
    /// Parser for the command line arguments given, when the ai client is started.
    /// </summary>
    /// <remarks>
    /// Therefore this class inherits the <see cref="ACommandLineParser{T}"/> and use the <see cref="AIClientOptions"/> as parameter,
    /// so the parser nows which options can / must be parsed. \n
    /// Addtionally this class provides a handler for the parsed arguments, which process and validates the arguments
    /// </remarks>
    public class CommandLineParser : ACommandLineParser<AIClientOptions>
    {
        private readonly AIClientConfigurationValidator validator;

        /// <summary>
        /// the configuration for the ai client
        /// </summary>
        public AIClientConfiguration Configuration { get; }

        /// <summary>
        /// creates a command line parser for the ai client and initializes the parser
        /// </summary>
        public CommandLineParser() : base()
        {
            InitializeParser();
        }

        /// <summary>
        /// validate and process the given arguments as the ai client was started
        /// </summary>
        /// <remarks>
        /// This means, for every argument (e.g. port) a validator checks, whether the argument is valid and print hints to
        /// user, if there any problems. If the argument is valid, the informationen is processed and the data set in the ai client configuration
        /// </remarks>
        /// <param name="options">the list of possible arguments and further information about them</param>
        /// <returns>true, if the parsed arguments were valid and could be set in the configuration and false if there were any errors</returns>
        protected override bool HandleParsedArguments(AIClientOptions options)
        {
            // handle all given arguments
            bool successfullySetAddress = HandleServerAddress(options.Address);

            bool successfullySetPort = HandlePort(options.Port);

            bool successfullySetName = HandleName(options.Name);

            return successfullySetAddress && successfullySetPort && successfullySetName;
        }


        /// <summary>
        /// handles the server address, so if it is the default address (127.0.0.1) set it in configuration file and if it a custom server address, check this server address and also set it to the configuration file
        /// if the the server address is valid. 
        /// </summary>
        /// <param name="port">the server address, which should be set to the configuration class</param>
        /// <returns>true, if the server address was sucessfully set in the configuration class</returns>
        private bool HandleServerAddress(string address)
        {
            if (address.Equals(AIClientConfiguration.DEFAULT_SERVER_ADDRESS))
            {
                Log.Information("The default server address " + AIClientConfiguration.DEFAULT_SERVER_ADDRESS + " is used.");
                Configuration.Address = AIClientConfiguration.DEFAULT_SERVER_ADDRESS;
                return true;
            }
            else
            {
                // check, whether the server address is valid
                if (validator.IsServerAddressValid(address))
                {
                    Log.Debug($"The server address {address} is valid and will be used to connect to the server.");
                    Configuration.Address = address;
                    return true;
                }
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
            if (port == AIClientConfiguration.DEFAULT_PORT)
            {
                Log.Information("The default port " + AIClientConfiguration.DEFAULT_PORT + " is used.");
                Configuration.Port = AIClientConfiguration.DEFAULT_PORT;
                return true;
            }
            else
            {
                // check, whether the port is valid
                if (validator.IsPortValid(port))
                {
                    Log.Debug($"The port {port} is valid and will be used to connect to the server.");
                    Configuration.Port = port;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// handles the name, so if it is teh default name, set this in the configuration and otherwise check the name and set it, if the name is valide
        /// </summary>
        /// <param name="name">the name, which was given as paramter and should be set in the configuration</param>
        /// <returns>true, if the name was sucessfully set in the configuration class</returns>
        private bool HandleName(string name)
        {
            if (name.Equals(AIClientConfiguration.DEFAULT_NAME))
            {
                Log.Information("The default name " + AIClientConfiguration.DEFAULT_PORT + " is used.");
                Configuration.Name = AIClientConfiguration.DEFAULT_NAME;
                return true;
            } else
            {
                // check, whether the name is valid
                if (validator.IsNameValid(name))
                {
                    Log.Debug($"The name {name} is valid and will be used to connect to the server.");
                    Configuration.Name = name;
                    return true;
                }
            }
            return false;
        }
    }
}
