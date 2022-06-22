using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.network.messages;
using GameData.network.util.world.character;
using GameData.Configuration;
using GameData.network.util.world;
using GameData;
using GameData.Clients;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class ClonePhase
    /// </summary>
    public class TestClonePhase
    {
        [SetUp]
        public void Setup()
        {
            ConfigurationFileLoader loader = new ConfigurationFileLoader();

            // load scenario and create a new scenario configuration
            ScenarioConfiguration scenarioConfiguration = loader.LoadScenarioConfiguration("../.././../ConfigurationFiles/team08.scenario.json");
            ScenarioConfiguration.CreateInstance(scenarioConfiguration);

            // load the party configuration and create a new party configuration class
            PartyConfiguration partyConfiguration = loader.LoadPartyConfiguration("../.././../ConfigurationFiles/team08.party.json");
            PartyConfiguration.SetInstance(partyConfiguration);

            //Initialization for greatHouses in GameData project
            GameData.Configuration.Configuration.InitializeConfigurations();
            // Initialization for the character configurations in GameData project
            GameData.Configuration.Configuration.InitializeCharacterConfiguration(
                PartyConfiguration.GetInstance().noble,
                PartyConfiguration.GetInstance().mentat,
                PartyConfiguration.GetInstance().beneGesserit,
                PartyConfiguration.GetInstance().fighter);
        }


        /// <summary>
        /// This Testcase validates the behaviour of the method Execute
        /// </summary>
        [Test]
        public void TestExecute()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            ClonePhase clonePhase = new ClonePhase(map, PartyConfiguration.GetInstance().cloneProbability);
            Party.GetInstance().AddClient(new HumanPlayer("client1", "1234"));
            Party.GetInstance().AddClient(new HumanPlayer("client2", "4321"));
            GreatHouse greatHousePlayer2 = Party.GetInstance().GetActivePlayers()[1].UsedGreatHouse;
            Party.GetInstance().GetActivePlayers()[1].UsedGreatHouse = new Atreides();
            Party.GetInstance().GetActivePlayers()[0].UsedGreatHouse = new Harkonnen();
            Party.GetInstance().GetActivePlayers()[1].UsedGreatHouse.Characters[0].healthCurrent = 0;

            clonePhase.Execute();
        }

    }
}
