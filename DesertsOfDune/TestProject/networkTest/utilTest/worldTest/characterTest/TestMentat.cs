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
    /// This Class is used to tests the methods of the class Mentat
    /// </summary>
    public class TestMentat : Setup
    {
        [SetUp]
        public void Setup()
        {
            ConfigurationSetUp();
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpiceHoarding on spice in range
        /// </summary>
        [Test]
        public void TestSpiceHoarding()
        {
            Mentat mentat = new Mentat("");
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            mentat.CurrentMapfield = map.fields[0, 1];
            map.fields[0, 2].hasSpice = true;
            Assert.True(map.fields[0, 2].hasSpice);
            Assert.AreEqual(0, mentat.inventoryUsed);
            bool actionPossible = mentat.SpiceHoarding(map);
            Assert.AreEqual(1,mentat.inventoryUsed);
            Assert.False(map.fields[0, 2].hasSpice);
            Assert.True(actionPossible);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpiceHoarding no spice in range
        /// </summary>
        [Test]
        public void TestSpiceHoardingNoSpiceInRange()
        {
            Mentat mentat = new Mentat("");
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            mentat.CurrentMapfield = map.fields[0, 1];
            Assert.AreEqual(0, mentat.inventoryUsed);
            Assert.AreEqual(mentat.APmax, mentat.APcurrent);
            bool actionPossible = mentat.SpiceHoarding(map);
            Assert.AreEqual(0, mentat.inventoryUsed);
            Assert.AreEqual(0, mentat.APcurrent);
            Assert.True(actionPossible);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpiceHoarding no spice in range
        /// </summary>
        [Test]
        public void TestSpiceHoardingSpiceInRange()
        {
            Mentat mentat = new Mentat("");
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            mentat.CurrentMapfield = map.fields[0, 1];
            map.fields[0, 2].hasSpice = true;
            map.fields[1, 1].hasSpice = true;
            Assert.AreEqual(0, mentat.inventoryUsed);
            Assert.AreEqual(mentat.APmax, mentat.APcurrent);
            bool actionPossible = mentat.SpiceHoarding(map);
            Assert.AreEqual(2, mentat.inventoryUsed);
            Assert.AreEqual(0, mentat.APcurrent);
            Assert.True(actionPossible);
        }

        /// <summary>
        /// this testcase validates the behaviour ot the resetData method
        /// </summary>
        public void TestResetData()
        {
            Mentat fighter = new Mentat("fighter");
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
            Assert.AreEqual(Enum.GetName(typeof(CharacterType), CharacterType.BENE_GESSERIT), fighter.characterType);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxHP, fighter.healthMax);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxHP, fighter.healthCurrent);
            Assert.AreEqual(CharacterConfiguration.Mentat.healingHP, fighter.HealingHP);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxMP, fighter.MPmax);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxMP, fighter.MPcurrent);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxAP, fighter.APmax);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxAP, fighter.APcurrent);
            Assert.AreEqual(CharacterConfiguration.Mentat.damage, fighter.attackDamage);
            Assert.AreEqual(CharacterConfiguration.Mentat.inventorySize, fighter.inventorySize);
            Assert.AreEqual(0, fighter.inventoryUsed);
            Assert.AreEqual(0, fighter.inventoryUsed);
            Assert.AreEqual(false, fighter.killedBySandworm);
            Assert.AreEqual(false, fighter.isLoud);
        }
    }
}
