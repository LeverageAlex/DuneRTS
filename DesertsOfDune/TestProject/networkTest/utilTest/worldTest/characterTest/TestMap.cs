using GameData.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using GameData.network.util.world.character;

namespace UnitTestSuite.networkTest.utilTest.worldTest.characterTest
{
    /// <summary>
    /// This class tests the Map.
    /// </summary>
    public class TestMap
    {

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
        }

        /// <summary>
        /// This Test validates the behaviour of the method getNeighborsFilds in case of a corner MapField
        /// </summary>
        [Test]
        public void TestGetNeighborFieldsCorner()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            List<MapField> fields = map.GetNeighborFields(map.fields[0, 0]);
            Assert.AreEqual(3, fields.Count);
            Assert.AreEqual(0, fields[0].XCoordinate);
            Assert.AreEqual(1, fields[0].ZCoordinate);
            Assert.AreEqual(1, fields[1].XCoordinate);
            Assert.AreEqual(0, fields[1].ZCoordinate);
            Assert.AreEqual(1, fields[2].XCoordinate);
            Assert.AreEqual(1, fields[2].ZCoordinate);
        }

        /// <summary>
        /// This Test validates the behaviour of the method getNeighborsFilds in case of a edge MapField
        /// </summary>
        [Test]
        public void TestGetNeighborFieldsEdge()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            List<MapField> fields = map.GetNeighborFields(map.fields[3, 0]);
            Assert.AreEqual(5, fields.Count);
            Assert.AreEqual(0, fields[0].XCoordinate);
            Assert.AreEqual(2, fields[0].ZCoordinate);
            Assert.AreEqual(0, fields[1].XCoordinate);
            Assert.AreEqual(4, fields[1].ZCoordinate);
            Assert.AreEqual(1, fields[2].XCoordinate);
            Assert.AreEqual(2, fields[2].ZCoordinate);
            Assert.AreEqual(1, fields[3].XCoordinate);
            Assert.AreEqual(3, fields[3].ZCoordinate);
            Assert.AreEqual(1, fields[4].XCoordinate);
            Assert.AreEqual(4, fields[4].ZCoordinate);
        }

        /// <summary>
        /// This Test validates the behaviour of the method getNeighborsFilds in case of a in Map field
        /// </summary>
        [Test]
        public void TestGetNeighborFields()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            List<MapField> fields = map.GetNeighborFields(map.fields[3, 3]);
            Assert.AreEqual(8, fields.Count);
            Assert.AreEqual(2, fields[0].XCoordinate);
            Assert.AreEqual(2, fields[0].ZCoordinate);
            Assert.AreEqual(2, fields[1].XCoordinate);
            Assert.AreEqual(3, fields[1].ZCoordinate);
            Assert.AreEqual(2, fields[2].XCoordinate);
            Assert.AreEqual(4, fields[2].ZCoordinate);
            Assert.AreEqual(3, fields[3].XCoordinate);
            Assert.AreEqual(2, fields[3].ZCoordinate);
            Assert.AreEqual(3, fields[4].XCoordinate);
            Assert.AreEqual(4, fields[4].ZCoordinate);
            Assert.AreEqual(4, fields[5].XCoordinate);
            Assert.AreEqual(2, fields[5].ZCoordinate);
            Assert.AreEqual(4, fields[6].XCoordinate);
            Assert.AreEqual(3, fields[6].ZCoordinate);
            Assert.AreEqual(4, fields[7].XCoordinate);
            Assert.AreEqual(4, fields[7].ZCoordinate);
        }


        /// <summary>
        /// This Test validates the behaviour of the method getNeighborsFilds in case of a in Map field
        /// </summary>
        [Test]
        public void TestGetRandomApproachableNeighborField()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            List<MapField> fields = map.GetNeighborFields(map.fields[3, 3]);
            Assert.AreEqual(8, fields.Count);
            Assert.AreEqual(2, fields[0].XCoordinate);
            Assert.AreEqual(2, fields[0].ZCoordinate);
            Assert.AreEqual(2, fields[1].XCoordinate);
            Assert.AreEqual(3, fields[1].ZCoordinate);
            Assert.AreEqual(2, fields[2].XCoordinate);
            Assert.AreEqual(4, fields[2].ZCoordinate);
            Assert.AreEqual(3, fields[3].XCoordinate);
            Assert.AreEqual(2, fields[3].ZCoordinate);
            Assert.AreEqual(3, fields[4].XCoordinate);
            Assert.AreEqual(4, fields[4].ZCoordinate);
            Assert.AreEqual(4, fields[5].XCoordinate);
            Assert.AreEqual(2, fields[5].ZCoordinate);
            Assert.AreEqual(4, fields[6].XCoordinate);
            Assert.AreEqual(3, fields[6].ZCoordinate);
            Assert.AreEqual(4, fields[7].XCoordinate);
            Assert.AreEqual(4, fields[7].ZCoordinate);
        }

        /// <summary>
        /// This Test validates the behaviour of the method GetRandomFreeApproachableNeighborField
        /// </summary>
        [Test]
        public void TestGetRandomFreeApproachableNeighborField()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            MapField field = map.GetRandomFreeApproachableNeighborField(map.fields[0,0]);
            Assert.True(field.XCoordinate >= 0 && field.ZCoordinate >= 0);
            Assert.True(field.XCoordinate < 2 && field.ZCoordinate < 2);
        }

        /// <summary>
        /// This Test validates the behaviour of the method GetRandomFreeApproachableNeighborField
        /// </summary>
        [Test]
        public void TestGetMapFieldAtPosition()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            for (int i = 0; i < ScenarioConfiguration.SCENARIO_WIDTH; i++)
            {
                for (int j = 0; j < ScenarioConfiguration.SCENARIO_HEIGHT; j++)
                {
                    MapField mapField =  map.GetMapFieldAtPosition(i, j);
                    Assert.True(mapField.XCoordinate == i);
                    Assert.True(mapField.ZCoordinate == j);
                }
            }
        }

        /// <summary>
        /// This Test validates the behaviour of the method SetMapFieldAtPosition
        /// </summary>
        [Test]
        public void TestSetMapFieldAtPositionWithPossiblePosition()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            bool worked = map.SetMapFieldAtPosition(map.fields[0,0],1,2);
            Assert.True(worked);
            Assert.AreEqual(1, map.fields[0, 0].XCoordinate);
            Assert.AreEqual(2, map.fields[0, 0].ZCoordinate);
        }

        /// <summary>
        /// This Test validates the behaviour of the method SetMapFieldAtPosition
        /// </summary>
        [Test]
        public void TestSetMapFieldAtPositionWithImpossiblePosition()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            bool worked = map.SetMapFieldAtPosition(map.fields[0, 0], 6, 6);
            Assert.False(worked);
            Assert.AreEqual(0, map.fields[0, 0].XCoordinate);
            Assert.AreEqual(0, map.fields[0, 0].ZCoordinate);
        }

        /// <summary>
        /// This Test validates the behaviour of the method GetCitiesOnMap
        /// </summary>
        [Test]
        public void TestGetCitiesOnMap()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            List<City> list = map.GetCitiesOnMap();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(0, list[0].XCoordinate);
            Assert.AreEqual(0, list[0].ZCoordinate);
            Assert.AreEqual(4, list[1].XCoordinate);
            Assert.AreEqual(4, list[1].ZCoordinate);
        }

        /// <summary>
        /// This Test validates the behaviour of the method IsMapFieldADesertField
        /// </summary>
        [Test]
        public void TestIsMapFieldADesertField()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            Assert.False(map.IsMapFieldADesertField(map.fields[0, 0]));
            Assert.True(map.IsMapFieldADesertField(map.fields[0, 1]));
            Assert.False(map.IsMapFieldADesertField(map.fields[0, 2]));
            Assert.True(map.IsMapFieldADesertField(map.fields[0, 3]));
            Assert.True(map.IsMapFieldADesertField(map.fields[0, 4]));
            Assert.False(map.IsMapFieldADesertField(map.fields[1, 0]));
            Assert.True(map.IsMapFieldADesertField(map.fields[1, 1]));
            Assert.True(map.IsMapFieldADesertField(map.fields[1, 2]));
            Assert.True(map.IsMapFieldADesertField(map.fields[1, 3]));
            Assert.True(map.IsMapFieldADesertField(map.fields[1, 4]));
        }

        /// <summary>
        /// This Test validates the behaviour of the method GetRandomDesertField
        /// </summary>
        [Test]
        public void TestGetRandomDesertField()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            MapField field = map.GetRandomDesertField();
            Assert.LessOrEqual(field.XCoordinate, ScenarioConfiguration.SCENARIO_WIDTH -1);
            Assert.LessOrEqual(field.ZCoordinate, ScenarioConfiguration.SCENARIO_HEIGHT - 1);
            Assert.True(field.tileType.Equals("DUNE") || field.tileType.Equals("FLAT_SAND"));
        }

        /// <summary>
        /// This Test validates the behaviour of the method GetAmountOfSpiceOnMap
        /// </summary>
        [Test]
        public void TestGetAmountOfSpiceOnMap()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            int amount = map.GetAmountOfSpiceOnMap();
            Assert.AreEqual(amount, 0);
            map.fields[0, 0].hasSpice = true;
            map.fields[1, 0].hasSpice = true;
            amount = map.GetAmountOfSpiceOnMap();
            Assert.AreEqual(2, amount);
        }

        /// <summary>
        /// This Test validates the behaviour of the method GetCharactersOnMap
        /// </summary>
        [Test]
        public void TestGetCharactersOnMap()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            List<Character> characters = map.GetCharactersOnMap();
            Assert.AreEqual(0, characters.Count);
            map.fields[0, 0].Character = new Mentat("some name");
            characters = map.GetCharactersOnMap();
            Assert.AreEqual(1, characters.Count);
            Assert.True(characters[0].CharacterName.Equals("some name"));
        }

        [Test]
        public void TestHasSandstormOnPath()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            Assume.That(ScenarioConfiguration.SCENARIO_WIDTH >= 5);
            Assume.That(ScenarioConfiguration.SCENARIO_HEIGHT >= 5);
            //preparing sandstorm
            MapField stormField = map.GetMapFieldAtPosition(2, 2);
            stormField.isInSandstorm = true;
            map.GetNeighborFields(stormField).ForEach((x) => x.isInSandstorm = true);

            MapField start = map.GetMapFieldAtPosition(0, 0);

            Position target = new Position(4, 4);
            for (int i = 1; i < 5; i++)
            {
                target = new Position(i, 4);
                Assert.True(map.HasSandstormOnPath(start, target));

                target = new Position(4, i);
                Assert.True(map.HasSandstormOnPath(start, target));
            }

            target = new Position(0, 4);
            Assert.False(map.HasSandstormOnPath(start, target));

            target = new Position(4, 0);
            Assert.False(map.HasSandstormOnPath(start, target));


            start = map.GetMapFieldAtPosition(3, 4);

            for (int i = 0; i < 4; i++)
            {
                target = new Position(i, 0);
                Assert.True(map.HasSandstormOnPath(start, target));

                target = new Position(0, i);
                Assert.True(map.HasSandstormOnPath(start, target));
            }

            target = new Position(4, 4);
            Assert.False(map.HasSandstormOnPath(start, target));

            target = new Position(0, 4);
            Assert.False(map.HasSandstormOnPath(start, target));

            //cornerHit
            target = new Position(4, 3);
            Assert.False(map.HasSandstormOnPath(start, target));
        }

        [Test]
        public void TestGetSandstormFieldsOnMap()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            Assume.That(ScenarioConfiguration.SCENARIO_WIDTH >= 5);
            Assume.That(ScenarioConfiguration.SCENARIO_HEIGHT >= 5);
            //preparing sandstorm
            MapField stormField = map.GetMapFieldAtPosition(3, 3);
            List<MapField> stormFields = map.GetNeighborFields(stormField);
            stormFields.Add(stormField);
            foreach(MapField sf in stormFields)
            {
                sf.isInSandstorm = true;
            }

            List<MapField> result = map.GetSandstormFieldsOnMap();

            foreach (MapField f in map.fields)
            {
                if (stormFields.Contains(f))
                {
                    Assert.True(f.isInSandstorm);
                }
                else
                {
                    Assert.False(f.isInSandstorm);
                }
            }
        }
    }
}
