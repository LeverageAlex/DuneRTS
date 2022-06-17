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

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class Sandworm
    /// </summary>
    public class TestSandworm
    {
        [SetUp]
        public void Setup()
        {
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
            Noble nobel1 = new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true);
            Noble nobel2 = new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true);
            Noble nobel3 = new Noble(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, false);
            Sandworm sandWorm = Sandworm.Spawn(10, 0, null, null, null);
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
            MapField[,] mapFields = new MapField[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mapFields[i, j] = new FlatSand(false, false);
                }
            }
           // Sandworm sandWorm = Sandworm.Spawn(10, 0, mapFields, null);

            // TODO: implement test

        }
    }
}
