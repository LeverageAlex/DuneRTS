using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.network.util.world.character;
using GameData.Configuration;
using GameData.network.messages;
using GameData.network.util.world.mapField;
using GameData.network.util.world;

namespace UnitTestSuite.networkTest.utilTest.worldTest.characterTest
{
    /// <summary>
    /// This Class is used to tests the methods of the class BeneGesserit
    /// </summary>
    public class TestBeneGesserit : Setup
    {
        [SetUp]
        public void Setup()
        {
            ConfigurationSetUp();
        }

        /// <summary>
        /// This Testcase validates the behavior of the method Voice
        /// </summary>
        [Test]
        public void TestVoice()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            beneGesserit.greatHouse = new Corrino();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            beneGesserit.CurrentMapfield = map.fields[0, 1];
            Noble noble = new Noble("someName");
            noble.greatHouse = new Harkonnen();
            noble.CurrentMapfield = map.fields[1, 1];
            noble.inventoryUsed = 5;
            bool possible = beneGesserit.Voice(noble);
            Assert.IsTrue(possible);
            Assert.AreEqual(5, beneGesserit.inventoryUsed);
            Assert.AreEqual(0, noble.inventoryUsed);
            Assert.AreEqual(0, beneGesserit.APcurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method Voice
        /// </summary>
        [Test]
        public void TestVoiceNotPossible()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            beneGesserit.greatHouse = new Corrino();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            beneGesserit.CurrentMapfield = map.fields[0, 1];
            beneGesserit.APcurrent = 0;
            Noble noble = new Noble("someName");
            noble.greatHouse = new Harkonnen();
            noble.CurrentMapfield = map.fields[1, 1];
            noble.inventoryUsed = 5;
            bool possible = beneGesserit.Voice(noble);
            Assert.IsFalse(possible);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method ResetData
        /// </summary>
        [Test]
        public void TestResetData()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            beneGesserit.ResetData();
            Assert.AreEqual(CharacterConfiguration.BeneGesserit.maxHP, beneGesserit.healthCurrent);
            Assert.AreEqual(CharacterConfiguration.BeneGesserit.maxMP, beneGesserit.MPcurrent);
            Assert.AreEqual(0, beneGesserit.inventoryUsed);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method DecreaseHP
        /// </summary>
        [Test]
        public void TestDecreaseHP()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.AreEqual(CharacterConfiguration.BeneGesserit.maxHP, beneGesserit.healthCurrent);
            beneGesserit.DecreaseHP(10);
            Assert.AreEqual(CharacterConfiguration.BeneGesserit.maxHP-10, beneGesserit.healthCurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method IsDead
        /// </summary>
        [Test]
        public void TestIsDead()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.False(beneGesserit.IsDead());
            beneGesserit.healthCurrent = 0;
            Assert.True(beneGesserit.IsDead());
        }


        /// <summary>
        /// This Testcase validates the behavior of the method IsLoud
        /// </summary>
        [Test]
        public void TestIsloud()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.False(beneGesserit.IsLoud());
            beneGesserit.isLoud = true;
            Assert.True(beneGesserit.IsLoud());
        }

        /// <summary>
        /// This Testcase validates the behavior of the method CollectSpice
        /// </summary>
        [Test]
        public void TestCollectSpice()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            beneGesserit.CurrentMapfield = new Dune(true, false);
            bool collectPossible = beneGesserit.CollectSpice();
            Assert.IsTrue(collectPossible);
            Assert.AreEqual(beneGesserit.APmax-1,beneGesserit.APcurrent);
            Assert.False(beneGesserit.CurrentMapfield.hasSpice);
        }



