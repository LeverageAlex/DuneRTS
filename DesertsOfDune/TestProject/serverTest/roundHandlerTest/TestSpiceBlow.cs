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
        private List<List<string>> scenarioConfiguration;
        private int scenarioWidth = 10;
        private int scenarioHeight = 10;

        [SetUp]
        public void Setup()
        {
            scenarioConfiguration = new List<List<string>>();
            for (int i = 0; i < 10; i++)
            {
                List<string> l = new List<string>();
                for (int j = 0; j < 10; j++)
                {
                    l.Add("FLAT_SAND");
                }
                scenarioConfiguration.Add(l);
            }

        }

        /// <summary>
        /// This Testcase validates the behaviour of the method PlaceSpiceOnFields
        /// </summary>
        [Test]
        public void TestPlaceSpiceOnFieldsAllFieldsAproachable()
        {
            
            Map map = new Map(3, 3, scenarioConfiguration);
            foreach (MapField mapField in map.fields)
            {
                mapField.IsApproachable = true;
            }

            SpiceBlow spiceBlow = new SpiceBlow(map);
            MapField randomField = map.GetRandomDesertField();
            Assert.AreEqual(0, map.GetAmountOfSpiceOnMap());
            
            spiceBlow.PlaceSpiceOnFields(randomField);

            Assert.Greater(map.GetAmountOfSpiceOnMap(),2);
            Assert.LessOrEqual(map.GetAmountOfSpiceOnMap(),7);
        }

        /// <summary>
        /// This Testcase validates the behaviour of the method IsSpiceBlowNecessary
        /// </summary>
        [Test]
        public void TestIsSpiceBlowNecessary()
        {
            SpiceBlow spiceBlow = new SpiceBlow(null);
            Assert.False(spiceBlow.IsSpiceBlowNecessary(1,2));
            Assert.False(spiceBlow.IsSpiceBlowNecessary(2, 2));
            Assert.False(spiceBlow.IsSpiceBlowNecessary(2, 3));
        }


        /// <summary>
        /// This Testcase validates the behaviour of the method Execute
        /// </summary>
        [Test]
        public void TestExecute()
        {
            List<List<string>> scenarioConfiguration = new List<List<string>>();
            List<string> list1 = new List<string>();
            list1.Add("FLAT_SAND");
            list1.Add("FLAT_SAND");
            list1.Add("FLAT_SAND");
            List<string> list2 = new List<string>();
            list2.Add("FLAT_SAND");
            list2.Add("FLAT_SAND");
            list2.Add("FLAT_SAND");
            List<string> list3 = new List<string>();
            list3.Add("FLAT_SAND");
            list3.Add("FLAT_SAND");
            list3.Add("FLAT_SAND");
            scenarioConfiguration.Add(list1);
            scenarioConfiguration.Add(list2);
            scenarioConfiguration.Add(list3);
            Map map = new Map(3, 3, scenarioConfiguration);
            foreach (MapField mapField in map.fields)
            {
                mapField.IsApproachable = true;
            }

            SpiceBlow spiceBlow = new SpiceBlow(map);
            Assert.AreEqual(0, map.GetAmountOfSpiceOnMap());
            foreach (MapField mapField in map.fields)
            {
                Assert.AreEqual("FLAT_SAND", mapField.tileType);
            }

            spiceBlow.Execute();

            foreach (MapField mapField in map.fields)
            {
                Assert.True(mapField.tileType.Equals("FLAT_SAND") || mapField.tileType.Equals("DUNE"));
            }

            Assert.Greater(map.GetAmountOfSpiceOnMap(), 1);
            Assert.LessOrEqual(map.GetAmountOfSpiceOnMap(), 7);
        }
    }
}
