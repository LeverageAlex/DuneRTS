using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameData;
using GameData.network.util.enums;
using GameData.network.util.world;
using GameData.network.util.world.character;
using GameData.network.util.world.mapField;
using GameData.server.roundHandler;
using NUnit.Framework;
using Server.ClientManagement.Clients;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class ShaiHuludPhase
    /// </summary>
    public class TestShaiHuludPhase : Setup
    {
        /// <summary>
        /// the phase to be tested
        /// </summary>
        private ShaiHuludPhase _phase;

        [SetUp]
        public void Setup()
        {
            // do the basic setup for the a network module and the configuration classes
            base.NetworkAndConfigurationSetUp();

            // create a new shai hulud phase
            EarthQuakeExecutor earthQuakeExecutor = new EarthQuakeExecutor(Party.GetInstance().map);
            earthQuakeExecutor.TransformRockPlanes();
            _phase = new ShaiHuludPhase(Party.GetInstance().map);
        }

        /// <summary>
        /// test, whether the shai hulud phase is created correctly
        /// </summary>
        [Test]
        public void TestShaiHuludPhaseC()
        {
            Assert.NotNull(_phase.CurrentField);
            Assert.False(_phase.CurrentField.IsApproachable);
            Assert.False(_phase.CurrentField.IsCharacterStayingOnThisField);
            Assert.True(Party.GetInstance().map.IsFieldOnMap(_phase.CurrentField.XCoordinate, _phase.CurrentField.ZCoordinate));
            Assert.True(Party.GetInstance().map.IsMapFieldADesertField(_phase.CurrentField));
        }

        /// <summary>
        /// validates the behaviour of choosing a target character
        /// </summary>
        [Test]
        public void TestChooseTargetCharacter()
        {
            // fetch method, because it is private
            MethodInfo method = typeof(ShaiHuludPhase).GetMethod("ChooseTargetCharacter", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                // initially no character is on the map, so no one can be chosen
                Character gotCharacter = (Character)method.Invoke(_phase, Array.Empty<object>());

                Assert.IsNull(gotCharacter);

                Noble noble1 = new Noble("C1");
                Noble noble2 = new Noble("C2");
                Noble noble3 = new Noble("C3");

                Party.GetInstance().map.GetMapFieldAtPosition(1, 1).PlaceCharacter(noble1);
                Party.GetInstance().map.GetMapFieldAtPosition(1, 2).PlaceCharacter(noble2);
                Party.GetInstance().map.GetMapFieldAtPosition(1, 3).PlaceCharacter(noble3);

                Assert.AreEqual(3, Party.GetInstance().map.GetCharactersOnMap().Count);

                Character gotCharacter2 = (Character)method.Invoke(_phase, Array.Empty<object>());
                Assert.NotNull(gotCharacter2);
                Assert.True(gotCharacter2.Equals(noble1) || gotCharacter2.Equals(noble2) || gotCharacter2.Equals(noble3));

                Party.GetInstance().map.RemoveCharactersFromMap();
            }

        }

        /// <summary>
        /// validate the movement of the shai hulud
        /// </summary>
        [Test]
        public void TestMoveToTargetCharacter()
        {
            Mentat target = new Mentat("C1");
            MapField characerField = Party.GetInstance().map.GetRandomApproachableField();
            characerField.PlaceCharacter(target);

            // fetch method, because it is private
            MethodInfo method = typeof(ShaiHuludPhase).GetMethod("MoveToTargetCharacter", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                Position oldField = new Position(_phase.CurrentField.XCoordinate, _phase.CurrentField.ZCoordinate);
                Assert.False(_phase.CurrentField.IsApproachable);

                method.Invoke(_phase, new object[] { target });

                MapField newFieldAtOldPosition = Party.GetInstance().map.GetMapFieldAtPosition(oldField.x, oldField.y);

                Assert.True(newFieldAtOldPosition.tileType.Equals(TileType.FLAT_SAND.ToString()));
                Assert.True(newFieldAtOldPosition.IsApproachable);

                Assert.AreEqual(target.CurrentMapfield, _phase.CurrentField);
                Assert.False(_phase.CurrentField.IsApproachable);
            }

            Party.GetInstance().map.RemoveCharactersFromMap();
        }

        /// <summary>
        /// validate the eating of a character
        /// </summary>
        [Test]
        public void TestEatTargetCharacter()
        {
            Mentat target = new Mentat("C1");
            MapField characerField = Party.GetInstance().map.GetRandomApproachableField();
            characerField.PlaceCharacter(target);

            // fetch method, because it is private
            MethodInfo method = typeof(ShaiHuludPhase).GetMethod("EatTargetCharacter", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                bool eaten = (bool)method.Invoke(_phase, new object[] { target });

                Assert.False(eaten);

                ClientForTests testPlayer = new ClientForTests(new List<Character>() { target });
                Party.GetInstance().AddClient(testPlayer);

                bool eaten2 = (bool)method.Invoke(_phase, new object[] { target });

                Assert.True(eaten2);
                Assert.True(target.KilledBySandworm);
                Assert.False(_phase.CurrentField.IsCharacterStayingOnThisField);
            }

            Party.GetInstance().map.RemoveCharactersFromMap();
            Party.GetInstance().ResetClients();
        }

        /// <summary>
        /// validate the determination, whether there is only character left
        /// </summary>
        [Test]
        public void TestDetermineLastPlayerStanding()
        {
            Noble noble1 = new Noble("C1");

            ClientForTests testPlayer1 = new ClientForTests(new List<Character>());
            Party.GetInstance().AddClient(testPlayer1);

            ClientForTests testPlayer2 = new ClientForTests(new List<Character>() { noble1 });
            Party.GetInstance().AddClient(testPlayer2);

            // fetch method, because it is private
            MethodInfo method = typeof(ShaiHuludPhase).GetMethod("DetermineLastPlayerStanding", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                bool oneCharacterLeft1 = (bool)method.Invoke(_phase, Array.Empty<object>());

                Assert.False(oneCharacterLeft1);

                MapField characerField = Party.GetInstance().map.GetRandomApproachableField();
                characerField.PlaceCharacter(noble1);

                List<GameData.Clients.Player> clients = Party.GetInstance().GetActivePlayers();

                bool oneCharacterLeft2 = (bool)method.Invoke(_phase, Array.Empty<object>());

                Assert.True(oneCharacterLeft2);
                Assert.False(testPlayer1.statistics.LastCharacterStanding);
                Assert.True(testPlayer2.statistics.LastCharacterStanding);

                testPlayer2.UsedGreatHouse.Characters.Remove(noble1);
                testPlayer1.UsedGreatHouse.Characters.Add(noble1);

                bool oneCharacterLeft3 = (bool)method.Invoke(_phase, Array.Empty<object>());

                Assert.True(oneCharacterLeft3);
                Assert.True(testPlayer1.statistics.LastCharacterStanding);
                Assert.False(testPlayer2.statistics.LastCharacterStanding);
            }

            Party.GetInstance().map.RemoveCharactersFromMap();
            Party.GetInstance().ResetClients();
        }

        /// <summary>
        /// test the execution of one shai hulud round (repeated test)
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

                _phase = new ShaiHuludPhase(Party.GetInstance().map);
                Party party = Party.GetInstance();

                ClientForTests testPlayer1 = new ClientForTests(new List<Character>() { noble1, noble2 });
                Party.GetInstance().AddClient(testPlayer1);

                ClientForTests testPlayer2 = new ClientForTests(new List<Character>() { noble3 });
                Party.GetInstance().AddClient(testPlayer2);

                Position firstPosition = new Position(_phase.CurrentField.XCoordinate, _phase.CurrentField.ZCoordinate);

                Assert.False(_phase.Execute());
                Position secondPosition = new Position(_phase.CurrentField.XCoordinate, _phase.CurrentField.ZCoordinate);
                //Assert.True(Party.GetInstance().map.GetMapFieldAtPosition(firstField.XCoordinate, firstField.ZCoordinate).tileType.Equals(TileType.FLAT_SAND.ToString()));
                Assert.AreNotEqual(firstPosition, secondPosition);
                Assert.AreEqual(2, Party.GetInstance().map.GetCharactersOnMap().Count);

                Assert.False(_phase.Execute());
                Position thirdPosition = new Position(_phase.CurrentField.XCoordinate, _phase.CurrentField.ZCoordinate);
                //Assert.True(Party.GetInstance().map.GetMapFieldAtPosition(secondField.XCoordinate, secondField.ZCoordinate).tileType.Equals(TileType.FLAT_SAND.ToString()));
                Assert.AreNotEqual(secondPosition, thirdPosition);
                Assert.AreEqual(1, Party.GetInstance().map.GetCharactersOnMap().Count);

                Assert.False(_phase.Execute());
                Position fourthPosition = new Position(_phase.CurrentField.XCoordinate, _phase.CurrentField.ZCoordinate);
                //Assert.True(Party.GetInstance().map.GetMapFieldAtPosition(thirdField.XCoordinate, thirdField.ZCoordinate).tileType.Equals(TileType.FLAT_SAND.ToString()));
                Assert.AreNotEqual(thirdPosition, fourthPosition);
                Assert.AreEqual(0, Party.GetInstance().map.GetCharactersOnMap().Count);

                Assert.True(testPlayer1.statistics.LastCharacterStanding || testPlayer2.statistics.LastCharacterStanding);

                Assert.True(_phase.Execute());

                Party.GetInstance().map.RemoveCharactersFromMap();
                Party.GetInstance().ResetClients();
            }
        }
    }
}
