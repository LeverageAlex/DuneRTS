using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.network.util.world.character;
using GameData.Configuration;
using GameData.network.util.world;
using GameData.network.messages;

namespace UnitTestSuite.networkTest.utilTest.worldTest.characterTest
{
    /// <summary>
    /// This Class is used to tests the methods of the class Character
    /// </summary>
    public class TestFighter : Setup
    {


        [SetUp]
        public void Setup()
        {
            ConfigurationSetUp();
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SwordSpin with one character in range
        /// </summary>
        [Test]
        public void TestSwordSpinOneCharacterNear()
        {
            Fighter fighter = new Fighter("fighter");
            fighter.greatHouse = new Ordos();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            fighter.CurrentMapfield = map.fields[2, 2];
            map.fields[1, 2].Character = new BeneGesserit("");
            map.fields[1, 2].Character.greatHouse = new Harkonnen();
            map.fields[1, 2].Character.CurrentMapfield = map.fields[1, 2];
            map.PositionOfEyeOfStorm = new Position(4, 4);
            Assert.AreEqual(map.fields[1, 2].Character.healthMax, map.fields[1, 2].Character.healthCurrent);
            Assert.AreEqual(fighter.APmax, fighter.APcurrent);
            fighter.SwordSpin(map);
            Assert.AreEqual(16, map.fields[1, 2].Character.healthCurrent);
            Assert.AreEqual(0, fighter.APcurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SwordSpin with no characters in range
        /// </summary>
        [Test]
        public void TestSwordSpinNoCharacterNear()
        {
            Fighter fighter = new Fighter("fighter");
            fighter.greatHouse = new Ordos();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            fighter.CurrentMapfield = map.fields[2, 2];
            map.PositionOfEyeOfStorm = new Position(4, 4);
            Assert.AreEqual(fighter.APmax, fighter.APcurrent);
            fighter.SwordSpin(map);
            Assert.AreEqual(fighter.APmax, fighter.APcurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SwordSpin with one team character in range
        /// </summary>
        [Test]
        public void TestSwordSpinOneTeamCharacter()
        {
            Fighter fighter = new Fighter("fighter");
            fighter.greatHouse = new Ordos();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            fighter.CurrentMapfield = map.fields[2, 2];
            map.fields[1, 2].Character = new BeneGesserit("");
            map.fields[1, 2].Character.greatHouse = fighter.greatHouse;
            map.fields[1, 2].Character.CurrentMapfield = map.fields[1, 2];
            map.PositionOfEyeOfStorm = new Position(4, 4);
            Assert.AreEqual(map.fields[1, 2].Character.healthMax, map.fields[1, 2].Character.healthCurrent);
            Assert.AreEqual(fighter.APmax, fighter.APcurrent);
            fighter.SwordSpin(map);
            Assert.AreEqual(map.fields[1, 2].Character.healthMax, map.fields[1, 2].Character.healthCurrent);
            Assert.AreEqual(fighter.APmax, fighter.APcurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method ResetData
        /// </summary>
        [Test]
        public void TestResetData()
        {
            Fighter fighter = new Fighter("fighter");
            fighter.characterType = Enum.GetName(typeof(CharacterType), CharacterType.BENE_GESSERIT);
            fighter.healthMax = 0;
            fighter.healthCurrent = 0;
            fighter.HealingHP = 0;
            fighter.MPmax = 0;
            fighter.MPcurrent = 0;
            fighter.APmax = 0;
            fighter.APcurrent = 0;
            fighter.attackDamage = 0;
            fighter.inventorySize = 0;
            fighter.inventoryUsed = 0;
            fighter.killedBySandworm = false;
            fighter.isLoud = false;
            fighter.ResetData();
            Assert.AreEqual(Enum.GetName(typeof(CharacterType), CharacterType.FIGHTER),fighter.characterType);
            Assert.AreEqual(CharacterConfiguration.Fighter.maxHP, fighter.healthMax);
            Assert.AreEqual(CharacterConfiguration.Fighter.maxHP, fighter.healthCurrent);
            Assert.AreEqual(CharacterConfiguration.Fighter.healingHP, fighter.HealingHP);
            Assert.AreEqual(CharacterConfiguration.Fighter.maxMP, fighter.MPmax);
            Assert.AreEqual(CharacterConfiguration.Fighter.maxMP, fighter.MPcurrent);
            Assert.AreEqual(CharacterConfiguration.Fighter.maxAP, fighter.APmax);
            Assert.AreEqual(CharacterConfiguration.Fighter.maxAP, fighter.APcurrent);
            Assert.AreEqual(CharacterConfiguration.Fighter.damage, fighter.attackDamage);
            Assert.AreEqual(CharacterConfiguration.Fighter.inventorySize, fighter.inventorySize);
            Assert.AreEqual(0, fighter.inventoryUsed);
            Assert.AreEqual(0, fighter.inventoryUsed);
            Assert.AreEqual(false, fighter.killedBySandworm);
            Assert.AreEqual(false, fighter.isLoud);
        }
    }
}
