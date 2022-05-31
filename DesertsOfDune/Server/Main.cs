using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Configuration;
using GameData.network.controller;
using GameData.network.util;
using GameData.network.util.world;
using GameData.server.roundHandler;
using Serilog;
using Server.Configuration;
using Server.parser.commandLineParser;
using Server.roundHandler.duneMovementHandler;

namespace Server
{
    /// <summary>
    /// The main class for configuring and starting the server. \n
    /// The server needs command line arguments for configuration and two configuration files, which are
    /// expected to be syntactically and semantically correct. For further information see <see cref="Main(string[])"/>
    /// </summary>
    static class Programm {

        private static ServerConfiguration configuration;
        private static ServerMessageController serverMessageController;

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
            Log.Debug("Created network module in server");

            LoadConfigurationFiles();
            Log.Debug("Loaded configuration files");

            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            map.DrawMapToConsole();

            DuneMovementPhase handler = new DuneMovementPhase(map);
            SandstormPhase handler2 = new SandstormPhase(map);

            for (int i = 0; i < 10; i++)
            {
                handler.Execute();
                handler2.Execute();
                map.DrawMapToConsole();
            }

            //Create new party here and set the ServerMessageController
            Party.GetInstance().messageController = serverMessageController;
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
            serverMessageController = new ServerMessageController();

            string serverAddress = ServerConfiguration.DEFAULT_SERVER_ADDRESS;
            int port = configuration.Port;

            ServerConnectionHandler serverConnectionHandler = new ServerConnectionHandler(serverAddress, port);
            _ = new ServerNetworkController(serverConnectionHandler, serverMessageController);
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

                // get the configuration data
                configuration = parser.Configuration;
            } else
            {
                Log.Fatal("The given command line arguments contains errors and cannot be processed. So restart the server with correct arguments.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// loads the configuration files and process the information in it
        /// </summary>
        private static void LoadConfigurationFiles()
        {
            ConfigurationFileLoader loader = new ConfigurationFileLoader();

            // load scenario and create a new scenario configuration
            ScenarioConfiguration scenarioConfiguration = loader.LoadScenarioConfiguration(configuration.FilePathScenarioConfiguration);
            ScenarioConfiguration.CreateInstance(scenarioConfiguration);

            // load the party configuration and create a new party configuration class
            PartyConfiguration partyConfiguration = loader.LoadPartyConfiguration(configuration.FilePathMatchConfiguration);
            PartyConfiguration.SetInstance(partyConfiguration);

            // set the character configurations
            CharacterConfiguration.Noble = PartyConfiguration.GetInstance().noble;
            CharacterConfiguration.Mentat = PartyConfiguration.GetInstance().mentat;
            CharacterConfiguration.BeneGesserit = PartyConfiguration.GetInstance().beneGesserit;
            CharacterConfiguration.Fighter = PartyConfiguration.GetInstance().fighter;

            //initialize the GreatHouseConfiguration
            GreatHouseConfiguration greatHouseConfiguration = new GreatHouseConfiguration();
        }
    }
}
