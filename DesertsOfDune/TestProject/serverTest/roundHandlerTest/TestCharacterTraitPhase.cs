using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData.gameObjects;
using GameData.network.util.world;
using GameData.network.util.world.greatHouse;
using NUnit.Framework;
using GameData;
using GameData.Configuration;

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
        }

        [Test]
        public void TestSendRequestForNextCharacter()
        {
            var characterTraitPhase = roundHandler.GetCharacterTraitPhase();
            var p1 = new HumanPlayer("client1", "session1");
            var p2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(p1);
            Party.GetInstance().AddClient(p2);
            p1.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            p2.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.ATREIDES);
            var randomizedCharacters = characterTraitPhase.GenerateTraitSequenze();
            characterTraitPhase.SendRequestForNextCharacter();
            foreach (var character in randomizedCharacters)
            {
                Assert.IsFalse(character.IsLoud());                                         //every Character gets set to silent at the beginning of the characterTraitPhase
                if(!character.IsDead() && !character.KilledBySandworm && !character.IsInSandStorm(Party.GetInstance().map))
                {
                    Assert.AreEqual(character.APmax, character.APcurrent);                  //if the character can do a turn, its AP gets resetted   
                    Assert.AreEqual(character.MPmax, character.MPcurrent);                  //if the character can do a turn, its MP gets resetted
                    characterTraitPhase.SendRequestForNextCharacter();                      //if the character can do a turn, the next request for the next character is sent when the character ends his turn; to test it we call it manually; if the character can't do a turn the request for the next character is sent automatically
                }
            }

            //TODO: implement test
        }

        [Test]
        public void TestStopTimer()
        {
            var characterTraitPhase = roundHandler.GetCharacterTraitPhase();
            characterTraitPhase.SetTimer();
            characterTraitPhase.StopTimer();
            Assert.AreEqual(false, characterTraitPhase.GetTimer().Enabled);
        }

        [Test]
        public void TestRequestClientForNextCharacterTrait()
        {
            var characterTraitPhase = roundHandler.GetCharacterTraitPhase();
            var p1 = new HumanPlayer("client1", "session1");
            var p2 = new HumanPlayer("client2", "session2");
            Party.GetInstance().AddClient(p1);
            Party.GetInstance().AddClient(p2);
            p1.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.CORRINO);
            p2.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(GameData.network.util.enums.GreatHouseType.ATREIDES);
            var randomizedCharacters = characterTraitPhase.GenerateTraitSequenze();
            foreach(var character in randomizedCharacters)
            {
                characterTraitPhase.RequestClientForNextCharacterTrait(character.CharacterId);                //test if RequestClientForNextCharacterTrait is successful for each character
                Assert.IsTrue(characterTraitPhase.GetTimer().Enabled);                                        //test if the timer starts, when RequestClientForNextCharacterTrait is executed
                characterTraitPhase.GetTimer().Stop();
            }

            //TODO: implement test
        }

        [Test]
        public void TestSetTimer()
        {
            var characterTraitPhase = roundHandler.GetCharacterTraitPhase();
            characterTraitPhase.SetTimer();
            Assert.IsNotNull(characterTraitPhase.GetTimer());
        }

        [Test]
        public void TestOnTimedEvent()
        {
            //TODO: implement test
        }

        [Test]
        public void TestFreezeTraitPhase()
        {
            var characterTraitPhase = roundHandler.GetCharacterTraitPhase();
            characterTraitPhase.SetTimer();
            characterTraitPhase.freezeTraitPhase(true);
            Assert.IsFalse(characterTraitPhase.GetTimer().Enabled);
            characterTraitPhase.freezeTraitPhase(false);
            Assert.IsTrue(characterTraitPhase.GetTimer().Enabled);
        }
    }
}
