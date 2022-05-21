using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the gamestate
    /// </summary>
    public class GameStateMessage : ClientServerMessage
    {
        [JsonProperty]
        private int[] ActivlyPlayingIDs;
        [JsonProperty]
        private string[] history;

        /// <summary>
        /// Constructor of the class GameStateMessage
        /// </summary>
        /// <param name="clientID">The ID of the client</param>
        /// <param name="activlyPlayingIDs">IDs of the activly playing players</param>
        /// <param name="history">the history of the Game</param>
        public GameStateMessage(int clientID, int[] activlyPlayingIDs, string[] history) : base(clientID,MessageType.GAMESTATE)
        {
            this.ActivlyPlayingIDs = activlyPlayingIDs;
            this.history = history;
        }
    }
}
