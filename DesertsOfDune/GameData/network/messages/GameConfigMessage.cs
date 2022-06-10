using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GameData.network.util.world;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the game configuration.
    /// </summary>
    public class GameConfigMessage : Message
    {
        [JsonProperty]
        public List<List<string>> scenario { get; }
        [JsonProperty]
        public PartyReference party { get; }
        [JsonProperty]
        public int client0ID { get; }
        [JsonProperty]
        public int client1ID { get; }

        /// <summary>
        /// Constructor of the class GameConfigMessage
        /// </summary>
        /// <param name="scenario">the scenario configuration</param>
        /// <param name="partyConfig">the party configuration</param>
        /// <param name="partyConfig">array of houses that are offered to the client</param>
        public GameConfigMessage(List<List<string>> scenario, PartyReference party, int client0ID, int client1ID) : base("1.0", MessageType.GAMECFG)
        {
            //TODO: change GameConfigMessage like it's declared in standard document
            this.scenario = scenario;
            this.party = party;
            this.client0ID = client0ID;
            this.client1ID = client1ID;
        }
    }
}
