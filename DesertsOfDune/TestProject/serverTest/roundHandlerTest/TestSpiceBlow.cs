using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.gameObjects;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class SpiceBlow
    /// </summary>
    public class TestSpiceBlow
    {
        /// <summary>
        /// This Testcase validates the behaviour of the method RandomSpiceBlow
        /// </summary>
     /*   [Test]
        public void TestRandomSpiceBlow()
        {
            // implement logic
        } */

        /// <summary>
        /// This Testcase validates the behaviour of the method ApplySpiceBlow
        /// </summary>
        [Test]
        public void TestApplySpiceBlow()
        {
            RoundHandler roundHandler = new RoundHandler(2, 3);
            roundHandler.CurrentSpice = 2;
            SpiceBlow spiceBlow = new SpiceBlow(roundHandler);
            bool spiceBlowApplicable = spiceBlow.SpiceBlowIsApplicable();
            Assert.IsTrue(spiceBlowApplicable);
        }
    }
}
