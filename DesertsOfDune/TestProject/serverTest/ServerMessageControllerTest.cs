using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.world;
using NUnit.Framework;
using GameData;
using GameData.Clients;
using GameData.Configuration;
using System.Diagnostics;
using GameData.network.controller;
using Server.ClientManagement.Clients;
using GameData.network.util.world.character;
using GameData.network.util.world.greatHouse;

namespace UnitTestSuite.serverTest
{
    /// <summary>
    /// This testcase validates the behviour of the ServerMessageControllerTest
    /// </summary>
    class ServerMessageControllerTest : Setup
    {

        [SetUp]
        public void Setup()
        {
            base.NetworkAndConfigurationSetUp();
        }

        /// <summary>
        /// This testcase validates the behaviour of the method OnJoinMessage
        /// </summary>
        [Test]
        public void TestOnJoinMessage()
        {
            Party.GetInstance().messageController.OnJoinMessage(new JoinMessage("client1", true, false), "session1");
            Assert.AreEqual(1, Party.GetInstance().GetActivePlayers().Count);
            Party.GetInstance().messageController.OnJoinMessage(new JoinMessage("client1", true, false), "session2");
            Assert.AreEqual(2, Party.GetInstance().GetActivePlayers().Count);
        }

        [Test]
        public void TestOnRejoinMessage()
        {

        }

        /// <summary>
        /// This test validates the behaviour of the method onHouseRequestMessage.
        /// </summary>
        [Test]
        public void TestOnHouseRequestMessage()
        {
            ClientForTests testPlayer1 = new ClientForTests(null);
            ClientForTests testPlayer2 = new ClientForTests(null);
            testPlayer1.SessionID = "session1";
            testPlayer2.SessionID = "session2";
            Party.GetInstance().AddClient(testPlayer1);
            Party.GetInstance().AddClient(testPlayer2);
            Ordos ordos = new Ordos();
            Harkonnen harkonnen = new Harkonnen();
            GreatHouseType[] greatHouseTypes = new GreatHouseType[2];
            greatHouseTypes[0] = GreatHouseType.ORDOS;
            greatHouseTypes[1] = GreatHouseType.HARKONNEN;
            Party.GetInstance().GetPlayerBySessionID("session1").OfferedGreatHouses = greatHouseTypes;
            Party.GetInstance().messageController.OnHouseRequestMessage(new HouseRequestMessage("ORDOS"), "session1");
            Assert.AreEqual("ORDOS", Party.GetInstance().GetPlayerBySessionID("session1").UsedGreatHouse.houseName);
        }

        /// <summary>
        /// This test validates the behaviour of the method onHouseRequestMessage.
        /// </summary>
        [Test]
        public void TestOnHouseRequestMessageWrongGreatHouse()
        {
            ClientForTests testPlayer1 = new ClientForTests(null);
            ClientForTests testPlayer2 = new ClientForTests(null);
            testPlayer1.SessionID = "session1";
            testPlayer2.SessionID = "session2";
            Party.GetInstance().AddClient(testPlayer1);
            Party.GetInstance().AddClient(testPlayer2);
            Ordos ordos = new Ordos();
            Harkonnen harkonnen = new Harkonnen();
            GreatHouseType[] greatHouseTypes = new GreatHouseType[2];
            greatHouseTypes[0] = GreatHouseType.ORDOS;
            greatHouseTypes[1] = GreatHouseType.HARKONNEN;
            Party.GetInstance().GetPlayerBySessionID("session1").OfferedGreatHouses = greatHouseTypes;
            Party.GetInstance().messageController.OnHouseRequestMessage(new HouseRequestMessage("VERNIUS"), "session1");
            Assert.IsNull(Party.GetInstance().GetPlayerBySessionID("session1").UsedGreatHouse.houseName);
            Assert.AreEqual(1, Party.GetInstance().GetPlayerBySessionID("session1").AmountOfStrikes);
        }

