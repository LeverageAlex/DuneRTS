using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.gameObjects;
using GameData.network.util.world;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class SpiceBlow
    /// </summary>
    public class TestSpiceBlow
    {
        /// <summary>
        /// This Testcase validates the behaviour of the method PlaceSpiceOnFields
        /// </summary>
        [Test]
        public void TestPlaceSpiceOnFields()
        {
           /* Map map = new Map(3, 3, null);
            RoundHandler roundHandler = new RoundHandler(2, 3, map);

            SpiceBlow spiceBlow = new SpiceBlow(map);
            MapField randomField = map.GetRandomDesertField();
            if (!randomField.HasSpice)
            {
                spiceBlow.PlaceSpiceOnFields(randomField);
            }
            Assert.True(randomField.HasSpice); */
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method IsSpiceBlowNecessary
        /// </summary>
        [Test]
        public void TestIsSpiceBlowNecessary()
        {

        }


        /// <summary>
        /// This Testcase validates the behaviour of the method Execute
        /// </summary>
        [Test]
        public void TestExecute()
        {

        }
    }
}
