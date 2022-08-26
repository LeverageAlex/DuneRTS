using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;
using GameData.network.util.enums;
using GameData.network.util.world;
using GameData.network.util.world.character;
using GameData.server.roundHandler;
using NUnit.Framework;
using Server.ClientManagement.Clients;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// Test the overlength mechanism
    /// </summary>
    public class TestOverLengthMechanism : Setup
    {

        /// <summary>
        /// the phase to be tested
        /// </summary>
        private OverLengthMechanism _phase;

        [SetUp]
        public void Setup()
        {
            // do the basic setup for the a network module and the configuration classes
            base.NetworkAndConfigurationSetUp();

            // create a new overlength mechanism
            _phase = new OverLengthMechanism(Party.GetInstance().map);

            Noble noble1 = new Noble("C1");
            Noble noble2 = new Noble("C2");
            Noble noble3 = new Noble("C3");

            Party.GetInstance().map.GetMapFieldAtPosition(1, 1).PlaceCharacter(noble1);
            Party.GetInstance().map.GetMapFieldAtPosition(1, 2).PlaceCharacter(noble2);
            Party.GetInstance().map.GetMapFieldAtPosition(1, 3).PlaceCharacter(noble3);

        }

        /// <summary>
        /// test, whether the overlength mechanism is created correctly
        /// </summary>
        [Test]
        public void TestOverLengthMechanismC()
        {
            // check, if map was changed by earth quake
            Assert.AreEqual(3, Party.GetInstance().map.GetCharactersOnMap().Count);
            for (int x = 0; x < Party.GetInstance().map.MAP_WIDTH; x++)
            {
                for (int y = 0; y < Party.GetInstance().map.MAP_HEIGHT; y++)
                {
                    string tileType = Party.GetInstance().map.GetMapFieldAtPosition(x, y).tileType;
                    Assert.True(tileType.Equals(TileType.CITY.ToString()) || tileType.Equals(TileType.FLAT_SAND.ToString()) || tileType.Equals(TileType.DUNE.ToString()));
                }
            }

            Assert.IsNull(Sandworm.GetSandworm());
        }

        /// <summary>
        /// test the execution of one round in the overlength mechanism (repeated test)
        /// </summary>
        [Test]
        public void TestExecute()
        {
            for (int i = 0; i < 100; i++)
            {
                Noble noble1 = new Noble("C1");
                Noble noble2 = new Noble("C2");
                Noble noble3 = new Noble("C3");

                Party.GetInstance().map.GetMapFieldAtPosition(1, 1).PlaceCharacter(noble1);
                Party.GetInstance().map.GetMapFieldAtPosition(1, 2).PlaceCharacter(noble2);
                Party.GetInstance().map.GetMapFieldAtPosition(1, 3).PlaceCharacter(noble3);

                _phase = new OverLengthMechanism(Party.GetInstance().map);

                ClientForTests testPlayer1 = new ClientForTests(new List<Character>() { noble1, noble2 });
                Party.GetInstance().AddClient(testPlayer1);

                ClientForTests testPlayer2 = new ClientForTests(new List<Character>() { noble3 });
                Party.GetInstance().AddClient(testPlayer2);

                Assert.False(_phase.Execute());
                Assert.False(_phase.Execute());
                Assert.False(_phase.Execute());

                Assert.True(_phase.Execute());

                Party.GetInstance().map.RemoveCharactersFromMap();
                Party.GetInstance().ResetClients();
            }

        }
    }
}
