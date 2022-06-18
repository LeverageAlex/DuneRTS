using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using GameData.network.util.world.character;
using GameData.gameObjects;
using Server.Configuration;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class Sandworm
    /// </summary>
    public class TestSandworm
    {
        private RoundHandler roundHandler;
        private Random rnd;
        private List<Character> rndCharacters;
        private Map map;

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

            map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            roundHandler = new RoundHandler(PartyConfiguration.GetInstance().numbOfRounds, PartyConfiguration.GetInstance().spiceMinimum, map);

            rnd = new Random();

            rndCharacters = new List<Character>();
            for (int i = 0; i < 10; i++)
            {
                switch (rnd.Next(4))
                {
                    case 0: 
                        rndCharacters.Add(new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, rnd.Next(1) == 0)); 
                        break;
                    case 1:
                        rndCharacters.Add(new Mentat(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, rnd.Next(1) == 0));
                        break;
                    case 2:
                        rndCharacters.Add(new BeneGesserit(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, rnd.Next(1) == 0));
                        break;
                    default:
                        rndCharacters.Add(new Fighter(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, rnd.Next(1) == 0));
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestExecuteWithSandwormInstance()
        {
           /* MapField[,] mapFields = new MapField[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mapFields[i, j] = new FlatSand(false, false, null);
                }
            }
            List<Character> characters = new List<Character>();
            Noble nobel1 = new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true);
            MapField mapField = new MapField(false, false, 0, null);
            mapField.XCoordinate = 0;
            mapField.ZCoordinate = 2;
            nobel1.CurrentMapfield = mapField;
            characters.Add(nobel1);
            Sandworm sandWorm = new Sandworm();
            sandWorm = sandWorm.Execute(mapFields, characters);
            sandWorm.Execute(mapFields, characters); */
        }

        [Test]
        public void TestExecuteWithOutSandwormInstance()
        {
            MapField[,] mapFields = new MapField[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mapFields[i, j] = new FlatSand(false, false);
                }
            }
            List<Character> characters = new List<Character>();
            Noble nobel1 = new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true);
            MapField mapField = new City(1234, false, false);
                //new MapField(GameData.network.util.enums.TileType.CITY, GameData.network.util.enums.Elevation.low,false, false, 0, null);
            mapField.XCoordinate = 0;
            mapField.ZCoordinate = 2;
            nobel1.CurrentMapfield = mapField;
            characters.Add(nobel1);
            Sandworm sandWorm = new Sandworm();
            //sandWorm.Execute(mapFields, characters);
        }

        [Test]
        public void TestChooseTargetCharacter()
        {
            List<Character> characters = new List<Character>();
            Noble nobel1 = new Noble(1, 2, 4, 4, 5, 6, 7, 8, 9, 4, false, true);
            Noble nobel2 = new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true);
            Noble nobel3 = new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, false);

            Sandworm sandWorm = new Sandworm();
            Character target = sandWorm.ChooseTargetCharacter(characters);
            Assert.IsNull(target);

            characters.Add(nobel1);
            target = sandWorm.ChooseTargetCharacter(characters);
            Assert.AreEqual(target, nobel1);

            characters.Add(nobel2);
            target = sandWorm.ChooseTargetCharacter(characters);
            Assert.True((target == nobel1 || target == nobel2));
            
            characters.Add(nobel3);
            target = sandWorm.ChooseTargetCharacter(characters);
            Assert.False(target == nobel3);
        }

        [Test]
        public void TestMoveSandWormByOneField()
        {
            //Sandworm sandWorm = Sandworm.Spawn(10, 1, map, rndCharacters, null);


            // TODO: implement test

        }
    }
}