        /// <summary>
        /// This Testcase validates the behavior of the method HealIfHasntMoved
        /// </summary>
        [Test]
        public void TestHealIfHasntMoved()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.AreEqual(beneGesserit.healthMax,beneGesserit.healthCurrent);
            bool healed = beneGesserit.HealIfHasntMoved();
            Assert.IsFalse(healed);
            Assert.AreEqual(beneGesserit.healthMax, beneGesserit.healthCurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpentMP
        /// </summary>
        [Test]
        public void TestSpentMP()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.AreEqual(beneGesserit.MPmax, beneGesserit.MPcurrent);
            beneGesserit.SpentMP(1);
            Assert.AreEqual(beneGesserit.MPmax-1, beneGesserit.MPcurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpentMP param to high
        /// </summary>
        [Test]
        public void TestSpentMPParamToHigh()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.AreEqual(beneGesserit.MPmax, beneGesserit.MPcurrent);
            bool possible = beneGesserit.SpentMP(100);
            Assert.IsFalse(possible);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpentAp
        /// </summary>
        [Test]
        public void TestSpentAp()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.AreEqual(beneGesserit.APmax, beneGesserit.APcurrent);
            beneGesserit.SpentAp(1);
            Assert.AreEqual(beneGesserit.APmax - 1, beneGesserit.APcurrent);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpentAp
        /// </summary>
        [Test]
        public void TestSpentApParamTooHigh()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Assert.AreEqual(beneGesserit.APmax, beneGesserit.APcurrent);
            bool spentPossible = beneGesserit.SpentAp(100);
            Assert.False(spentPossible);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method resetMPandAp
        /// </summary>
        [Test]
        public void TestresetMPandAp()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            beneGesserit.MPcurrent = 0;
            beneGesserit.APcurrent = 0;
            beneGesserit.resetMPandAp();
            Assert.AreEqual(beneGesserit.APmax, beneGesserit.APcurrent);
            Assert.AreEqual(beneGesserit.MPmax, beneGesserit.MPcurrent);
        }


        /// <summary>
        /// This Testcase validates the behavior of the method StandingNextToCityField
        /// </summary>
        [Test]
        public void TestStandingNextToCityFieldTrue()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            beneGesserit.greatHouse = new Corrino();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            beneGesserit.greatHouse.City = (City)map.fields[0, 0];
            beneGesserit.CurrentMapfield = map.fields[0, 1];
            bool standingNextToCity = beneGesserit.StandingNextToCityField();
            Assert.IsTrue(standingNextToCity);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method StandingNextToCityField
        /// </summary>
        [Test]
        public void TestStandingNextToCityFieldFalse()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            beneGesserit.greatHouse = new Corrino();
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            beneGesserit.greatHouse.City = (City)map.fields[0, 0];
            beneGesserit.CurrentMapfield = map.fields[2, 1];
            bool standingNextToCity = beneGesserit.StandingNextToCityField();
            Assert.IsFalse(standingNextToCity);
        }


        /// <summary>
        /// This Testcase validates the behavior of the method Movement with invalid parameter
        /// </summary>
        [Test]
        public void TestMovementInvalid()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            bool movementPossible = beneGesserit.Movement(map.fields[0, 0], map.fields[0, 3]);
            Assert.IsFalse(movementPossible);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method Movement
        /// </summary>
        [Test]
        public void TestMovement()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Map map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            bool movementPossible = beneGesserit.Movement(map.fields[0, 0], map.fields[1, 1]);
            Assert.IsTrue(movementPossible);
            Assert.AreEqual(beneGesserit.MPmax - 1, beneGesserit.MPcurrent);
            Assert.AreEqual(map.fields[1, 1], beneGesserit.CurrentMapfield);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method GiftSpice impossible
        /// </summary>
        [Test]
        public void TestGiftSpiceNotPossible()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Noble nobel = new Noble("name");
            Assert.AreEqual(0, nobel.inventoryUsed);
            bool giftPossible = beneGesserit.GiftSpice(nobel, 1);
            Assert.AreEqual(0, nobel.inventoryUsed);
            Assert.IsFalse(giftPossible);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method GiftSpice impossible
        /// </summary>
        [Test]
        public void TestGiftSpicePossible()
        {
            BeneGesserit beneGesserit = new BeneGesserit("someName");
            Noble nobel = new Noble("name");
            beneGesserit.inventoryUsed = 1;
            Assert.AreEqual(0, nobel.inventoryUsed);
            bool giftPossible = beneGesserit.GiftSpice(nobel, 1);
            Assert.AreEqual(1, nobel.inventoryUsed);
            Assert.AreEqual(0, beneGesserit.inventoryUsed);
            Assert.IsTrue(giftPossible);
        }
    }
}
