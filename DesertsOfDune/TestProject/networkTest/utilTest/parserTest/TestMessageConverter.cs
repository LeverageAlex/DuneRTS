using NUnit.Framework;
using GameData.network.util.parser;
using GameData.network.messages;
using GameData.network.util.world;

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
            ActionDemandMessage message = new ActionDemandMessage(1234, 12, ActionType.ATTACK, new Position(2, 3), 4);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"ACTION_DEMAND\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12,\"action\":\"ATTACK\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message ActionRequestMessage
        /// </summary>
        [Test]
        public void TestFromActionRequestMessage()
        {
            ActionRequestMessage message = new ActionRequestMessage(1234, 12, ActionType.VOICE, new Position(2, 3));
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"ACTION_REQUEST\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12,\"action\":\"VOICE\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message ChangePlayerSpiceDemandMessage
        /// </summary>
        [Test]
        public void TestFromChangePlayerSpiceDemandMessage()
        {
            ChangePlayerSpiceDemandMessage message = new ChangePlayerSpiceDemandMessage(5, 123123);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"CHANGE_PLAYER_SPICE_DEMAND\",\"version\":\"0.1\",\"clientID\":123123,\"newSpiceValue\":5}", serializedMessage);
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
            Assert.AreEqual("{\"type\":\"CHARACTER_STAT_CHANGE_DEMAND\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12,\"stats\":{\"HP\":10,\"AP\":4,\"MP\":3,\"spice\":8,\"isLoud\":false,\"isSwallowed\":false}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message CreateMessage
        /// </summary>
        [Test]
        public void TestFromCreateMessage()
        {
            CreateMessage message = new CreateMessage("SecretArena123", false);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"CREATE\",\"version\":\"1.0\",\"lobbyCode\":\"SecretArena123\",\"spectate\":false}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message DebugMessage
        /// </summary>
        [Test]
        public void TestFromDebugMessage()
        {
            DebugMessage message = new DebugMessage(1, "explenation");
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("\"code\":\"1\",\"explanation\":\"explenation\"", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message EndGameMessage
        /// </summary>
        [Test]
        public void TestFromEndGameMessage()
        {
            EndGameMessage message = new EndGameMessage();
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\": \"ENDGAME\",\"version\":\"0.1\"}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message EndTurnRequestMessage
        /// </summary>
        [Test]
        public void TestFromEndTurnRequestMessage()
        {
            EndTurnRequestMessage message = new EndTurnRequestMessage(1234, 12);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"END_TURN_REQUEST\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message GameConfigMessage
        /// </summary>
        [Test]
        public void TestFromGameConfigMessage()
        {
            GameConfigMessage message = new GameConfigMessage(null, "", null);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("\"scenario\":[[\"String\",\"String\"],[\"String\",\"String\"]],\"party\": {\"$ref\": \"#/definitions/partiekonfigschema\"},\"client0ID\":\"2\",\"client1ID\":\"3\"", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message GameEndMessage
        /// </summary>
        [Test]
        public void TestFromGameEndMessage()
        {
            GameEndMessage message = new GameEndMessage(1, 2, null);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAME_END\",\"version\":\"0.1\",\"winnerID\":1234,\"loserID\":1235,\"statistics\":{ }}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message GameStateMessage
        /// </summary>
        [Test]
        public void TestFromGameStateMessage()
        {
            GameStateMessage message = new GameStateMessage(null, 1);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAMESTATE\",\"version\":\"0.1\",\"clientID\":1234,\"activelyPlayingIDs\":[1235,1345],\"history\":[\" * MAP_CHANGE als String * "," * Mehrere SPAWN_CHARACTER als String * \",", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message GameStateRequestMessage
        /// </summary>
        [Test]
        public void TestFromGameStateRequestMessage()
        {
            GameStateRequestMessage message = new GameStateRequestMessage(1234);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"REQUEST_GAMESTATE\",\"version\":\"0.1\",\"clientID\":1234}", serializedMessage);
        }

        // HouseAcknowledgementMessage
        /// <summary>
        /// This Testcase validates the serialization of the Message HouseAcknowledgementMessage
        /// </summary>
        [Test]
        public void TestFromHouseAcknowledgementMessage()
        {
            HouseAcknowledgementMessage message = new HouseAcknowledgementMessage(1, "test");
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"HOUSE_ACKNOWLEDGEMENT\",\"version\":\"0.1\",\"clientID\":1234,\"houseName\":\"ATREIDES\"}", serializedMessage);
        }
        //HouseOfferMessage

        /// <summary>
        /// This Testcase validates the serialization of the Message HouseOfferMessage
        /// </summary>
        [Test]
        public void TestFromHouseOfferMessage()
        {
            HouseOfferMessage message = new HouseOfferMessage(1, null);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"HOUSE_OFFER\",\"version\":\"0.1\",\"clientID\":1234,\"houses\":[{\"houseName\":\"CORRINO\",\"houseColor\":\"GOLD\",\"houseCharacters\":[{\"characterName\":\"Emperor Shaddam IV Corrino\",\"characterClass\":\"NOBLE\"},{\"characterName\":\"Princess Irulan Corrino\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Count Hasimir Fenring\",\"characterClass\":\"MENTAT\"},{\"characterName\":\"Lady Margot Fenring\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Reverend Mother Gaius Helen Mohiam\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Captain Aramsham\",", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message HouseRequestMessage
        /// </summary>
        [Test]
        public void TestFromHouseRequestMessage()
        {
            HouseRequestMessage message = new HouseRequestMessage("");
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message JoinAcceptedMessage
        /// </summary>
        [Test]
        public void TestFromJoinAcceptedMessage()
        {
            JoinAcceptedMessage message = new JoinAcceptedMessage("");
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message JoinMessage
        /// </summary>
        [Test]
        public void TestFromJoinMessage()
        {
            JoinMessage message = new JoinMessage("", "", true);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message MapChangeDemandMessage
        /// </summary>
        [Test]
        public void TestFromMapChangeDemandMessage()
        {
            MapChangeDemandMessage message = new MapChangeDemandMessage(MapChangeReasons.ROUND_PHASE, null);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message MovementDemandMessage
        /// </summary>
        [Test]
        public void TestFromMovementDemandMessage()
        {
            MovementDemandMessage message = new MovementDemandMessage(1, 2, null);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message MovementRequestMessage
        /// </summary>
        [Test]
        public void TestFromMovementRequestMessage()
        {
            MovementRequestMessage message = new MovementRequestMessage(1, 2, null);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message PauseGameMessage
        /// </summary>
        [Test]
        public void TestFromPauseGameMessage()
        {
            GamePauseDemandMessage message = new GamePauseDemandMessage(2, true);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message PauseGameRequestMessage
        /// </summary>
        [Test]
        public void TestFromPauseGameRequestMessage()
        {
            PauseGameRequestMessage message = new PauseGameRequestMessage(true);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SandwormDespawnDemandMessage
        /// </summary>
        [Test]
        public void TestFromSandwormDespawnDemandMessage()
        {
            SandwormDespawnDemandMessage message = new SandwormDespawnDemandMessage();
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SandwormMoveDemandMessage
        /// </summary>
        [Test]
        public void TestFromSandwormMoveDemandMessage()
        {
            SandwormMoveDemandMessage message = new SandwormMoveDemandMessage(null);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SandwormSpawnDemandMessage
        /// </summary>
        [Test]
        public void TestFromSandwormSpawnDemandMessage()
        {
            SandwormSpawnDemandMessage message = new SandwormSpawnDemandMessage(1, 2, new Position(2, 3));
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message SpawnCharacterDemandMessage
        /// </summary>
        [Test]
        public void TestFromSpawnCharacterDemandMessage()
        {
            SpawnCharacterDemandMessage message = new SpawnCharacterDemandMessage(1, 2, "", new Position(2, 3), null, CharacterType.NOBEL);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message StrikeMessage
        /// </summary>
        [Test]
        public void TestFromStrikeMessage()
        {
            StrikeMessage message = new StrikeMessage(1, "asd", 2);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message TurnRequestMessage
        /// </summary>
        [Test]
        public void TestFromTurnRequestMessage()
        {
            TurnRequestMessage message = new TurnRequestMessage(1, 2);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message TurnDemandMessage
        /// </summary>
        [Test]
        public void TestFromTurnDemandMessage()
        {
            TurnDemandMessage message = new TurnDemandMessage(1, 2);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("", serializedMessage);
        }

        // The following tests are validating the ToMessage Method

        /// <summary>
        /// This Testcase validates the deserialization of the Message TurnDemandMessage
        /// </summary>
        [Test]
        public void TestToMessageActionDemandMessage()
        {
            string serializedMessage = "{\"type\":\"ACTION_DEMAND\",\"version\":\"0.1\",\"clientID\":1234,\"characterID\":12,\"action\":\"ATTACK\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);
            Assert.IsNotNull(deserializedMessage);
            Assert.IsInstanceOf<ActionDemandMessage>(deserializedMessage);
            ((ActionDemandMessage)deserializedMessage).getMessageType();
        }
    }
}