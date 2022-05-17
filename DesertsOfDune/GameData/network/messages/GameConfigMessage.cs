using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the game configuration.
    /// </summary>
    public class GameConfigMessage : Message
    {
        private string scenario;
        private string partyConfig;

        /// <summary>
        /// Constructor of the class GameConfigMessage
        /// </summary>
        /// <param name="scenario">the scenario configuration</param>
        /// <param name="partyConfig">the party configuration</param>
        public GameConfigMessage(string scenario, string partyConfig) : base("v1", MessageType.GAMECFG)
        {
            this.scenario = scenario;
            this.partyConfig = partyConfig;
        }
    }
}
