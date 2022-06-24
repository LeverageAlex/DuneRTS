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
using GameData.Configuration;
using System.Reflection;
using GameData;
using Server.ClientManagement.Clients;
using GameData.network.util.enums;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class Sandworm
    /// </summary>
    public class TestSandworm : Setup
    {
        private RoundHandler roundHandler;
        private Random rnd;
        private Map map;
        private List<Character> characters;

        private Sandworm worm;
        private ClientForTests testPlayer1, testPlayer2;

        [SetUp]
        public void Setup()
        {
            base.NetworkAndConfigurationSetUp();

            map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            roundHandler = new RoundHandler(PartyConfiguration.GetInstance().numbOfRounds, PartyConfiguration.GetInstance().spiceMinimum, map);

            rnd = new Random();

            testPlayer1 = new ClientForTests(new List<Character>());
            Party.GetInstance().AddClient(testPlayer1);

            testPlayer2 = new ClientForTests(new List<Character>());
            Party.GetInstance().AddClient(testPlayer2);

            //Characters
            characters = new List<Character>();
            for (int i = 0; i < 10; i++)
            {
                MapField field = map.GetRandomFieldWithoutCharacter();
                switch (rnd.Next(4))
                {
                    case 0: 
                        field.PlaceCharacter(new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true));
                        break;
                    case 1:
                        field.PlaceCharacter(new Mentat(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true));
                        break;
                    case 2:
                        field.PlaceCharacter(new BeneGesserit(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true));
                        break;
                    default:
                        field.PlaceCharacter(new Fighter(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true));
                        break;
                }
                
                if(i % 2 == 0)
                {
                    testPlayer1.UsedGreatHouse.Characters.Add(field.Character);
                }
                else
                {
                    testPlayer2.UsedGreatHouse.Characters.Add(field.Character);
                }

                characters.Add(field.Character);
            }

           
        }

        /// <summary>
        /// test sandworm spwan 
        /// and indirectly ChooseTargetCharacter, DetermineField
        /// </summary>
        [Test]
        public void TestSpawnSandworm()
        {
            worm = Sandworm.Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, map, characters, Party.GetInstance().messageController);
            Assert.NotNull(worm);
            Assert.NotNull(worm.GetCurrentField());
            Assert.NotNull(worm.GetTarget());
            Assert.False(worm.GetCurrentField().IsApproachable);

            foreach(Character c in characters){
                c.SetSilent();
            }
            Character loudChar = characters[rnd.Next(characters.Count)];
            loudChar.SetLoud();
            Sandworm.Despawn(Party.GetInstance().messageController);

            worm = Sandworm.Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, map, characters, Party.GetInstance().messageController);

            Assert.True(loudChar == worm.GetTarget());
        }

        [Test]
        public void TestDespawn()
        {
            worm = Sandworm.Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, map, map.GetCharactersOnMap(), Party.GetInstance().messageController);
            Assume.That(worm != null);

            Sandworm.Despawn(Party.GetInstance().messageController);

            Assert.Null(Sandworm.GetSandworm());
        }

        /// <summary>
        /// tests moveSandworm
        /// </summary>
        [Test]
        public void TestMoveSandworm()
        {
            worm = Sandworm.Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, map, map.GetCharactersOnMap(), Party.GetInstance().messageController);
            Assume.That(worm != null);

            Character target = worm.GetTarget();
            if(target.CurrentMapfield.tileType.Equals(TileType.PLATEAU.ToString()))
            {
                worm.MoveSandWorm(null);
                Assert.Null(Sandworm.GetSandworm());
            } 
            else
            {
                Queue<MapField> path = Sandworm.GetSandworm().CalculatePathToTarget();
                List<MapField> path2 = new List<MapField>(path);

                if(path2.Count > PartyConfiguration.GetInstance().sandWormSpeed)
                {
                    int count = path2.Count - 1;
                    Assert.True(count - PartyConfiguration.GetInstance().sandWormSpeed == path2.Count || Sandworm.GetSandworm() == null);
                }
            }
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

        /// <summary>
        /// test wheather sandworm lands on the given field
        /// </summary>
        [Test]
        public void TestMoveSandWormByOneField()
        {
            worm = Sandworm.Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, map, map.GetCharactersOnMap(), Party.GetInstance().messageController);
            Assume.That(worm != null);

            MapField current = worm.GetCurrentField();
            MapField[] neighbors = map.GetNeighborFields(current).ToArray();
            MapField next = neighbors[rnd.Next(neighbors.Length)];

            worm.MoveSandwormByOneField(next);

            Assert.True(worm.GetCurrentField() == next);
            
        }
    }
}
