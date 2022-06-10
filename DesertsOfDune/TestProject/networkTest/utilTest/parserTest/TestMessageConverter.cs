using NUnit.Framework;
using GameData.network.util.parser;
using GameData.network.messages;
using GameData.network.util.world;
using System.Collections.Generic;
using Server.Configuration;
using GameData.Configuration;
using GameData.network.util.world.character;

namespace TestProject.networkTest.utilTest.parserTest
{
    /// <summary>
    /// This Class is used to Test the MessageConverter
    /// </summary>
    public class TestMessageConverter
    {
        [SetUp]
        public void Setup()
        {
        }

        // The following tests validate the FromMessage Method

        /// <summary>
        /// This Testcase validates the serialization of the Message ActionDemandMessage
        /// </summary>
        [Test]
        public void TestFromActionDemandMessage()
        {
            ActionDemandMessage message = new ActionDemandMessage(1234, 12, ActionType.ATTACK, new Position(2, 3));
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"ACTION_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"action\":\"ATTACK\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message ActionRequestMessage
        /// </summary>
        [Test]
        public void TestFromActionRequestMessage()
        {
            ActionRequestMessage message = new ActionRequestMessage(1234, 12, ActionType.VOICE, new Position(2, 3));
            string serializedMessage = MessageConverter.FromMessage(message);
            // {\"type\":\"ACTION_REQUEST\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12,\"action\":\"VOICE\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}
            Assert.AreEqual("{\"type\":\"ACTION_REQUEST\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"action\":\"VOICE\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message ChangePlayerSpiceDemandMessage
        /// </summary>
        [Test]
        public void TestFromChangePlayerSpiceDemandMessage()
        {
            ChangePlayerSpiceDemandMessage message = new ChangePlayerSpiceDemandMessage(123123, 5);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"CHANGE_PLAYER_SPICE_DEMAND\",\"version\":\"1.0\",\"clientID\":123123,\"newSpiceValue\":5}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message ChangeCharacterStatisticsDemandMessage
        /// </summary>
        [Test]
        public void TestFromCharacterStatChangeDemandMessage()
        {
            CharacterStatistics characterStatistics = new CharacterStatistics(10, 4, 3, 8, false, false);
            ChangeCharacterStatisticsDemandMessage message = new ChangeCharacterStatisticsDemandMessage(1234, 12, characterStatistics);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"CHARACTER_STAT_CHANGE_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"stats\":{\"HP\":10,\"AP\":4,\"MP\":3,\"spice\":8,\"isLoud\":false,\"isSwallowed\":false}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message DebugMessage
        /// </summary>
        [Test]
        public void TestFromDebugMessage()
        {
            DebugMessage message = new DebugMessage(1, "explenation");
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"DEBUG\",\"version\":\"1.0\",\"code\":1,\"explanation\":\"explenation\"}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message EndGameMessage
        /// </summary>
        [Test]
        public void TestFromEndGameMessage()
        {
            EndGameMessage message = new EndGameMessage();
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"ENDGAME\",\"version\":\"1.0\"}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message EndTurnRequestMessage
        /// </summary>
        [Test]
        public void TestFromEndTurnRequestMessage()
        {
            EndTurnRequestMessage message = new EndTurnRequestMessage(1234, 12);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"END_TURN_REQUEST\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message GameConfigMessage
        /// </summary>
        [Test]
        public void TestFromGameConfigMessage()
        {
            string[] arr1 = { "String", "String" };
            string[] arr2 = { "String", "String" };
            List<List<string>> scenario = new List<List<string>>();
            scenario.Add(new List<string>(arr1));
            scenario.Add(new List<string>(arr2));
            PartyReference party = new PartyReference("#/definitions/partiekonfigschema");
            GameConfigMessage message = new GameConfigMessage(scenario, party, 0, 0);
            string serializedMessage = MessageConverter.FromMessage(message);

                                                            // \"scenario\":[[\"<String>\",\"<String>\"],[\"<String>\",\"<String>\"]],\"party\":{\"$ref\":\"#/definitions/partiekonfigschema\"},\"cityToClient\":[{\"clientID\":\"<int>\",\"x\":\"<int>\",\"y\":\"<int>\"},{\"clientID\":\"<int>\",\"x\":\"<int>\",\"y\":\"<int>\"}],\"stormEye\":{\"x\":\"<int>\",\"y\":\"<int>\"}

            Assert.AreEqual("{\"type\":\"GAMECFG\",\"version\":\"1.0\",\"scenario\":[[\"String\",\"String\"],[\"String\",\"String\"]],\"party\":\"party\",\"client0ID\":0,\"client1ID\":0}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message GameEndMessage with empty statistics
        /// </summary>
        [Test]
        public void TestFromGameEndMessageEmptyStats()
        {
            EmptyStatistics statistics = new EmptyStatistics();
            GameEndMessage message = new GameEndMessage(1234, 1235, statistics);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAME_END\",\"version\":\"1.0\",\"winnerID\":1234,\"loserID\":1235,\"statistics\":{}}", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message GameEndMessage with statistics
        /// </summary>
        [Test]
        public void TestFromGameEndMessage()
        {
            Statistics statistics = new Statistics();
            GameEndMessage message = new GameEndMessage(1234, 1235, statistics);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAME_END\",\"version\":\"1.0\",\"winnerID\":1234,\"loserID\":1235,\"statistics\":{\"HouseSpiceStorage\":0,\"TotalSpiceCollected\":0,\"EnemiesDefeated\":0,\"CharactersSwallowed\":0,\"CharactersAlive\":null,\"LastCharacterStanding\":false}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message GameStateMessage
        /// </summary>
        [Test]
        public void TestFromGameStateMessage()
        {
            string[] history = { "*MAP_CHANGE als String*", "*Mehrere SPAWN_CHARACTER als String*" };
            int[] activelyPlayingIDs = { 1235, 1345 };
            GameStateMessage message = new GameStateMessage(history, activelyPlayingIDs, 1);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAMESTATE\",\"version\":\"1.0\",\"clientID\":1,\"activelyPlayingIDs\":[1235,1345],\"history\":[\"*MAP_CHANGE als String*\",\"*Mehrere SPAWN_CHARACTER als String*\"]}", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message GameStateRequestMessage
        /// </summary>
        [Test]
        public void TestFromGameStateRequestMessage()
        {
            GameStateRequestMessage message = new GameStateRequestMessage(1234);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAMESTATE_REQUEST\",\"version\":\"1.0\",\"clientID\":1234}", serializedMessage);
        }

        // HouseAcknowledgementMessage
        /// <summary>
        /// This Testcase validates the serialization of the Message HouseAcknowledgementMessage
        /// </summary>
        [Test]
        public void TestFromHouseAcknowledgementMessage()
        {
            HouseAcknowledgementMessage message = new HouseAcknowledgementMessage(1234, "ATREIDES");
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"HOUSE_ACKNOWLEDGEMENT\",\"version\":\"1.0\",\"clientID\":1234,\"houseName\":\"ATREIDES\"}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message HouseOfferMessage
        /// </summary>
        [Test]
        public void TestFromHouseOfferMessage()
        {
            GreatHouse[] houses = new GreatHouse[2];

            houses[0] = new Corrino();
            houses[0].Characters.Add(new Noble());
            //HouseCharacter emperorShaddamIVCorrino = new HouseCharacter("EmperorShaddamIVCorrino", "NOBLE");
            //HouseCharacter princessIrulanCorrino = new HouseCharacter("PrincessIrulanCorrino", "BENE_GESSERIT");

            //HouseCharacter[] houseCharacters = { emperorShaddamIVCorrino , princessIrulanCorrino };
            //houses[0].houseCharacters = houseCharacters;
            //houses[1] = new Atreides(); 
            HouseOfferMessage message = new HouseOfferMessage(1234, houses);
            string serializedMessage = MessageConverter.FromMessage(message);

            Assert.AreEqual("{\"type\":\"HOUSE_OFFER\",\"version\":\"1.0\",\"clientID\":1234,\"houses\":[{\"houseName\":\"CORRINO\",\"houseColor\":\"GOLD\",\"houseCharacters\":[{\"characterName\":\"EmperorShaddamIVCorrino\",\"characterClass\":\"NOBLE\"},{\"characterName\":\"PrincessIrulanCorrino\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"CountHasimirFenring\",\"characterClass\":\"MENTAT\"},{\"characterName\":\"LadyMargotFenring\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"ReverendMotherGaiusHelenMohiam\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"CaptainAramsham\",", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message HouseRequestMessage
        /// </summary>
        [Test]
        public void TestFromHouseRequestMessage()
        {
            HouseRequestMessage message = new HouseRequestMessage("ATREIDES");
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"HOUSE_REQUEST\",\"version\":\"1.0\",\"houseName\":\"ATREIDES\"}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message JoinAcceptedMessage
        /// </summary>
        [Test]
        public void TestFromJoinAcceptedMessage()
        {
            JoinAcceptedMessage message = new JoinAcceptedMessage("secret1234",1234);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"JOINACCEPTED\",\"version\":\"1.0\",\"clientSecret\":\"secret1234\",\"clientID\":1234}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message JoinMessage
        /// </summary>
        [Test]
        public void TestFromJoinMessage()
        {
            JoinMessage message = new JoinMessage("name", true, false);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"JOIN\",\"version\":\"1.0\",\"clientName\":\"name\",\"isActive\":true,\"isCpu\":false}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message MapChangeDemandMessage
        /// </summary>
        [Test]
        public void TestFromMapChangeDemandMessage()
        {
            MapField[,] map = new MapField[1, 2];
            map[0, 0] = new MapField(false, false, 1234, new Position(1,2));
            map[0, 1] = new MapField(false, false, 4321, new Position(1, 2));
            MapChangeDemandMessage message = new MapChangeDemandMessage(MapChangeReasons.FAMILY_ATOMICS, map);
            string serializedMessage = MessageConverter.FromMessage(message);
            // {\"type\":\"MAP_CHANGE_DEMAND\",\"version\":\"0.1\",\"changeReason\":\"FAMILY_ATOMICS\",\"newMap\":[[{\"tileType\":\"CITY\",\"clientID\":1234,\"hasSpice\":false,\"isInSandstorm\":false},{\"tileType\":\"DUNE\",\"hasSpice\":false,\"isInSandstorm\":false},...],...],\"stormEye\":{\"x\":4,

            Assert.AreEqual("{\"type\":\"MAP_CHANGE_DEMAND\",\"version\":\"1.0\",\"changeReason\":\"FAMILY_ATOMICS\",\"newMap\":[[{\"tileType\":\"CITY\",\"clientID\":1234,\"hasSpice\":false,\"isInSandstorm\":false},{\"tileType\":\"CITY\",\"clientID\":4321,\"hasSpice\":false,\"isInSandstorm\":false}]]}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message MovementDemandMessage
        /// </summary>
        [Test]
        public void TestFromMovementDemandMessage()
        {
            List<Position> path = new List<Position>();
            path.Add(new Position(1, 2));
            path.Add(new Position(2, 2));
            path.Add(new Position(2, 3));
            MovementDemandMessage message = new MovementDemandMessage(1234, 12, path);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"MOVEMENT_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"specs\":{\"path\":[{\"x\":1,\"y\":2},{\"x\":2,\"y\":2},{\"x\":2,\"y\":3}]}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message MovementRequestMessage
        /// </summary>
        [Test]
        public void TestFromMovementRequestMessage()
        {
            List<Position> path = new List<Position>();
            path.Add(new Position(1, 2));
            path.Add(new Position(2, 2));
            path.Add(new Position(2, 3));
            MovementRequestMessage message = new MovementRequestMessage(1234, 12, path);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"MOVEMENT_REQUEST\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"specs\":{\"path\":[{\"x\":1,\"y\":2},{\"x\":2,\"y\":2},{\"x\":2,\"y\":3}]}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message PauseGameRequestMessage
        /// </summary>
        [Test]
        public void TestFromPauseGameRequestMessage()
        {
            PauseGameRequestMessage message = new PauseGameRequestMessage(true);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"PAUSE_REQUEST\",\"version\":\"1.0\",\"pause\":true}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SandwormDespawnDemandMessage
        /// </summary>
        [Test]
        public void TestFromSandwormDespawnDemandMessage()
        {
            SandwormDespawnDemandMessage message = new SandwormDespawnDemandMessage();
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"SANDWORM_DESPAWN_DEMAND\",\"version\":\"1.0\"}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SandwormMoveDemandMessage
        /// </summary>
        [Test]
        public void TestFromSandwormMoveDemandMessage()
        {
            List<Position> path = new List<Position>();
            path.Add(new Position(2, 3));
            path.Add(new Position(3, 3));
            path.Add(new Position(4, 3));
            SandwormMoveDemandMessage message = new SandwormMoveDemandMessage(path);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"SANDWORM_MOVE_DEMAND\",\"version\":\"1.0\",\"path\":[{\"x\":2,\"y\":3},{\"x\":3,\"y\":3},{\"x\":4,\"y\":3}]}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SandwormSpawnDemandMessage
        /// </summary>
        [Test]
        public void TestFromSandwormSpawnDemandMessage()
        {
            SandwormSpawnDemandMessage message = new SandwormSpawnDemandMessage(1234, 1236, new Position(2, 3));
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"SANDWORM_SPAWN_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":1236,\"position\":{\"x\":2,\"y\":3}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SpawnCharacterDemandMessage
        /// </summary>
        [Test]
        public void TestFromSpawnCharacterDemandMessage()
        {
            Character attributes = new Fighter(100, 75, 10, 3, 1, 4, 2, 10, 5, 3, false, true);
            SpawnCharacterDemandMessage message = new SpawnCharacterDemandMessage(1234, 12, "Vorname Nachname", new Position(0, 1), attributes);
            string serializedMessage = MessageConverter.FromMessage(message);
            // {\"type\":\"SPAWN_CHARACTER_DEMAND\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12,\"characterName\":\"VornameNachname\",\"position\":{\"x\":0,\"y\":1},\"attributes\":{\"characterType\":\"FIGHTHER\",\"healthMax\":100,\"healthCurrent\":75,\"healingHP\":10,\"MPmax\":3,\"MPcurrent\":1,\"APmax\":4,\"APcurrent\":2,\"attackDamage\":10,\"inventorySize\":5,\"\"killedBySandworm\":false,\"isLoud\":true}

            Assert.AreEqual("{\"type\":\"SPAWN_CHARACTER_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"characterName\":\"Vorname Nachname\",\"position\":{\"x\":0,\"y\":1},\"attributes\":{\"characterType\":\"FIGHTHER\",\"healthMax\":100,\"healthCurrent\":75,\"healingHP\":10,\"MPmax\":3,\"MPcurrent\":1,\"APmax\":4,\"APcurrent\":2,\"attackDamage\":10,\"inventorySize\":5,\"inventoryUsed\":3,\"killedBySandworm\":false,\"isLoud\":true}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message UnpauseGameOfferMessage
        /// </summary>
        [Test]
        public void TestFromUnpauseGameOfferMessage()
        {
            UnpauseGameOfferMessage message = new UnpauseGameOfferMessage(1234);
            string serializedMessage = MessageConverter.FromMessage(message);

            Assert.AreEqual("{\"type\":\"UNPAUSE_GAME_OFFER\",\"version\":\"1.0\",\"requestedByClientID\":1234}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message StrikeMessage
        /// </summary>
        [Test]
        public void TestFromStrikeMessage()
        {
            StrikeMessage message = new StrikeMessage(1234, "*fehlerhafte Message als String*", 4);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"STRIKE\",\"version\":\"1.0\",\"clientID\":1234,\"wrongMessage\":\"*fehlerhafte Message als String*\",\"count\":4}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message TurnDemandMessage
        /// </summary>
        [Test]
        public void TestFromTurnDemandMessage()
        {
            TurnDemandMessage message = new TurnDemandMessage(1234, 12);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"TURN_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12}", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message PausGameDemandMessage
        /// </summary>
        [Test]
        public void TestFromPausGameDemandMessage()
        {
            PausGameDemandMessage message = new PausGameDemandMessage(12, true);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAME_PAUSE_DEMAND\",\"version\":\"1.0\",\"requestedByClientID\":12,\"pause\":true}", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message TransferDemandMessage
        /// </summary>
        [Test]
        public void TestFromTransferDemandMessage()
        {
            TransferDemandMessage message = new TransferDemandMessage(1234, 12, 13);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"TRANSFER_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"targetID\":13}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message TransferRequestMessage
        /// </summary>
        [Test]
        public void TestFromTransferRequestMessage()
        {
            TransferRequestMessage message = new TransferRequestMessage(1234, 12, 13, 10);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"TRANSFER_REQUEST\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"targetID\":13,\"amount\":10}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message AtomicsUpdateDemandMessage
        /// </summary>
        [Test]
        public void TestFromAtomicsUpdateDemandMessage()
        {
            AtomicsUpdateDemandMessage message = new AtomicsUpdateDemandMessage(1234, false, 2);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"ATOMICS_UPDATE_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"shunned\":false,\"atomicsLeft\":2}", serializedMessage);
        }

        // The following tests are validating the ToMessage Method

        /// <summary>
        /// This Testcase validates the deserialization of the Message TurnDemandMessage
        /// </summary>
        [Test]
        public void TestToMessageActionDemandMessage()
        {
            string serializedMessage = "{\"type\":\"ACTION_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"action\":\"ATTACK\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);
            Assert.IsNotNull(deserializedMessage);
            Assert.IsInstanceOf<ActionDemandMessage>(deserializedMessage);
            ((ActionDemandMessage)deserializedMessage).GetMessageTypeAsString();
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message ActionRequestMessage
        /// </summary>
        [Test]
        public void TestToActionRequestMessage()
        {
         /* TODO implement
          * string serializedMessage = "{\"type\":\"ACTION_REQUEST\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12,\"action\":\"VOICE\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);
            Assert.IsNotNull(deserializedMessage);
            Assert.IsInstanceOf<ActionDemandMessage>(deserializedMessage);
            ((ActionRequestMessage)deserializedMessage).getMessageType(); */
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message ChangeCharacterStatisticsDemandMessage
        /// </summary>
        [Test]
        public void TestToChangeCharacterStatisticsDemandMessage()
        {
            /* todo: implement
            string serializedMessage = "";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);
            Assert.IsNotNull(deserializedMessage);
            Assert.IsInstanceOf<ActionDemandMessage>(deserializedMessage);
            ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).getMessageType(); 
            */
        }

        // TODO: implement tests for these messages..

        //ChangePlayerSpiceDemandMessage

        //CreateMessage

        //DebugMessage

        //EndGameMessage

        // EndTurnRequestMessage

        // GameConfigMessage

        // GameEndMessage

        //GameStateMessage

        //GameStateRequestMessage

        //HouseAcknowledgementMessage

        //HouseOfferMessage

        //HouseRequestMessage

        //JoinAcceptedMessage

        //JoinMessage

        //MapChangeDemandMessage

        //MovementDemandMessage

        //MovementRequestMessage

        //PauseGameMessage

        //PauseGameRequestMessage

        //SandwormDespawnDemandMessage

        //SandwormMoveDemandMessage

        // SandwormSpawnDemandMessage

        // SpawnCharacterDemandMessage

        // StrikeMessage

        // TurnDemandMessage

        // TurnRequestMessage

        // RejoinMessage

        // AtomicsUpdateDemandMessage

    }
}