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
    /// This Class is used to tests the methods of the class Nobel
    /// </summary>
    public class TestNoble : Setup
    {

        [SetUp]
        public void Setup()
        {
            ConfigurationSetUp();
        }

        /// <summary>
        /// This Testcase validates the behavior of the method Kanly
        /// </summary>
        [Test]
        public void TestKanlyFalse()
        {
            Fighter n2 = new Fighter("");
            Noble noble = new Noble("");
            bool possible = noble.Kanly(n2);
            Assert.IsFalse(possible);  
        }

        /// <summary>
        /// This Testcase validates the behavior of the method Kanly
        /// </summary>
        [Test]
        public void TestKanly()
        {
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            Noble n2 = new Noble("");
            n2.CurrentMapfield = map.fields[0, 1];
            n2.greatHouse = new Harkonnen();
            Noble noble = new Noble("");
            noble.CurrentMapfield = map.fields[1, 1];
            n2.greatHouse = new Ordos();
            bool possible = noble.Kanly(n2);
            Assert.True(n2.healthMax == n2.healthCurrent || 0 == n2.healthCurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method AtomicBomb
        /// </summary>
        [Test]
        public void TestAtomicBomb()
        {
            Ordos ordos = new Ordos();
            Corrino corrino = new Corrino();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            MapField target = map.fields[0, 1];
            map.fields[1, 1].hasSpice = true;
            BeneGesserit bene = new BeneGesserit("");
            bene.CurrentMapfield = map.fields[0, 2];
            map.fields[0, 2].Character = bene;
            Noble noble = new Noble("");
            noble.greatHouse = ordos;
            Assert.AreEqual("MOUNTAINS", map.fields[1, 0].tileType);
            Assert.AreEqual("FLAT_SAND", map.fields[1, 1].tileType);
            noble.AtomicBomb(target, map, false, ordos, corrino);
            Assert.AreEqual("PLATEAU", map.fields[1, 0].tileType);
            Assert.AreEqual("FLAT_SAND", map.fields[1, 1].tileType);
            Assert.False(map.fields[1, 1].hasSpice);
            Assert.IsNull(map.fields[0, 2].Character);
            Assert.True(bene.IsDead());
        }

        /// <summary>
        /// This Testcase validates the behavior of the method ResetData
        /// </summary>
        [Test]
        public void TestResetData()
        {
            Noble noble = new Noble("");
            noble.characterType = Enum.GetName(typeof(CharacterType), CharacterType.BENE_GESSERIT);
            noble.healthMax = 0;
            noble.healthCurrent = 0;
            noble.HealingHP = 0;
            noble.MPmax = 0;
            noble.MPcurrent = 0;
            noble.APmax = 0;
            noble.APcurrent = 0;
            noble.attackDamage = 0;
            noble.inventorySize = 0;
            noble.inventoryUsed = 0;
            noble.killedBySandworm = false;
            noble.isLoud = false;
            noble.ResetData();
            Assert.AreEqual(Enum.GetName(typeof(CharacterType), CharacterType.NOBLE), noble.characterType);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxHP, noble.healthMax);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxHP, noble.healthCurrent);
            Assert.AreEqual(CharacterConfiguration.Mentat.healingHP, noble.HealingHP);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxMP, noble.MPmax);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxMP, noble.MPcurrent);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxAP, noble.APmax);
            Assert.AreEqual(CharacterConfiguration.Mentat.maxAP, noble.APcurrent);
            Assert.AreEqual(CharacterConfiguration.Mentat.damage, noble.attackDamage);
            Assert.AreEqual(CharacterConfiguration.Mentat.inventorySize, noble.inventorySize);
            Assert.AreEqual(0, noble.inventoryUsed);
            Assert.AreEqual(0, noble.inventoryUsed);
            Assert.AreEqual(false, noble.killedBySandworm);
            Assert.AreEqual(false, noble.isLoud);
            noble.ResetData();
        }
    }
}
