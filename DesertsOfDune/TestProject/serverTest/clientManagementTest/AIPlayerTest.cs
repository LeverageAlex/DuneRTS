using System;
using GameData;
using NUnit.Framework;

namespace UnitTestSuite.serverTest.clientManagementTest
{
    /// <summary>
    /// Test for creating an ai player
    /// </summary>
    public class AIPlayerTest : Setup
    {
        public AIPlayerTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            base.ConfigurationSetUp();
        }

        [Test]
        public void TestCreateAIPlayer()
        {
            AIPlayer player1 = new AIPlayer("Client1", "ID1");
            AIPlayer player2 = new AIPlayer("Client2", "ID2");
            AIPlayer player3 = new AIPlayer("Client1", "ID1");

            Assert.IsInstanceOf(typeof(AIPlayer), player1);
            Assert.AreNotSame(player1, player2);
            Assert.AreNotSame(player1, player3);

            Assert.True(player1.IsActivePlayer);
            Assert.True(player1.IsAI);
            Assert.AreEqual("Client1", player1.ClientName);
            Assert.AreEqual("ID1", player1.SessionID);
            Assert.NotNull(player1.ClientSecret);
            Assert.AreNotEqual(0, player1.ClientID);
        }
    }
}

