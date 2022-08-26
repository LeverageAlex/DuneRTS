using System;
using GameData;
using NUnit.Framework;

namespace UnitTestSuite.serverTest.clientManagementTest
{
    /// <summary>
    /// Test for creating an spectator
    /// </summary>
    public class SpectatorTest : Setup
    {
        public SpectatorTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            base.ConfigurationSetUp();
        }

        [Test]
        public void TestCreateSpectator()
        {
            Spectator spectator1 = new Spectator("Client1", "ID1");
            Spectator spectator2 = new Spectator("Client2", "ID2");
            Spectator spectator3 = new Spectator("Client1", "ID1");

            Assert.IsInstanceOf(typeof(Spectator), spectator1);
            Assert.AreNotSame(spectator1, spectator2);
            Assert.AreNotSame(spectator1, spectator3);

            Assert.False(spectator1.IsActivePlayer);
            Assert.False(spectator1.IsAI);
            Assert.AreEqual("Client1", spectator1.ClientName);
            Assert.AreEqual("ID1", spectator1.SessionID);
            Assert.NotNull(spectator1.ClientSecret);
            Assert.AreNotEqual(0, spectator1.ClientID);
        }
    }
}

