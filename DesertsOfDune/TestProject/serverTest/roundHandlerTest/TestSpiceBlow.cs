using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.gameObjects;
using GameData.network.util.world;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class SpiceBlow
    /// </summary>
    public class TestSpiceBlow
    {


        /// <summary>
        /// This Testcase validates the behaviour of the method ApplySpiceBlow
        /// </summary>
        [Test]
        public void TestApplySpiceBlow()
        {
           /* RoundHandler roundHandler = new RoundHandler(2, 3);
            roundHandler.CurrentSpice = 2;
            SpiceBlow spiceBlow = new SpiceBlow(roundHandler.Map);
            bool spiceBlowApplicable = spiceBlow.SpiceBlowIsApplicable(roundHandler.SpiceMinimum,roundHandler.CurrentSpice);
            Assert.IsTrue(spiceBlowApplicable); */
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method ChoosRandomMapFieldIndxX
        /// </summary>
        [Test]
        public void TestChoosRandomMapFieldIndxX()
        {
           /* RoundHandler roundHandler = new RoundHandler(2, 3);
            MapField[,] mapFields = new MapField[2, 2];
            mapFields[0, 0] = new MapField(false, false, 0, null);
            mapFields[1, 0] = new MapField(false, false, 0, null);
            mapFields[0, 1] = new MapField(false, false, 0, null);
            mapFields[1, 1] = new MapField(false, false, 0, null);
            roundHandler.Map = mapFields;

            SpiceBlow spiceBlow = new SpiceBlow(roundHandler.Map);
            int result = spiceBlow.ChoosRandomMapFieldIndexX();
            Assert.GreaterOrEqual(result, 0);
            Assert.LessOrEqual(result, 1); */
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method ChoosRandomMapFieldIndexZ
        /// </summary>
        [Test]
        public void TestChoosRandomMapFieldIndxZ()
        {
           /* RoundHandler roundHandler = new RoundHandler(2, 3);
            MapField[,] mapFields = new MapField[2, 2];
            mapFields[0, 0] = new MapField(false, false, 0, null);
            mapFields[1, 0] = new MapField(false, false, 0, null);
            mapFields[0, 1] = new MapField(false, false, 0, null);
            mapFields[1, 1] = new MapField(false, false, 0, null);
            roundHandler.Map = mapFields;

            SpiceBlow spiceBlow = new SpiceBlow(roundHandler.Map);
            int result = spiceBlow.ChoosRandomMapFieldIndexZ();
            Assert.GreaterOrEqual(result, 0);
            Assert.LessOrEqual(result, 1); */
        }

        [Test]
        public void TestplaceSpiceOnFields()
        {
           /* RoundHandler roundHandler = new RoundHandler(2, 3);
            MapField[,] mapFields = new MapField[3, 3];
            mapFields[0, 0] = new MapField(false, false, 0, null);
            mapFields[0, 1] = new MapField(false, false, 0, null);
            mapFields[0, 2] = new MapField(false, false, 0, null);
            mapFields[1, 0] = new MapField(false, false, 0, null);
            mapFields[1, 1] = new MapField(false, false, 0, null);
            mapFields[1, 2] = new MapField(false, false, 0, null);
            mapFields[2, 0] = new MapField(false, false, 0, null);
            mapFields[2, 1] = new MapField(false, false, 0, null);
            mapFields[2, 2] = new MapField(false, false, 0, null);
            roundHandler.Map = mapFields;
            SpiceBlow spiceBlow = new SpiceBlow(roundHandler.Map);
            spiceBlow.PlaceSpiceOnFields(0, 0);
            int counter = 0;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(mapFields[i,j].HasSpice == true)
                    {
                        counter++;
                    }
                }
            }
            Assert.GreaterOrEqual(counter, 3);
            Assert.LessOrEqual(counter, 6); */
        }


        /// <summary>
        /// This Testcase validates the behaviour of the method ChangeFieldAndNeighborsRandomly
        /// </summary>
        [Test]
        public void TestChangeFieldAndNeighborsRandomly()
        {
            /*RoundHandler roundHandler = new RoundHandler(2, 3);
            SpiceBlow spiceBlow = new SpiceBlow(roundHandler.Map);
            MapField[,] mapFields = new MapField[3, 3];
            mapFields[0, 0] = new MapField(false, false, 0, null);
            mapFields[0, 1] = new MapField(false, false, 0, null);
            mapFields[0, 2] = new MapField(false, false, 0, null);
            mapFields[1, 0] = new MapField(false, false, 0, null);
            mapFields[1, 1] = new MapField(false, false, 0, null);
            mapFields[1, 2] = new MapField(false, false, 0, null);
            mapFields[2, 0] = new MapField(false, false, 0, null);
            mapFields[2, 1] = new MapField(false, false, 0, null);
            mapFields[2, 2] = new MapField(false, false, 0, null);
            roundHandler.Map = mapFields;
            spiceBlow.ChangeFieldAndNeighborsRandomly(1, 1);

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    Assert.True(mapFields[i, j].TileType == "DUNE" || mapFields[i, j].TileType == "FLAT");
                }
            } */
        }
    }
}
