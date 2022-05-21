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
        private string[] history;

        /// <summary>
        /// Constructor of the class GameStateMessage
        /// </summary>
        /// <param name="history">the history of the Game</param>
        /// <param name="history">the client id</param>
        public GameStateMessage(string[] history, int clientID) : base(clientID,MessageType.GAMESTATE)
        {
            this.history = history;
        }
    }
}
