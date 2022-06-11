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
        public CityToClient[] cityToClient { get; }
        [JsonProperty]
        public Position stormEye { get; }

        /// <summary>
        /// Constructor of the class GameConfigMessage
        /// </summary>
        /// <param name="scenario">the scenario configuration</param>
        /// <param name="party">the party configuration</param>
        /// <param name="cityToClient">The id or the client and the coordinates of his cityt</param>
        /// <param name="stormEye"></param>
        /// <param name="client1ID"></param>
        public GameConfigMessage(List<List<string>> scenario, PartyReference party, CityToClient[] cityToClient, Position stormEye) : base("1.0", MessageType.GAMECFG)
        {
            this.scenario = scenario;
            this.party = party;
            this.cityToClient = cityToClient;
            this.stormEye = stormEye;
        }
    }
}
