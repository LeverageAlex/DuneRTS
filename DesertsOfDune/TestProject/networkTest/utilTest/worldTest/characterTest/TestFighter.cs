using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.network.util.world.character;
using GameData.Configuration;
using GameData.network.util.world;

namespace UnitTestSuite.networkTest.utilTest.worldTest.characterTest
{
    /// <summary>
    /// This Class is used to tests the methods of the class Character
    /// </summary>
    public class TestFighter
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
        /// This Testcase validates the behavior of the method SwordSpin
        /// </summary>
        [Test]
        public void TestSwordSpin()
        {
            /*Fighter fighter = new Fighter("fighter");
            fighter.greatHouse = new Ordos();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            fighter.CurrentMapfield = map.fields[2, 2];
            map.fields[1, 2].Character = new BeneGesserit("");
            map.fields[1, 2].Character.greatHouse = new Harkonnen();
            map.PositionOfEyeOfStorm = new Position(4, 4);
            fighter.SwordSpin(map); */
        }
    }
}
