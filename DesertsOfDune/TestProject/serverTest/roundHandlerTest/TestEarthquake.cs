using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.network.util.world;
using GameData.network.util.world.mapField;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class Earthquake
    /// </summary>
    public class TestEarthquake
    {
        
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method TransformRockPlanes
        /// </summary>
        [Test]
        public void TestTransformRockPlanes()
        {
          /*  MapField[,] fields = new MapField[4, 2];
            RockPlateau field = new RockPlateau(false, false, null);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    fields[i, j] = field;
                }
            }

            EarthQuakeExecutor e = new EarthQuake(fields);
            e.TransformRockPlanes();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Assert.AreEqual("DUNE", fields[i, j].TileType);
                }
            } */
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
