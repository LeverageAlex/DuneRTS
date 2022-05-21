using System;

using GameData.network.controller;
using GameData.network.util;
using Serilog;
using Server.parser.commandLineParser;

namespace Server
{
    /// <summary>
    /// The main class for configuring and starting the server. \n
    /// The server needs command line arguments for configuration and two configuration files, which are
    /// expected to be syntactically and semantically correct. For further information see <see cref="Main(string[])"/>
    /// </summary>
    static class Programm {

        /// <summary>
        /// main method, which is executed when the server was started / executed
        /// It will initialize all classes and trigger all starting events including
        /// <list type="bullet">
        /// <item>create and configure logger</item>
        /// <item>create and configure network module</item>
        /// <item>load configuration files and configure everything</item>
        /// </list>
        /// </summary>
        /// <param name="args">arguments for starting the server (see Commandline args in "Standardisierungskomitee")</param>
        static void Main(string[] args)
        {
            InitiliazeLogger();
            Log.Information("Starting server... Everything will be created and configured.");

            ParseCommandLineArguments(args);
            Log.Debug("Parsed the command line arguments and configuring the server");

            CreateNetworkModule();
            Log.Debug("Create network module in server");
        }

        /// <summary>
        /// creates and configures a new logger for the server
        /// </summary>
        private static void InitiliazeLogger()
        {
            Logger logger = new Logger();
            logger.CreateDebugLogger();
        }

        /// <summary>
        /// creates and configure the network module for the server
        /// </summary>
        private static void CreateNetworkModule()
        {
            MessageController messageController = new MessageController();

            ServerConnectionHandler serverConnectionHandler = new ServerConnectionHandler("127.0.0.1", 7890);
            _ = new ServerNetworkController(serverConnectionHandler, messageController);
        }

        /// <summary>
        /// parsing the command line arguments giving when started the server and configure the server
        /// </summary>
        /// <param name="args"></param>
        private static void ParseCommandLineArguments(String[] args)
        {
            CommandLineParser parser = new CommandLineParser();
            bool wasSuccessfullyParsed = parser.ParseCommandLineArguments(args);
            if (wasSuccessfullyParsed)
            {
                Log.Debug("The command line arguments of the server were parsed sucessfully");
            } else
            {
                Log.Fatal("The given command line arguments contains errors and cannot be processed. So restart the server with correct arguments.");
            }
        }
    }
}
