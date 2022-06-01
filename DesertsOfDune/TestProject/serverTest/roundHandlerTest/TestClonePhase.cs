using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.network.messages;
using GameData.network.util.world.character;

namespace UnitTestSuite.serverTest.roundHandlerTest
{
    /// <summary>
    /// This Class is used to Test the class ClonePhase
    /// </summary>
    public class TestClonePhase
    {
        [SetUp]
        public void Setup()
        {
        }


        /// <summary>
        /// This Testcase validates the behaviour of the method CloneCharacter
        /// </summary>
        [Test]
        public void TestCloneCharacter()
        {
            // implement logic
            ClonePhase clonePhase = new ClonePhase();
            Noble character = new Noble(2, 1, 4, 5, 6, 7, 8, 8, 5, 4, true, true);
            clonePhase.CloneCharacter(character, null);
        }
        
    }
}
