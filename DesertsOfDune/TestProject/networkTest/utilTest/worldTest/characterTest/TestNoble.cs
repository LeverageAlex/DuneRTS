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
    /// This Class is used to tests the methods of the class Nobel
    /// </summary>
    public class TestNoble
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
        /// This Testcase validates the behavior of the method Kanly
        /// </summary>
        [Test]
        public void TestKanlyFalse()
        {
            Fighter n2 = new Fighter("");
            Noble noble = new Noble("");
            bool possible = noble.Kanly(n2);
            Assert.IsFalse(possible);  
        }

        /// <summary>
        /// This Testcase validates the behavior of the method Kanly
        /// </summary>
        [Test]
        public void TestKanly()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            Noble n2 = new Noble("");
            n2.CurrentMapfield = map.fields[0, 1];
            n2.greatHouse = new Harkonnen();
            Noble noble = new Noble("");
            noble.CurrentMapfield = map.fields[1, 1];
            n2.greatHouse = new Ordos();
            bool possible = noble.Kanly(n2);
            Assert.True(n2.healthMax == n2.healthCurrent && noble.APcurrent == 0 || 0 == n2.healthCurrent && noble.APcurrent == 0);
        }

        /// This Testcase validates the behavior of the method AtomicBomb
        /// </summary>
        [Test]
        public void TestAtomicBomb()
        {
            // TODO: implement logic
        }
    }
}
