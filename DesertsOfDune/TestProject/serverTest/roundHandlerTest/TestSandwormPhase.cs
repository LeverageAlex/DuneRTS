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
    /// This Class is used to Test the class SandwormPhase
    /// </summary>
    public class TestSandwormPhase : Setup
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

                if (i % 2 == 0)
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


        [Test]
        public void TestExecute()
        {
            foreach (Character c in characters)
            {
                c.SetSilent();
            }
            Sandworm.Despawn(Party.GetInstance().messageController);
            Assume.That(Sandworm.GetSandworm() == null);
            SandwormPhase sp = new SandwormPhase(map);

            sp.Execute();// Expect nothing

            Assert.Null(Sandworm.GetSandworm());

            Character loudChar = characters[rnd.Next(characters.Count)];
            loudChar.SetLoud();
            if (loudChar.CurrentMapfield.tileType.Equals(TileType.PLATEAU.ToString()))
            {
                var oldfield = loudChar.CurrentMapfield;
                MapField field = new FlatSand(oldfield.hasSpice, oldfield.isInSandstorm);
                field.PlaceCharacter(loudChar);

                map.SetMapFieldAtPosition(field, oldfield.XCoordinate, oldfield.ZCoordinate);
                field.IsApproachable = true;
            }

            sp.Execute();// Expect Sandworm creation

            worm = Sandworm.GetSandworm();

            Assert.NotNull(worm);
            Assert.True(worm.GetTarget() == loudChar);

            float dist = MathF.Sqrt(MathF.Pow((worm.GetCurrentField().XCoordinate - loudChar.CurrentMapfield.XCoordinate), 2f) 
                + MathF.Pow((worm.GetCurrentField().ZCoordinate - loudChar.CurrentMapfield.ZCoordinate), 2f));
            int rounds = (int) MathF.Ceiling(dist/PartyConfiguration.GetInstance().sandWormSpeed);
            for(int i = 0; i < rounds; i++)
            {
                if (Sandworm.GetSandworm() == null) break;
                sp.Execute();
            }

            Assert.True(loudChar.KilledBySandworm);
            Assert.Null(Sandworm.GetSandworm());
        }

        [Test]
        public void CheckLoudness()
        {
            SandwormPhase sp = new SandwormPhase(map);

            foreach (Character c in characters)
            {
                c.SetSilent();
            }
            Assert.False(sp.CheckLoudness(characters));

            Character loudChar = characters[rnd.Next(characters.Count)];
            loudChar.SetLoud();

            Assert.True(sp.CheckLoudness(characters));
        }
        
    }
}
