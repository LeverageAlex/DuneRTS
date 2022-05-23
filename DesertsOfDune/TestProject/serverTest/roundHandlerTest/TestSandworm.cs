using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;

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
            SandWorm sandWorm = SandWorm.Spawn(0,0);
            sandWorm.Execute();

        }
    }
}
