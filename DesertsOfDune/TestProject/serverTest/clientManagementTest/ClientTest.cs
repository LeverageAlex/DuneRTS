using System;
using System.Collections.Generic;
using GameData;
using GameData.ClientManagement.Clients;
using NUnit.Framework;

namespace UnitTestSuite.serverTest.clientManagementTest
{
    /// <summary>
    /// Test for creating a client and tests its methods (generating ids and secrets)
    /// </summary>
    public class ClientTest : Setup
    {
        public ClientTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            base.ConfigurationSetUp();
        }


        [Test]
        public void TestCreateClient()
        {
            Client client1 = new HumanPlayer("Client1", "ID1");
            Client client2 = new AIPlayer("Client2", "ID2");
            Client client3 = new Spectator("Client3", "ID3");

            Assert.AreNotEqual(client1.GetType(), client2.GetType());
            Assert.AreNotEqual(client1.GetType(), client3.GetType());
            Assert.AreNotEqual(client2.GetType(), client3.GetType());
            Assert.NotNull(client1.ClientSecret);
            Assert.AreNotEqual(0, client1.ClientID);
        }

        [Test]
        /// <summary>
        /// tests, whether the client ids and secrets are unique after creating the clients
        /// </summary>
        public void TestGenerateClientIDAndSecret()
        {
            List<int> clientIDs = new List<int>();
            List<string> secrets = new List<string>();

            for (int i = 0; i < 10000; i++)
            {
                HumanPlayer client = new HumanPlayer("Client", "");

                Assert.False(clientIDs.Contains(client.ClientID));
                Assert.False(secrets.Contains(client.ClientSecret));

                clientIDs.Add(client.ClientID);
                secrets.Add(client.ClientSecret);
            }
        }
    }
}

