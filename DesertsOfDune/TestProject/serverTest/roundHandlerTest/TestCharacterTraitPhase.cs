using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData.gameObjects;
using GameData.network.util.world;
using GameData.network.util.world.greatHouse;
using NUnit.Framework;
using Server;
using Server.Configuration;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class CharacterTraitPhase
    /// </summary>
    public class TestCharacterTraitPhase
    {
        RoundHandler roundHandler = null;

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

            var map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            roundHandler = new RoundHandler(PartyConfiguration.GetInstance().numbOfRounds, PartyConfiguration.GetInstance().spiceMinimum, map);
        }

        [Test]
        public void TestExecute()
        {
            var characterTraitPhase = roundHandler.GetCharacterTraitPhase();
            var p1 = new HumanPlayer("client1", "session1");
            var p2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(p1);
            Party.GetInstance().AddClient(p2);
            p1.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            p2.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.ATREIDES);

            //TODO: implement test
        }
        /// <summary>
        /// This Testcase validates the behaviour of the method RandomizeTraitSequenze
        /// </summary>
        [Test]
        public void TestGenerateTraitSequenze()
        {
            var characterTraitPhase = roundHandler.GetCharacterTraitPhase();
            var p1 = new HumanPlayer("client1", "session1");
            var p2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(p1);
            Party.GetInstance().AddClient(p2);
            p1.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            p2.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.ATREIDES);

            var characters = new List<Character>();
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    characters.Add(character);
                }
            }
            var randomizedCharacters = characterTraitPhase.GenerateTraitSequenze();
            Assert.AreEqual(characters.Count, randomizedCharacters.Count);          //randomizedCharacter has exact the same number of elements as characters
            Assert.AreNotEqual(characters, randomizedCharacters);                   //randomizedCharacter is not just a copy of characters
            foreach (var character in characters) {
                Assert.IsTrue(randomizedCharacters.Contains(character));            //every character from characters is in randomizedCharacters
                int characterIndex = randomizedCharacters.IndexOf(character);
                randomizedCharacters.Remove(character);
                Assert.IsFalse(randomizedCharacters.Contains(character));           //no character from characters is duplicated in randomizedCharacters
                randomizedCharacters.Insert(characterIndex, character);
            }
            foreach(var character in randomizedCharacters)
            {
                Assert.IsTrue(!character.IsDead());                                 //test if every charcter in the initial generated trait sequence is alive
                Assert.IsTrue(!character.killedBySandworm);                         //character in initial generated trait sequence is also not killed by a sandworm
                Assert.IsTrue(characters.Contains(character));                      //every character in randomizedCharacters is from the characters list and not created new
            }
            var randomizedCharacters2 = characterTraitPhase.GenerateTraitSequenze();
            Assert.AreNotEqual(randomizedCharacters, randomizedCharacters2);        //tests if the GenerateTraitSequenz is really random (small odds that there are 2 equal lists if they were generated random)
            
            //TODO: implement test
        }

        [Test]
        public void TestSendRequestForNextCharacter()
        {
            //TODO: implement test
        }

        [Test]
        public void TestStopTimer()
        {
            //TODO: implement test
        }

        [Test]
        public void TestRequestClientForNextCharacterTrait()
        {
            //TODO: implement test
        }

        [Test]
        public void TestSetTimer()
        {
            //TODO: implement test
        }

        [Test]
        public void TestOnTimedEvent()
        {
            //TODO: implement test
        }
    }
}