        /// This test validates the behaviour of the method onHouseRequestMessage.
        /// </summary>
        [Test]
        public void TestOnHouseRequestMessageWithWrongSessionID()
        {
            ClientForTests testPlayer1 = new ClientForTests(null);
            ClientForTests testPlayer2 = new ClientForTests(null);
            testPlayer1.SessionID = "session1";
            testPlayer2.SessionID = "session2";
            Party.GetInstance().AddClient(testPlayer1);
            Party.GetInstance().AddClient(testPlayer2);
            Ordos ordos = new Ordos();
            Harkonnen harkonnen = new Harkonnen();
            GreatHouseType[] greatHouseTypes = new GreatHouseType[2];
            greatHouseTypes[0] = GreatHouseType.ORDOS;
            greatHouseTypes[1] = GreatHouseType.HARKONNEN;
            Party.GetInstance().GetPlayerBySessionID("session1").OfferedGreatHouses = greatHouseTypes;
            Party.GetInstance().messageController.OnHouseRequestMessage(new HouseRequestMessage("ORDOS"), "wrongSessionID");
            Assert.IsNull(Party.GetInstance().GetPlayerBySessionID("session1").UsedGreatHouse.houseName);
        }

        /// <summary>
        /// This testcase validates the behaviour of the method OnMovementRequestMessage.
        /// </summary>
        [Test]
        public void TestOnMovementRequestMessage()
        {
            
            Player activePlayer = new HumanPlayer("client1", "session1");
            Player player2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(activePlayer);
            Party.GetInstance().AddClient(player2);
            Party.GetInstance().PrepareGame();

            var path = new List<Position>();
            path.Add(new Position(2, 0));

            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            activePlayer.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield = map.fields[0, 1];

            Assert.AreEqual(activePlayer.UsedGreatHouse.Characters[0].MPmax, activePlayer.UsedGreatHouse.Characters[0].MPcurrent);
            Assert.AreEqual(map.fields[0, 1], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
            Party.GetInstance().messageController.OnMovementRequestMessage(new MovementRequestMessage(activePlayer.ClientID, activePlayer.UsedGreatHouse.Characters[0].CharacterId, new Specs(new Position(1, 2), path)));
            Assert.AreEqual(activePlayer.UsedGreatHouse.Characters[0].MPmax-1, activePlayer.UsedGreatHouse.Characters[0].MPcurrent);
            Assert.AreEqual(map.fields[0, 2], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
        }

        /// <summary>
        /// This testcase validates the behaviour of the method OnMovementRequestMessage.
        /// </summary>
        [Test]
        public void TestOnMovementRequestMessageInValidPath()
        {
            Player activePlayer = new HumanPlayer("client1", "session1");
            Player player2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(activePlayer);
            Party.GetInstance().AddClient(player2);
            Party.GetInstance().PrepareGame();

            var path = new List<Position>();
            path.Add(new Position(4, 0));

            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            activePlayer.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield = map.fields[0, 1];

            Assert.AreEqual(activePlayer.UsedGreatHouse.Characters[0].MPmax, activePlayer.UsedGreatHouse.Characters[0].MPcurrent);
            Assert.AreEqual(map.fields[0, 1], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
            Party.GetInstance().messageController.OnMovementRequestMessage(new MovementRequestMessage(activePlayer.ClientID, activePlayer.UsedGreatHouse.Characters[0].CharacterId, new Specs(new Position(1, 2), path)));
            Assert.AreEqual(activePlayer.UsedGreatHouse.Characters[0].MPmax, activePlayer.UsedGreatHouse.Characters[0].MPcurrent);
            Assert.AreEqual(map.fields[0, 1], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
        }

        /// <summary>
        /// This testcase validates the behaviour of the method OnMovementRequestMessage.
        /// </summary>
        [Test]
        public void TestOnMovementRequestMessageWithoutMP()
        {
            Player activePlayer = new HumanPlayer("client1", "session1");
            Player player2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(activePlayer);
            Party.GetInstance().AddClient(player2);
            Party.GetInstance().PrepareGame();

            var path = new List<Position>();
            path.Add(new Position(2, 0));

            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            activePlayer.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield = map.fields[0, 1];
            activePlayer.UsedGreatHouse.Characters[0].MPcurrent = 0;

            Assert.AreEqual(map.fields[0, 1], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
            Party.GetInstance().messageController.OnMovementRequestMessage(new MovementRequestMessage(activePlayer.ClientID, activePlayer.UsedGreatHouse.Characters[0].CharacterId, new Specs(new Position(1, 2), path)));
            Assert.AreEqual(map.fields[0, 1], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
        }

        /// <summary>
        /// This testcase validates the behaviour of the method OnMovementRequestMessage.
        /// </summary>
        [Test]
        public void TestOnMovementRequestMessageWithOldPositionAsTarget()
        {
            Player activePlayer = new HumanPlayer("client1", "session1");
            Player player2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(activePlayer);
            Party.GetInstance().AddClient(player2);
            Party.GetInstance().PrepareGame();

            var path = new List<Position>();
            path.Add(new Position(1, 0));

            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            activePlayer.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield = map.fields[0, 1];

            Assert.AreEqual(map.fields[0, 1], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
            Party.GetInstance().messageController.OnMovementRequestMessage(new MovementRequestMessage(activePlayer.ClientID, activePlayer.UsedGreatHouse.Characters[0].CharacterId, new Specs(new Position(1, 2), path)));
            Assert.AreEqual(map.fields[0, 1], activePlayer.UsedGreatHouse.Characters[0].CurrentMapfield);
            Assert.AreEqual(activePlayer.UsedGreatHouse.Characters[0].MPmax, activePlayer.UsedGreatHouse.Characters[0].MPcurrent);
        }

        [Test]
        public void TestOnActionRequestMessage()
        {
            /*string action = "ATTACK";
            switch (Enum.Parse(typeof(ActionType), action))
            {
                case ActionType.ATTACK:
                    actionCharacter.Atack(targetCharacter);
                    Console.WriteLine("Test");
                    break;

                case ActionType.COLLECT:
                    actionCharacter.CollectSpice();
                    break;
            }*/
        }

        [Test]
        public void TestOnTransferRequestMessage()
        {

        }

        [Test]
        public void TestOnEndTurnRequestMessage()
        {

        }

        [Test]
        public void TestOnGameStateRequestMessage()
        {

        }

        [Test]
        public void TestOnPauseGameRequestMessage()
        {

        }

        [Test]
        public void TestDoAcceptJoin()
        {

        }

        [Test]
        public void TestDoSendError()
        {

        }

        [Test]
        public void TestDoSendGameConfig()
        {

        }

        [Test]
        public void TestDoSendHouseOffer()
        {

        }

        [Test]
        public void TestDoSendHouseAck()
        {

        }

        [Test]
        public void TestDoSendTurnDemand()
        {

        }

        [Test]
        public void TestDoSendMovementDemand()
        {

        }

        [Test]
        public void TestDoSendActionDemand()
        {

        }

        [Test]
        public void TestDoSendTransferDemand()
        {

        }

        [Test]
        public void TestDoSendChangeCharacterStatsDemand()
        {

        }

        [Test]
        public void TestDoSendMapChangeDemand()
        {

        }

        [Test]
        public void TestDoSendAtomicsUpdateDemand()
        {

        }

        [Test]
        public void TestDoSpawnCharacterDemand()
        {

        }

        [Test]
        public void TestDoChangePlayerSpiceDemand()
        {

        }

        [Test]
        public void TestDoSpawnSandwormDemand()
        {

        }

        [Test]
        public void TestDoMoveSandwormDemand()
        {

        }

        [Test]
        public void TestDoDespawnSandwormDemand()
        {

        }

        [Test]
        public void TestDoEndGame()
        {

        }

        [Test]
        public void TestDoGameEndMessage()
        {

        }

        [Test]
        public void TestDoSendGameState()
        {

        }

        [Test]
        public void TestDoSendStrike()
        {
            
        }

        [Test]
        public void TestDoGamePauseDemand()
        {
            
        }

        [Test]
        public void TestOnUnpauseGameOffer()
        {
            
        }
    }
}
