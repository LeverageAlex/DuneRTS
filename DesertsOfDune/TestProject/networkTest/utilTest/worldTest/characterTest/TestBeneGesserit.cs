using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.network.util.world.character;

namespace UnitTestSuite.networkTest.utilTest.worldTest.characterTest
{
    /// <summary>
    /// This Class is used to tests the methods of the class BeneGesserit
    /// </summary>
    public class TestBeneGesserit
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// This Testcase validates the behavior of the method atack
        /// </summary>
        [Test]
        public void TestAtack()
        {
            // TODO: implement logic
        }

        /// <summary>
        /// This Testcase validates the behavior of the method DecreaseHP
        /// </summary>
        [Test]
        public void TestDecreaseHP()
        {
            // TODO: implement logic
        }

        /// <summary>
        /// This Testcase validates the behavior of the method IsDead
        /// </summary>
        [Test]
        public void TestIsDead()
        {
            // TODO: implement logic
        }


        /// <summary>
        /// This Testcase validates the behavior of the method IsLoud
        /// </summary>
        [Test]
        public void Testloud()
        {
            BeneGesserit bene = new BeneGesserit(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, true, false);
            bool loud = bene.IsLoud();
            Assert.IsTrue(loud);
        }

        /// <summary>
        /// This Testcase validates the behavior of the method CollectSpice
        /// </summary>
        [Test]
        public void TestCollectSpice()
        {
            BeneGesserit bene = new BeneGesserit(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, true, false);
            bool loud = bene.CollectSpice();
            Assert.IsTrue(loud);
        }



        /// <summary>
        /// This Testcase validates the behavior of the method HealIfHasntMoved
        /// </summary>
        [Test]
        public void TestHealIfHasntMoved()
        {
            // TODO: implement logic
        }

        /// <summary>
        /// This Testcase validates the behavior of the method SpentMP
        /// </summary>
        [Test]
        public void TestSpentMP()
        {
            // TODO: implement logic
        }


        /// <summary>
        /// This Testcase validates the behavior of the method SpentAp
        /// </summary>
        [Test]
        public void TestSpentAp()
        {
            // TODO: implement logic
        }


        /// <summary>
        /// This Testcase validates the behavior of the method resetMPandAp
        /// </summary>
        [Test]
        public void TestresetMPandAp()
        {
            // TODO: implement logic
        }


        /// <summary>
        /// This Testcase validates the behavior of the method StandingNextToCityField
        /// </summary>
        [Test]
        public void TestStandingNextToCityField()
        {
            // TODO: implement logic
        }


        /// <summary>
        /// This Testcase validates the behavior of the method Movement
        /// </summary>
        [Test]
        public void TestMovement()
        {
            // TODO: implement logic
        }

        /// <summary>
        /// This Testcase validates the behavior of the method GiftSpice
        /// </summary>
        [Test]
        public void TestGiftSpice()
        {
            // TODO: implement logic
        }

        /// <summary>
        /// This Testcase validates the behavior of the method Voice
        /// </summary>
        [Test]
        public void TestVoice()
        {
            // TODO: implement logic
        }

    }
}
