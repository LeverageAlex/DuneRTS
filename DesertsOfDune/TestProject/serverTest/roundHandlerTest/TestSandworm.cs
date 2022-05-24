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

        [Test]
        public void TestExecute()
        {

            MapField[,] mapFields = new MapField[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mapFields[i, j] = new FlatSand(false, false, null);
                }
            }
            SandWorm sandWorm = SandWorm.Spawn(10,0, mapFields);
            List<Character> loudCharacters = new List<Character>();
            Nobel nobel1 = new Nobel(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true);
            MapField mapField = new MapField(false, false, 0, null);
            mapField.XCoordinate = 0;
            mapField.ZCoordinate = 2;
            nobel1.CurrentMapfield = mapField;
           /* Nobel nobel2 = new Nobel(1, 2, 3, 4, 5, 6, 7, 8, 9, 4, false, true);
            MapField mapField2 = new MapField(false, false, 0, null);
            mapField.XCoordinate = 1;
            mapField.ZCoordinate = 2;
            nobel1.CurrentMapfield = mapField2; */
            loudCharacters.Add(nobel1);
            sandWorm.Execute(mapFields, loudCharacters);
        }
    }
}
