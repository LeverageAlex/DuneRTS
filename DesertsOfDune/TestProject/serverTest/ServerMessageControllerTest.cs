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

namespace UnitTestSuite.serverTest
{

    class ServerMessageControllerTest
    {

        [SetUp]
        public void Setup()
        {
            ConfigurationFileLoader loader = new ConfigurationFileLoader();

            // load scenario and create a new scenario configuration
            ScenarioConfiguration scenarioConfiguration = loader.LoadScenarioConfiguration("../.././../ConfigurationFiles/team08.scenario.json");
            ScenarioConfiguration.CreateInstance(scenarioConfiguration);

            // load the party configuration and create a new party configuration class
            PartyConfiguration partyConfiguration = loader.LoadPartyConfiguration("../.././../ConfigurationFiles/team08.party.json");
            PartyConfiguration.SetInstance(partyConfiguration);

            //Initialization for greatHouses in GameData project
            GameData.Configuration.Configuration.InitializeConfigurations();
            // Initialization for the character configurations in GameData project
            GameData.Configuration.Configuration.InitializeCharacterConfiguration(
                PartyConfiguration.GetInstance().noble,
                PartyConfiguration.GetInstance().mentat,
                PartyConfiguration.GetInstance().beneGesserit,
                PartyConfiguration.GetInstance().fighter);
        }

        [Test]
        public void TestOnJoinMessage()
        {
            ServerMessageController serverMessageController = new ServerMessageController();
            serverMessageController.OnJoinMessage(new JoinMessage("client1", true, false), "session1");
            Assert.AreEqual(1, Party.GetInstance().GetActivePlayers().Count);
            serverMessageController.OnJoinMessage(new JoinMessage("client1", true, false), "session2");
            Assert.AreEqual(2, Party.GetInstance().GetActivePlayers().Count);
            //fails at DoAcceptJoin because the NetworkController is null
        }

        [Test]
        public void TestOnRejoinMessage()
        {

        }

        [Test]
        public void TestOnHouseRequestMessage()
        {

        }

        [Test]
        public void TestOnMovementRequestMessage()
        {
            ServerMessageController smc = new ServerMessageController();
            Player player1 = new HumanPlayer("client1", "session1");
            Player player2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(player1);
            Party.GetInstance().AddClient(player2);
            Party.GetInstance().PrepareGame();

            var path = new List<Position>();
            path.Add(new Position(2, 2));
            path.Add(new Position(2, 3));
            path.Add(new Position(2, 4));
            path.Add(new Position(2, 4));

            Player activePlayer = player1;

            Character movingCharacter = null;
            //Assert.AreNotEqual(null, movingCharacter);

            Assert.AreEqual(movingCharacter.MPmax - path.Count, movingCharacter.MPcurrent);
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
