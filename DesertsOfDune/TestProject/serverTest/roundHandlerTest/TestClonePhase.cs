using System;
using System.Collections.Generic;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.network.util.world.character;
using GameData.Configuration;
using GameData.network.util.world;
using GameData.gameObjects;
using GameData;
using GameData.network.util.world.mapField;
using Server.ClientManagement.Clients;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class ClonePhase
    /// </summary>
    public class TestClonePhase : Setup
    {
        private Map map;
        private ClientForTests testPlayer1, testPlayer2;
        private List<Character> chars = new List<Character>();

        [SetUp]
        public void Setup()
        {
            base.NetworkAndConfigurationSetUp();

            map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);

            Random rnd = new Random();

            testPlayer1 = new ClientForTests(new List<Character>());
            Party.GetInstance().AddClient(testPlayer1);

            testPlayer2 = new ClientForTests(new List<Character>());
            Party.GetInstance().AddClient(testPlayer2);

            //make sure the maxHP is 5
            CharacterConfiguration.Noble.maxHP = 5;
            CharacterConfiguration.Mentat.maxMP = 5;
            CharacterConfiguration.BeneGesserit.maxHP = 5;
            CharacterConfiguration.Fighter.maxMP = 5;


            //add characters with currentHP = 0 and maxHP = 5
            for (int i = 0; i < 6; i++)
            {
                MapField field = map.GetRandomFieldWithoutCharacter();
                switch (rnd.Next(4))
                {
                    case 0:
                        field.PlaceCharacter(new Noble(5, 0, 3, 4, 5, 6, 7, 8, 9, 4, false, false));
                        break;
                    case 1:
                        field.PlaceCharacter(new Mentat(5, 0, 3, 4, 5, 6, 7, 8, 9, 4, false, false));
                        break;
                    case 2:
                        field.PlaceCharacter(new BeneGesserit(5, 0, 3, 4, 5, 6, 7, 8, 9, 4, false, false));
                        break;
                    default:
                        field.PlaceCharacter(new Fighter(5, 0, 3, 4, 5, 6, 7, 8, 9, 4, false, false));
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
                
                chars.Add(field.Character);
            }

        }


        /// <summary>
        /// This Testcase validates the behaviour of the method Execute
        /// </summary>
        [Test]
        public void TestExecute()
        {
            //set city tiles
            City[] cities = map.GetCitiesOnMap().ToArray();
            Assume.That(cities.Length >= 2);
            testPlayer1.UsedGreatHouse.City = cities[0];
            testPlayer2.UsedGreatHouse.City = cities[1];

            List<MapField> neighbors0 = map.GetNeighborFields(cities[0]);
            List<MapField> neighbors1 = map.GetNeighborFields(cities[1]);

            neighbors0.RemoveAll(neighbor => !neighbor.IsApproachable || neighbor.IsCharacterStayingOnThisField);
            neighbors1.RemoveAll(neighbor => !neighbor.IsApproachable || neighbor.IsCharacterStayingOnThisField);

            int possibleCloneCount = neighbors0.Count + neighbors1.Count;

            ClonePhase clonePhase = new ClonePhase(map, 2d);//defenetly over 100% cloning rate

            foreach(Character c in chars)
            {
                Assume.That(c.IsDead());
            }
            
            clonePhase.Execute();

            int count = 0;

            foreach (Character c in testPlayer1.UsedGreatHouse.Characters)
            {
                if (!c.IsDead()) count++;
            }

            foreach (Character c in testPlayer2.UsedGreatHouse.Characters)
            {
                if (!c.IsDead()) count++;
            }

            Assert.True(count == possibleCloneCount);
        }

    }
}
