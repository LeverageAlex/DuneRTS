using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to comunicate the statistics at the end of the game
    /// </summary>
    public class GameEndMessage : Message
    {
        [JsonProperty]
        private int winnerID;
        [JsonProperty]
        private int loserID;
        [JsonProperty]
        private Statistics statistics;

        /// <summary>
        /// Constructor of the class GameEndMessage
        /// </summary>
        /// <param name="winner">the winner of the game</param>
        /// <param name="loser">the loser of the game</param>
        /// <param name="statistics">the statistics of the game</param>
        public GameEndMessage(int winnerID, int loserID, Statistics statistics) : base("v1",MessageType.GAME_END)
        {
            this.winnerID = winnerID;
            this.loserID = loserID;
            this.statistics = statistics;
        }
    }
}
