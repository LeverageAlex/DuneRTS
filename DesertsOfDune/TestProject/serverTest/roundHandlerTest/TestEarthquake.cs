using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using GameData.Configuration;
using GameData.network.util.enums;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class Earthquake
    /// </summary>
    public class TestEarthquake : Setup
    {
        private Map map;

        [SetUp]
        public void Setup()
        {
            base.NetworkAndConfigurationSetUp();
            map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);

        }

        /// <summary>
        /// This Testcase validates the behaviour of the method TransformRockPlanes
        /// </summary>
        [Test]
        public void TestTransformRockPlanes()
        {
            int n = 0;
            foreach(MapField f in map.fields)
            {
                if(f.tileType.Equals(TileType.PLATEAU.ToString()) || f.tileType.Equals(TileType.MOUNTAINS.ToString())) n++;
            }
            Assume.That(n > 0);

            EarthQuakeExecutor e = new EarthQuakeExecutor(map);
            e.TransformRockPlanes();

            foreach (MapField f in map.fields)
            {
                Assert.False(f.tileType.Equals(TileType.PLATEAU.ToString()) || f.tileType.Equals(TileType.MOUNTAINS.ToString()));
            }
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method RemoveSandworm
        /// </summary>
        [Test]
        public void TestRemoveSandworm()
        {
            // implement logic
        }
    }
}
