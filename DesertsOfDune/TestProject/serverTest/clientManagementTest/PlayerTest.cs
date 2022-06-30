using System;
using System.Collections.Generic;
using GameData;
using GameData.Clients;
using NUnit.Framework;

namespace UnitTestSuite.serverTest.clientManagementTest
{
    /// <summary>
    /// Test for creating a general player
    /// </summary>
    public class PlayerTest : Setup
    {
        public PlayerTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            base.ConfigurationSetUp();
        }

        [Test]
        public void TestCreatePlayer()
        {
            Player player1 = new HumanPlayer("Client1", "ID1");
            Player player2 = new AIPlayer("Client2", "ID2");

            Assert.AreNotEqual(player1.GetType(), player2.GetType());

            Assert.True(player1.IsActivePlayer);
            Assert.False(player1.IsAI);
            Assert.AreEqual(0, player1.AmountOfStrikes);
            Assert.NotNull(player1.statistics);

            Assert.True(player2.IsActivePlayer);
            Assert.True(player2.IsAI);
        }


        [Test]
        /// <summary>
        /// validates, that the client secret is generated correctly
        /// </summary>
        public void TestGenerateClientSecret()
        {
            Player player1 = new HumanPlayer("Client1", "ID1");

            List<string> clientSecrets = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                string secret = player1.GenerateClientSecret();

                Assert.False(clientSecrets.Contains(secret));

                clientSecrets.Add(secret);
            }
        }

        [Test]
        /// <summary>
        /// validates, that strikes can be added correctly
        /// </summary>
        public void TestAddStrike()
        {
            Player player1 = new HumanPlayer("Client1", "ID1");

            Assert.AreEqual(0, player1.AmountOfStrikes);
            player1.AddStrike();
            Assert.AreEqual(1, player1.AmountOfStrikes);
        }
    }
}

