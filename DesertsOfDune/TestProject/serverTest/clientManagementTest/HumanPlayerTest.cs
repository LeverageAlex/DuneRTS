using System;
using System.Collections.Generic;
using GameData;
using GameData.ClientManagement.Clients;
using NUnit.Framework;

namespace UnitTestSuite.serverTest.clientManagementTest
{
    /// <summary>
    /// Test for creating a human player
    /// </summary>
    public class HumanPlayerTest : Setup
    {
        public HumanPlayerTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            base.ConfigurationSetUp();
        }

        [Test]
        public void TestCreateHumanPlayer()
        {
            HumanPlayer player1 = new HumanPlayer("Client1", "ID1");
            HumanPlayer player2 = new HumanPlayer("Client2", "ID2");

            Assert.IsInstanceOf(typeof(HumanPlayer), player1);

            Assert.True(player1.IsActivePlayer);
            Assert.False(player1.IsAI);
            Assert.AreEqual("Client1", player1.ClientName);
            Assert.AreEqual("ID1", player1.SessionID);
            Assert.NotNull(player1.ClientSecret);
            Assert.AreNotEqual(0, player1.ClientID);
        }
    }
}

