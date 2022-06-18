using NUnit.Framework;
using GameData.network.util.parser;
using GameData.network.messages;
using GameData.network.util.world;
using System.Collections.Generic;
using Server.Configuration;
using GameData.Configuration;
using GameData.network.util.world.character;
using GameData.network.util.enums;
using GameData.network.util.world.mapField;

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
            ActionRequestMessage message = new ActionRequestMessage(1234, 12, ActionType.VOICE, new Specs (new Position(2, 3), null));
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
            CityToClient cityToClient1 = new CityToClient(1234, 2, 3);
            CityToClient cityToClient2 = new CityToClient(1234, 6, 6);
            CityToClient[] citiesToClients = { cityToClient1, cityToClient2 };
            GameConfigMessage message = new GameConfigMessage(scenario, party, citiesToClients, new Position(0, 1));
            string serializedMessage = MessageConverter.FromMessage(message);

            Assert.AreEqual("{\"type\":\"GAMECFG\",\"version\":\"1.0\",\"scenario\":[[\"String\",\"String\"],[\"String\",\"String\"]],\"party\":{\"refr\":\"#/definitions/partiekonfigschema\"},\"cityToClient\":[{\"clientID\":1234,\"x\":2,\"y\":3},{\"clientID\":1234,\"x\":6,\"y\":6}],\"stormEye\":{\"x\":0,\"y\":1}}", serializedMessage);
        }

        /// <summary>
        /// This Testcase validates the serialization of the Message GameEndMessage with empty statistics
        /// </summary>
        [Test]
        public void TestFromGameEndMessageEmptyStats()
        {
            EmptyStatistics stat1 = new EmptyStatistics();
            EmptyStatistics stat2 = new EmptyStatistics();
            Statistics[] statistics = { stat1, stat2 };
            GameEndMessage message = new GameEndMessage(1234, 1235, statistics);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAME_END\",\"version\":\"1.0\",\"winnerID\":1234,\"loserID\":1235,\"statistics\":[{},{}]}", serializedMessage);
        }


        /// <summary>
        /// This Testcase validates the serialization of the Message GameEndMessage with statistics
        /// </summary>
        [Test]
        public void TestFromGameEndMessage()
        {
            Statistics stat1 = new Statistics();
            stat1.HouseSpiceStorage = 10;
            stat1.TotalSpiceCollected = 12;
            stat1.EnemiesDefeated = 1;
            stat1.CharactersSwallowed = 2;
            stat1.LastCharacterStanding = true;
            Statistics stat2 = new Statistics();
            stat2.HouseSpiceStorage = 8;
            stat2.TotalSpiceCollected = 10;
            stat2.EnemiesDefeated = 0;
            stat2.CharactersSwallowed = 1;
            stat2.LastCharacterStanding = false;
            Statistics[] statistics = { stat1, stat2 };
            GameEndMessage message = new GameEndMessage(1234, 1235, statistics);
            string serializedMessage = MessageConverter.FromMessage(message);
            Assert.AreEqual("{\"type\":\"GAME_END\",\"version\":\"1.0\",\"winnerID\":1234,\"loserID\":1235,\"statistics\":[{\"HouseSpiceStorage\":10,\"TotalSpiceCollected\":12,\"EnemiesDefeated\":1,\"CharactersSwallowed\":2,\"CharactersAlive\":null,\"LastCharacterStanding\":true},{\"HouseSpiceStorage\":8,\"TotalSpiceCollected\":10,\"EnemiesDefeated\":0,\"CharactersSwallowed\":1,\"CharactersAlive\":null,\"LastCharacterStanding\":false}]}", serializedMessage);
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

            HouseCharacter houseCharacter1 = new HouseCharacter("Emperor Shaddam IV Corrino", "NOBLE");
            HouseCharacter houseCharacter2 = new HouseCharacter("Princess Irulan Corrino", "BENE_GESSERIT");
            HouseCharacter houseCharacter3 = new HouseCharacter("Count Hasimir Fenring", "MENTAT");
            HouseCharacter houseCharacter4 = new HouseCharacter("Lady Margot Fenring", "BENE_GESSERIT");
            HouseCharacter houseCharacter5 = new HouseCharacter("Reverend Mother Gaius Helen Mohiam", "BENE_GESSERIT");
            HouseCharacter houseCharacter6 = new HouseCharacter("Captain Aramsham", "BENE_GESSERIT");

            HouseCharacter[] characters = new HouseCharacter[] { houseCharacter1, houseCharacter2, houseCharacter3, houseCharacter4, houseCharacter5, houseCharacter6 };
            Corrino c = new Corrino(characters);

            houses[0] = c;


            HouseOfferMessage message = new HouseOfferMessage(1234, houses);
            string serializedMessage = MessageConverter.FromMessage(message);

            Assert.AreEqual("{\"type\":\"HOUSE_OFFER\",\"version\":\"1.0\",\"clientID\":1234,\"houses\":[{\"houseName\":\"CORRINO\",\"houseColor\":\"GOLD\",\"houseCharacters\":[{\"characterName\":\"Emperor Shaddam IV Corrino\",\"characterClass\":\"NOBLE\"},{\"characterName\":\"Princess Irulan Corrino\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Count Hasimir Fenring\",\"characterClass\":\"MENTAT\"},{\"characterName\":\"Lady Margot Fenring\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Reverend Mother Gaius Helen Mohiam\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Captain Aramsham\",\"characterClass\":\"BENE_GESSERIT\"}]},null]}", serializedMessage);
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
            JoinAcceptedMessage message = new JoinAcceptedMessage("secret1234", 1234);
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
            map[0, 0] = new GameData.network.util.world.mapField.City(1234, false, false);
            map[0, 1] = new Dune(false, false);
            MapChangeDemandMessage message = new MapChangeDemandMessage(MapChangeReasons.FAMILY_ATOMICS, map, new Position(1,2));
            string serializedMessage = MessageConverter.FromMessage(message);

            Assert.AreEqual("{\"type\":\"MAP_CHANGE_DEMAND\",\"version\":\"1.0\",\"changeReason\":\"FAMILY_ATOMICS\",\"newMap\":[[{\"tileType\":\"CITY\",\"clientID\":1234,\"hasSpice\":false,\"isInSandstorm\":false},{\"tileType\":\"DUNE\",\"hasSpice\":false,\"isInSandstorm\":false}]],\"stormEye\":{\"x\":1,\"y\":2}}", serializedMessage);
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
            MovementDemandMessage message = new MovementDemandMessage(1234, 12, new Specs(null,path));
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
            MovementRequestMessage message = new MovementRequestMessage(1234, 12, new Specs(null,path));
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

            Assert.AreEqual("{\"type\":\"SPAWN_CHARACTER_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"characterName\":\"Vorname Nachname\",\"position\":{\"x\":0,\"y\":1},\"attributes\":{\"characterType\":\"FIGHTER\",\"healthMax\":100,\"healthCurrent\":75,\"healingHP\":10,\"MPmax\":3,\"MPcurrent\":1,\"APmax\":4,\"APcurrent\":2,\"attackDamage\":10,\"inventorySize\":5,\"inventoryUsed\":3,\"killedBySandworm\":false,\"isLoud\":true}}", serializedMessage);
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

            Assert.AreEqual("ACTION_DEMAND", ((ActionDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((ActionDemandMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((ActionDemandMessage)deserializedMessage).clientID);
            Assert.AreEqual(12, ((ActionDemandMessage)deserializedMessage).characterID);
            Assert.AreEqual("ATTACK", ((ActionDemandMessage)deserializedMessage).action);
            Assert.AreEqual(2, ((ActionDemandMessage)deserializedMessage).specs.target.x);
            Assert.AreEqual(3, ((ActionDemandMessage)deserializedMessage).specs.target.y);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message ActionRequestMessage
        /// </summary>
        [Test]
        public void TestToActionRequestMessage()
        {
            string serializedMessage = "{\"type\":\"ACTION_REQUEST\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"action\":\"VOICE\",\"specs\":{\"target\":{\"x\":2,\"y\":3}}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("ACTION_REQUEST", ((ActionRequestMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((ActionRequestMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((ActionRequestMessage)deserializedMessage).clientID);
            Assert.AreEqual(12, ((ActionRequestMessage)deserializedMessage).characterID);
            Assert.AreEqual("VOICE", ((ActionRequestMessage)deserializedMessage).action);
            //Assert.Null(((ActionRequestMessage)deserializedMessage).specs);
            Assert.AreEqual(2, ((ActionRequestMessage)deserializedMessage).specs.target.x);
            Assert.AreEqual(3, ((ActionRequestMessage)deserializedMessage).specs.target.y);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message ChangeCharacterStatisticsDemandMessage
        /// </summary>
        [Test]
        public void TestToChangeCharacterStatisticsDemandMessage()
        {
            string serializedMessage = "{\"type\":\"CHARACTER_STAT_CHANGE_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"stats\":{\"HP\":10,\"AP\":4,\"MP\":3,\"spice\":8,\"isLoud\":false,\"isSwallowed\":false}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("CHARACTER_STAT_CHANGE_DEMAND", ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).clientID);
            Assert.AreEqual(12, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).characterID);
            Assert.AreEqual(10, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).stats.HP);
            Assert.AreEqual(4, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).stats.AP);
            Assert.AreEqual(3, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).stats.MP);
            Assert.AreEqual(8, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).stats.spice);
            Assert.AreEqual(false, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).stats.isLoud);
            Assert.AreEqual(false, ((ChangeCharacterStatisticsDemandMessage)deserializedMessage).stats.isSwallowed);

        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message HouseOfferMessage
        /// </summary>
        [Test]
        public void TestToHouseOfferMessage()
        {
            string serializedMessage = "{\"type\":\"HOUSE_OFFER\",\"version\":\"1.0\",\"clientID\":1234,\"houses\":[{\"houseName\":\"CORRINO\",\"houseColor\":\"GOLD\",\"houseCharacters\":[{\"characterName\":\"Emperor Shaddam IV Corrino\",\"characterClass\":\"NOBLE\"},{\"characterName\":\"Princess Irulan Corrino\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Count Hasimir Fenring\",\"characterClass\":\"MENTAT\"},{\"characterName\":\"Lady Margot Fenring\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Reverend Mother Gaius Helen Mohiam\",\"characterClass\":\"BENE_GESSERIT\"},{\"characterName\":\"Captain Aramsham\",\"characterClass\":\"BENE_GESSERIT\"}]},null]}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("HOUSE_OFFER", ((HouseOfferMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual(2, ((HouseOfferMessage)deserializedMessage).houses.Length);
            Assert.AreEqual("CORRINO", ((HouseOfferMessage)deserializedMessage).houses[0].houseName);
            Assert.AreEqual("GOLD", ((HouseOfferMessage)deserializedMessage).houses[0].houseColor);
            Assert.AreEqual("Emperor Shaddam IV Corrino", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[0].characterName);
            Assert.AreEqual("NOBLE", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[0].characterClass);
            Assert.AreEqual("Princess Irulan Corrino", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[1].characterName);
            Assert.AreEqual("BENE_GESSERIT", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[1].characterClass);
            Assert.AreEqual("Count Hasimir Fenring", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[2].characterName);
            Assert.AreEqual("MENTAT", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[2].characterClass);
            Assert.AreEqual("Lady Margot Fenring", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[3].characterName);
            Assert.AreEqual("BENE_GESSERIT", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[3].characterClass);
            Assert.AreEqual("Reverend Mother Gaius Helen Mohiam", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[4].characterName);
            Assert.AreEqual("BENE_GESSERIT", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[4].characterClass);
            Assert.AreEqual("Captain Aramsham", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[5].characterName);
            Assert.AreEqual("BENE_GESSERIT", ((HouseOfferMessage)deserializedMessage).houses[0].houseCharacters[5].characterClass);
            Assert.Null(((HouseOfferMessage)deserializedMessage).houses[1]);
        }


        /// <summary>
        /// This Testcase validates the deserialization of the Message ChangePlayerSpiceDemandMessage
        /// </summary>
        [Test]
        public void TestToChangePlayerSpiceDemandMessage()
        {
            string serializedMessage = "{\"type\":\"CHANGE_PLAYER_SPICE_DEMAND\",\"version\":\"1.0\",\"clientID\":123123,\"newSpiceValue\":5}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("CHANGE_PLAYER_SPICE_DEMAND", ((ChangePlayerSpiceDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((ChangePlayerSpiceDemandMessage)deserializedMessage).version);
            Assert.AreEqual(123123, ((ChangePlayerSpiceDemandMessage)deserializedMessage).clientID);
            Assert.AreEqual(5, ((ChangePlayerSpiceDemandMessage)deserializedMessage).newSpiceValue);
        }


        /// <summary>
        /// This Testcase validates the deserialization of the Message CreateMessage
        /// </summary>
        [Test]
        public void TestToDebugMessage()
        {
            string serializedMessage = "{\"type\":\"DEBUG\",\"version\":\"1.0\",\"code\":1,\"explanation\":\"explenation\"}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("DEBUG", ((DebugMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((DebugMessage)deserializedMessage).version);
            Assert.AreEqual(1, ((DebugMessage)deserializedMessage).code);
            Assert.AreEqual("explenation", ((DebugMessage)deserializedMessage).explanation);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message EndGameMessage
        /// </summary>
        [Test]
        public void TestToEndGameMessage()
        {
            string serializedMessage = "{\"type\":\"ENDGAME\",\"version\":\"1.0\"}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("ENDGAME", ((EndGameMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((EndGameMessage)deserializedMessage).version);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message EndTurnRequestMessage
        /// </summary>
        [Test]
        public void TestToEndTurnRequestMessage()
        {
            string serializedMessage = "{\"type\":\"END_TURN_REQUEST\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("END_TURN_REQUEST", ((EndTurnRequestMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((EndTurnRequestMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((EndTurnRequestMessage)deserializedMessage).clientID);
            Assert.AreEqual(12, ((EndTurnRequestMessage)deserializedMessage).characterID);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message GameConfigMessage
        /// </summary>
        [Test]
        public void TestToGameConfigMessage()
        {
            string serializedMessage = "{\"type\":\"GAMECFG\",\"version\":\"1.0\",\"scenario\":[[\"String\",\"String\"],[\"String\",\"String\"]],\"party\":{\"refr\":\"#/definitions/partiekonfigschema\"},\"cityToClient\":[{\"clientID\":1234,\"x\":2,\"y\":3},{\"clientID\":1234,\"x\":6,\"y\":6}],\"stormEye\":{\"x\":0,\"y\":1}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("GAMECFG", ((GameConfigMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((GameConfigMessage)deserializedMessage).version);
            Assert.AreEqual(2, ((GameConfigMessage)deserializedMessage).scenario.Count);
            Assert.AreEqual("String", ((GameConfigMessage)deserializedMessage).scenario[0][0]);
            Assert.AreEqual("String", ((GameConfigMessage)deserializedMessage).scenario[0][1]);
            Assert.AreEqual("String", ((GameConfigMessage)deserializedMessage).scenario[1][0]);
            Assert.AreEqual("String", ((GameConfigMessage)deserializedMessage).scenario[1][1]);
            Assert.AreEqual("#/definitions/partiekonfigschema", ((GameConfigMessage)deserializedMessage).party.refr);
            Assert.AreEqual(2, ((GameConfigMessage)deserializedMessage).cityToClient[0].x);
            Assert.AreEqual(3, ((GameConfigMessage)deserializedMessage).cityToClient[0].y);
            Assert.AreEqual(1234, ((GameConfigMessage)deserializedMessage).cityToClient[0].clientID);
            Assert.AreEqual(6, ((GameConfigMessage)deserializedMessage).cityToClient[1].x);
            Assert.AreEqual(6, ((GameConfigMessage)deserializedMessage).cityToClient[1].y);
            Assert.AreEqual(1234, ((GameConfigMessage)deserializedMessage).cityToClient[1].clientID);
            Assert.AreEqual(0, ((GameConfigMessage)deserializedMessage).stormEye.x);
            Assert.AreEqual(1, ((GameConfigMessage)deserializedMessage).stormEye.y);

        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message GameEndMessage
        /// </summary>
        [Test]
        public void TestToGameEndMessage()
        {
            string serializedMessage = "{\"type\":\"GAME_END\",\"version\":\"1.0\",\"winnerID\":1234,\"loserID\":1235,\"statistics\":[{\"HouseSpiceStorage\":2,\"TotalSpiceCollected\":3,\"EnemiesDefeated\":1,\"CharactersSwallowed\":2,\"CharactersAlive\":null,\"LastCharacterStanding\":false},{\"HouseSpiceStorage\":0,\"TotalSpiceCollected\":0,\"EnemiesDefeated\":0,\"CharactersSwallowed\":0,\"CharactersAlive\":null,\"LastCharacterStanding\":false}]}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("GAME_END", ((GameEndMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((GameEndMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((GameEndMessage)deserializedMessage).winnerID);
            Assert.AreEqual(1235, ((GameEndMessage)deserializedMessage).loserID);
            Assert.AreEqual(2, ((GameEndMessage)deserializedMessage).statistics[0].HouseSpiceStorage);
            Assert.AreEqual(3, ((GameEndMessage)deserializedMessage).statistics[0].TotalSpiceCollected);
            Assert.AreEqual(1, ((GameEndMessage)deserializedMessage).statistics[0].EnemiesDefeated);
            Assert.AreEqual(2, ((GameEndMessage)deserializedMessage).statistics[0].CharactersSwallowed);
            Assert.Null(((GameEndMessage)deserializedMessage).statistics[0].CharactersAlive);
            Assert.AreEqual(false, ((GameEndMessage)deserializedMessage).statistics[0].LastCharacterStanding);
            Assert.AreEqual(0, ((GameEndMessage)deserializedMessage).statistics[1].HouseSpiceStorage);
            Assert.AreEqual(0, ((GameEndMessage)deserializedMessage).statistics[1].TotalSpiceCollected);
            Assert.AreEqual(0, ((GameEndMessage)deserializedMessage).statistics[1].EnemiesDefeated);
            Assert.AreEqual(0, ((GameEndMessage)deserializedMessage).statistics[1].CharactersSwallowed);
            Assert.Null(((GameEndMessage)deserializedMessage).statistics[1].CharactersAlive);
            Assert.AreEqual(false, ((GameEndMessage)deserializedMessage).statistics[1].LastCharacterStanding);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message GameStateMessage
        /// </summary>
        [Test]
        public void TestToGameStateMessage()
        {
            string serializedMessage = "{\"type\":\"GAMESTATE\",\"version\":\"1.0\",\"clientID\":1,\"activelyPlayingIDs\":[1235,1345],\"history\":[\"*MAP_CHANGE als String*\",\"*Mehrere SPAWN_CHARACTER als String*\"]}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("GAMESTATE", ((GameStateMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((GameStateMessage)deserializedMessage).version);
            Assert.AreEqual(1, ((GameStateMessage)deserializedMessage).clientID);
            Assert.AreEqual(1235, ((GameStateMessage)deserializedMessage).activelyPlayingIDs[0]);
            Assert.AreEqual(1345, ((GameStateMessage)deserializedMessage).activelyPlayingIDs[1]);
            Assert.AreEqual("*MAP_CHANGE als String*", ((GameStateMessage)deserializedMessage).history[0]);
            Assert.AreEqual("*Mehrere SPAWN_CHARACTER als String*", ((GameStateMessage)deserializedMessage).history[1]);
            Assert.AreEqual(2, ((GameStateMessage)deserializedMessage).history.Length);

        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message GameStateRequestMessage
        /// </summary>
        [Test]
        public void TestToGameStateRequestMessage()
        {
            string serializedMessage = "{\"type\":\"GAMESTATE_REQUEST\",\"version\":\"1.0\",\"clientID\":1234}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("GAMESTATE_REQUEST", ((GameStateRequestMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((GameStateRequestMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((GameStateRequestMessage)deserializedMessage).clientID);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message HouseAcknowledgementMessage
        /// </summary>
        [Test]
        public void TestToHouseAcknowledgementMessage()
        {
            string serializedMessage = "{\"type\":\"HOUSE_ACKNOWLEDGEMENT\",\"version\":\"1.0\",\"clientID\":1234,\"houseName\":\"ATREIDES\"}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("HOUSE_ACKNOWLEDGEMENT", ((HouseAcknowledgementMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((HouseAcknowledgementMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((HouseAcknowledgementMessage)deserializedMessage).clientID);
            Assert.AreEqual("ATREIDES", ((HouseAcknowledgementMessage)deserializedMessage).houseName);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message HouseRequestMessage
        /// </summary>
        [Test]
        public void TestToHouseRequestMessage()
        {
            string serializedMessage = "{\"type\":\"HOUSE_REQUEST\",\"version\":\"1.0\",\"houseName\":\"ATREIDES\"}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("HOUSE_REQUEST", ((HouseRequestMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((HouseRequestMessage)deserializedMessage).version);
            Assert.AreEqual("ATREIDES", ((HouseRequestMessage)deserializedMessage).houseName);
        }


        /// <summary>
        /// This Testcase validates the deserialization of the Message JoinAcceptedMessage
        /// </summary>
        [Test]
        public void TestToJoinAcceptedMessage()
        {
            string serializedMessage = "{\"type\":\"JOINACCEPTED\",\"version\":\"1.0\",\"clientSecret\":\"secret1234\",\"clientID\":1234}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("JOINACCEPTED", ((JoinAcceptedMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((JoinAcceptedMessage)deserializedMessage).version);
            Assert.AreEqual("secret1234", ((JoinAcceptedMessage)deserializedMessage).clientSecret);
            Assert.AreEqual(1234, ((JoinAcceptedMessage)deserializedMessage).clientID);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message JoinMessage
        /// </summary>
        [Test]
        public void TestToJoinMessage()
        {
            string serializedMessage = "{\"type\":\"JOIN\",\"version\":\"1.0\",\"clientName\":\"name\",\"isActive\":true,\"isCpu\":false}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("JOIN", ((JoinMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((JoinMessage)deserializedMessage).version);
            Assert.AreEqual("name", ((JoinMessage)deserializedMessage).clientName);
            Assert.AreEqual(true, ((JoinMessage)deserializedMessage).isActive);
            Assert.AreEqual(false, ((JoinMessage)deserializedMessage).isCpu);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message MapChangeDemandMessage
        /// </summary>
        [Test]
        public void TestToMapChangeDemandMessage()
        {
            string serializedMessage = "{\"type\":\"MAP_CHANGE_DEMAND\",\"version\":\"1.0\",\"changeReason\":\"FAMILY_ATOMICS\",\"newMap\":[[{\"tileType\":\"CITY\",\"clientID\":1234,\"hasSpice\":false,\"isInSandstorm\":false},{\"tileType\":\"DUNE\",\"hasSpice\":false,\"isInSandstorm\":false}]],\"stormEye\":{\"x\":4,\"y\":5}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("MAP_CHANGE_DEMAND", ((MapChangeDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((MapChangeDemandMessage)deserializedMessage).version);
            Assert.AreEqual("FAMILY_ATOMICS", ((MapChangeDemandMessage)deserializedMessage).changeReason);
            Assert.AreEqual("CITY", ((MapChangeDemandMessage)deserializedMessage).newMap[0,0].tileType);
            Assert.AreEqual(1234, ((MapChangeDemandMessage)deserializedMessage).newMap[0,0].clientID);
            Assert.AreEqual(false, ((MapChangeDemandMessage)deserializedMessage).newMap[0,0].hasSpice);
            Assert.AreEqual(false, ((MapChangeDemandMessage)deserializedMessage).newMap[0,0].isInSandstorm);
            Assert.AreEqual("DUNE", ((MapChangeDemandMessage)deserializedMessage).newMap[0,1].tileType);
            Assert.AreEqual(false, ((MapChangeDemandMessage)deserializedMessage).newMap[0,1].hasSpice);
            Assert.AreEqual(false, ((MapChangeDemandMessage)deserializedMessage).newMap[0,1].isInSandstorm);
            Assert.AreEqual(0, ((MapChangeDemandMessage)deserializedMessage).newMap[0, 1].clientID);
            Assert.AreEqual(4, ((MapChangeDemandMessage)deserializedMessage).stormEye.x);
            Assert.AreEqual(5, ((MapChangeDemandMessage)deserializedMessage).stormEye.y);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message MovementDemandMessage
        /// </summary>
        [Test]
        public void TestToMovementDemandMessage()
        {
            string serializedMessage = "{\"type\":\"MOVEMENT_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"specs\":{\"path\":[{\"x\":1,\"y\":2},{\"x\":2,\"y\":2},{\"x\":2,\"y\":3}]}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("MOVEMENT_DEMAND", ((MovementDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((MovementDemandMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((MovementDemandMessage)deserializedMessage).clientID);
            Assert.AreEqual(12, ((MovementDemandMessage)deserializedMessage).characterID);
            Assert.AreEqual(1, ((MovementDemandMessage)deserializedMessage).specs.path[0].x);
            Assert.AreEqual(2, ((MovementDemandMessage)deserializedMessage).specs.path[0].y);
            Assert.AreEqual(2, ((MovementDemandMessage)deserializedMessage).specs.path[1].x);
            Assert.AreEqual(2, ((MovementDemandMessage)deserializedMessage).specs.path[1].y);
            Assert.AreEqual(2, ((MovementDemandMessage)deserializedMessage).specs.path[2].x);
            Assert.AreEqual(3, ((MovementDemandMessage)deserializedMessage).specs.path[2].y);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message MovementRequestMessage
        /// </summary>
        [Test]
        public void TestToMovementRequestMessage()
        {
            string serializedMessage = "{\"type\":\"MOVEMENT_REQUEST\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"specs\":{\"path\":[{\"x\":1,\"y\":2},{\"x\":2,\"y\":2},{\"x\":2,\"y\":3}]}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("MOVEMENT_REQUEST", ((MovementRequestMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((MovementRequestMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((MovementRequestMessage)deserializedMessage).clientID);
            Assert.AreEqual(12, ((MovementRequestMessage)deserializedMessage).characterID);
            Assert.AreEqual(1, ((MovementRequestMessage)deserializedMessage).specs.path[0].x);
            Assert.AreEqual(2, ((MovementRequestMessage)deserializedMessage).specs.path[0].y);
            Assert.AreEqual(2, ((MovementRequestMessage)deserializedMessage).specs.path[1].x);
            Assert.AreEqual(2, ((MovementRequestMessage)deserializedMessage).specs.path[1].y);
            Assert.AreEqual(2, ((MovementRequestMessage)deserializedMessage).specs.path[2].x);
            Assert.AreEqual(3, ((MovementRequestMessage)deserializedMessage).specs.path[2].y);
        }


        /// <summary>
        /// This Testcase validates the deserialization of the Message PauseGameRequestMessage
        /// </summary>
        [Test]
        public void TestToPauseGameRequestMessage()
        {            
            string serializedMessage = "{\"type\":\"PAUSE_REQUEST\",\"version\":\"1.0\",\"pause\":true}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("PAUSE_REQUEST", ((PauseGameRequestMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((PauseGameRequestMessage)deserializedMessage).version);
            Assert.AreEqual(true, ((PauseGameRequestMessage)deserializedMessage).pause);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message PauseGameRequestMessage
        /// </summary>
        [Test]
        public void TestToGamePauseDemandMessage()
        {
            string serializedMessage = "{\"type\":\"GAME_PAUSE_DEMAND\",\"version\":\"0.1\",\"requestedByClientID\":1234,\"pause\": true}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("GAME_PAUSE_DEMAND", ((GamePauseDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((GamePauseDemandMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((GamePauseDemandMessage)deserializedMessage).requestedByClientID);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message SandwormDespawnDemandMessage
        /// </summary>
        [Test]
        public void TestToSandwormDespawnDemandMessage()
        {
            string serializedMessage = "{\"type\":\"SANDWORM_DESPAWN_DEMAND\",\"version\":\"1.0\"}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("SANDWORM_DESPAWN_DEMAND", ((SandwormDespawnDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((SandwormDespawnDemandMessage)deserializedMessage).version);
        }


        /// <summary>
        /// This Testcase validates the deserialization of the Message SandwormMoveDemandMessage
        /// </summary>
        [Test]
        public void TestToSandwormMoveDemandMessage()
        {
            string serializedMessage = "{\"type\":\"SANDWORM_DESPAWN_DEMAND\",\"version\":\"1.0\"}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("SANDWORM_DESPAWN_DEMAND", ((SandwormDespawnDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((SandwormDespawnDemandMessage)deserializedMessage).version);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message SandwormSpawnDemandMessage
        /// </summary>
        [Test]
        public void TestToSandwormSpawnDemandMessage()
        {
            string serializedMessage = "{\"type\":\"SANDWORM_SPAWN_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":1236,\"position\":{\"x\":2,\"y\":3}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("SANDWORM_SPAWN_DEMAND", ((SandwormSpawnDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((SandwormSpawnDemandMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((SandwormSpawnDemandMessage)deserializedMessage).clientID);
            Assert.AreEqual(1236, ((SandwormSpawnDemandMessage)deserializedMessage).characterID);
            Assert.AreEqual(2, ((SandwormSpawnDemandMessage)deserializedMessage).position.x);
            Assert.AreEqual(3, ((SandwormSpawnDemandMessage)deserializedMessage).position.y);
        }

        /// <summary>
        /// This Testcase validates the deserialization of the Message SpawnCharacterDemandMessage
        /// </summary>
        [Test]
        public void TestToSpawnCharacterDemandMessage()
        {
            string serializedMessage = "{\"type\":\"SPAWN_CHARACTER_DEMAND\",\"version\":\"1.0\",\"clientID\":1234,\"characterID\":12,\"characterName\":\"Vorname Nachname\",\"position\":{\"x\":0,\"y\":1},\"attributes\":{\"characterType\":\"FIGHTER\",\"healthMax\":100,\"healthCurrent\":75,\"healingHP\":10,\"MPmax\":3,\"MPcurrent\":1,\"APmax\":4,\"APcurrent\":2,\"attackDamage\":10,\"inventorySize\":5,\"inventoryUsed\":3,\"killedBySandworm\":false,\"isLoud\":true}}";
            Message deserializedMessage = MessageConverter.ToMessage(serializedMessage);

            Assert.AreEqual("SPAWN_CHARACTER_DEMAND", ((SpawnCharacterDemandMessage)deserializedMessage).GetMessageTypeAsString());
            Assert.AreEqual("1.0", ((SpawnCharacterDemandMessage)deserializedMessage).version);
            Assert.AreEqual(1234, ((SpawnCharacterDemandMessage)deserializedMessage).clientID);
            Assert.AreEqual(12, ((SpawnCharacterDemandMessage)deserializedMessage).characterID);
        }

        // SandwormSpawnDemandMessage

        // SpawnCharacterDemandMessage

        // StrikeMessage

        // TurnDemandMessage

        // TurnRequestMessage

        // RejoinMessage

        // AtomicsUpdateDemandMessage

    }
}