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
using GameData.network.util.enums;

namespace UnitTestSuite.serverTest.roundHandlerTest.endOfGameTest
{
    internal class TestEarthquake
    {

        private Map map;
        private EarthQuakeExecutor e;

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

            e = new EarthQuakeExecutor(map);
        }


        [Test]
        public void TestTransformRockPlanes()
        {
            e.TransformRockPlanes();
            
            for (int x = 0; x < map.MAP_WIDTH; x++)
            {
                for (int y = 0; y < map.MAP_HEIGHT; y++)
                {
                    MapField f = map.GetMapFieldAtPosition(x, y);
                    Assert.False(f.tileType.Equals(TileType.MOUNTAINS.ToString()) || f.tileType.Equals(TileType.PLATEAU.ToString()));
                }
            }
        }
    }
}