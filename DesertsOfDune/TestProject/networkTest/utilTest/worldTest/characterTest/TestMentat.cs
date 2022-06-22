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
    /// This Class is used to tests the methods of the class Mentat
    /// </summary>
    public class TestMentat
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
        /// This Testcase validates the behavior of the method SpiceHoarding
        /// </summary>
        [Test]
        public void TestSpiceHoarding()
        {
            Mentat mentat = new Mentat("");
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            mentat.CurrentMapfield = map.fields[0, 1];
            map.fields[0, 2].hasSpice = true;
            Assert.True(map.fields[0, 2].hasSpice);
            Assert.AreEqual(0, mentat.inventoryUsed);
            bool actionPossible = mentat.SpiceHoarding(map);
            Assert.AreEqual(1,mentat.inventoryUsed);
            Assert.False(map.fields[0, 2].hasSpice);
            Assert.True(actionPossible);
        }
    }
}
