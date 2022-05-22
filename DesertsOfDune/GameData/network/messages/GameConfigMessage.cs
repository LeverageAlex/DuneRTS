using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the game configuration.
    /// </summary>
    public class GameConfigMessage : Message
    {
        [JsonProperty]
        private List<string[]> scenario;
        [JsonProperty]
        private string party;
        [JsonProperty]
        private string[] houseOffer;

        /// <summary>
        /// Constructor of the class GameConfigMessage
        /// </summary>
        /// <param name="scenario">the scenario configuration</param>
        /// <param name="partyConfig">the party configuration</param>
        /// <param name="partyConfig">array of houses that are offered to the client</param>
        public GameConfigMessage(List<string[]> scenario, string party, string[] houseOffer) : base("0.1", MessageType.GAMECFG)
        {
            this.scenario = scenario;
            this.party = party;
            this.houseOffer = houseOffer;
        }
    }
}
