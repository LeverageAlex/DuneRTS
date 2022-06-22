using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.network.util.world;
using GameData.server.roundHandler;
using GameData.server;
using GameData.network.util.world.mapField;
using GameData.network.util.world.character;
using GameData.Configuration;
using GameData;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class SandstormPhase
    /// </summary>
    public class TestSandstormPhase
    {
        private Map map;

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

            
            map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);

        }

        /// <summary>
        /// tests SandstormPhase(Map map) which contains ChangeStatusOfMapFields()
        /// </summary>
        [Test]
        public void TestCreateSandstormPhase()
        {
            Assume.That(map.GetSandstormFieldsOnMap().Count == 0);
            
            //create SandstormPhase
            SandstormPhase sp = new SandstormPhase(map);

            //check for sandStormTiles
            List<MapField> fields = new List<MapField>(map.GetSandstormFieldsOnMap());//GetSandstormFieldsOnMap is correctly testet

            Assert.True(fields.Count > 0);

            Assert.True(fields.Contains(sp.EyeOfStorm));

            foreach(MapField f in map.GetNeighborFields(sp.EyeOfStorm))//GetNeighborFields is correctly tested
            {
                Assert.True(fields.Contains(f));
            }

        }

        /// <summary>
        /// This Testcase validates the behaviour of the method RandomWalk
        /// </summary>
        [Test]
        public void TestRandomWalk()
        {
            //create SandstormPhase
            SandstormPhase sp = new SandstormPhase(map);
            MapField eye = sp.EyeOfStorm;

            sp.Execute();

            Assert.False(eye.XCoordinate == sp.EyeOfStorm.XCoordinate && eye.ZCoordinate == sp.EyeOfStorm.ZCoordinate);
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method CalculateNewStrom
        /// </summary>
        [Test]
        public void TestCalculateNewStrom()
        {
            // implement logic
        }

        [Test]
        public void Execute()
        {
            



        }
    }
}
