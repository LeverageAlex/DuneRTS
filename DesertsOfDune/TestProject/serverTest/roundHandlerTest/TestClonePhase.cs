using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameData.server.roundHandler;
using GameData.network.messages;
using GameData.network.util.world.character;
using Server.Configuration;

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
            ConfigurationFileLoader loader = new ConfigurationFileLoader();

            // load scenario and create a new scenario configuration
            ScenarioConfiguration scenarioConfiguration = loader.LoadScenarioConfiguration("");
            ScenarioConfiguration.CreateInstance(scenarioConfiguration);

            // load the party configuration and create a new party configuration class
            PartyConfiguration partyConfiguration = loader.LoadPartyConfiguration("");
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
        /// This Testcase validates the behaviour of the method CloneCharacter
        /// </summary>
        [Test]
        public void TestCloneCharacter()
        {
            // implement logic
         /*   ClonePhase clonePhase = new ClonePhase();
            Noble character = new Noble(2, 1, 4, 5, 6, 7, 8, 8, 5, 4, true, true);
            clonePhase.CloneCharacter(character, null); */
        }
        
    }
}
