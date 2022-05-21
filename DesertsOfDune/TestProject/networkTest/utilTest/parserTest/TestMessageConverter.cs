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

    }
}