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
        private int Client0Id;
        [JsonProperty]
        private int Client1Id;

        /// <summary>
        /// Constructor of the class GameConfigMessage
        /// </summary>
        /// <param name="scenario">the scenario configuration</param>
        /// <param name="party">The reference to the saved party configuration</param>
        /// <param name="client0ID">ID of the first player</param>
        /// <param name="client1ID">ID of the second player</param>
        public GameConfigMessage(List<string[]> scenario, string party, int client0ID, int client1ID) : base("v1", MessageType.GAMECFG)
        {
            this.scenario = scenario;
            this.party = party;
            this.Client0Id = client0ID;
            this.Client1Id = client1ID;
        }
    }
}
