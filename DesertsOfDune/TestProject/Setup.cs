using System;
using GameData;
using GameData.Configuration;
using GameData.network.controller;
using NUnit.Framework;

namespace UnitTestSuite
{
    /// <summary>
    /// Base class for the Unit Tests containing the setup
    /// </summary>
    /// <remarks>
    /// This is necessary, because most of the tests need the same setup consisting of
    /// <list type="bullet">
    /// <item>creating all configurations</item>
    /// <item>creating a dummy network controller and message controller</item>
    /// </list>
    /// With inheriting this class, all unit tests can easily use the setup method and do their basic setup and add further if needed
    /// </remarks>
    public class Setup
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public Setup()
        {
        }

        /// <summary>
        /// set up the configuration classes and a basic network module as well as a party containing all information
        /// </summary>
        [SetUp]
        public void NetworkAndConfigurationSetUp()
        {
            ConfigurationFileLoader loader = new ConfigurationFileLoader();

            // load scenario and create a new scenario configuration
            ScenarioConfiguration scenarioConfiguration = loader.LoadScenarioConfiguration("../../../ConfigurationFiles/team08.scenario.json");
            ScenarioConfiguration.CreateInstance(scenarioConfiguration);

            // load the party configuration and create a new party configuration class
            PartyConfiguration partyConfiguration = loader.LoadPartyConfiguration("../../../ConfigurationFiles/team08.party.json");
            PartyConfiguration.SetInstance(partyConfiguration);

            // initialization for greatHouses in GameData project
            Configuration.InitializeConfigurations();
            // initialization for the character configurations in GameData project
            Configuration.InitializeCharacterConfiguration(
                PartyConfiguration.GetInstance().noble,
                PartyConfiguration.GetInstance().mentat,
                PartyConfiguration.GetInstance().beneGesserit,
                PartyConfiguration.GetInstance().fighter);

            // create a new party and set the message controller
            if (Party.GetInstance().messageController == null)
            {
                // TODO: eventually change to message controller
                ServerMessageController messageController = new ServerMessageController();

                ServerConnectionHandler serverConnectionHandler = new ServerConnectionHandler("127.0.0.1", 8000);
                _ = new ServerNetworkController(serverConnectionHandler, messageController);

                Party.GetInstance().messageController = messageController;
            }

        }
    }
}

