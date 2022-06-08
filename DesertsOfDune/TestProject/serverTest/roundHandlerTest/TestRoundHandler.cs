using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.gameObjects;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class RoundHandler
    /// </summary>
    public class TestRoundHandler
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method CheckOverLength
        /// </summary>
        [Test]
        public void TestCheckOverLength()
        {
            RoundHandler roundHandler = new RoundHandler(0,10);
            bool overLength = roundHandler.IsLastRoundOver();
            Assert.IsTrue(overLength);

            RoundHandler r2 = new RoundHandler(1,10);
            bool overL = r2.IsLastRoundOver();
            Assert.IsFalse(overL);
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method CheckVictory
        /// </summary>
        [Test]
        public void TestCheckVictory()
        {
            // implement logic
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method CheckSpiceThreshold
        /// </summary>
        [Test]
        public void TestCheckSpiceThreshold()
        {
            // implement logic
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method PauseGame
        /// </summary>
        [Test]
        public void TestPauseGame()
        {
            // implement logic
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method ContinueGame
        /// </summary>
        [Test]
        public void TestContinueGame()
        {
            // implement logic
        }
    }
}
