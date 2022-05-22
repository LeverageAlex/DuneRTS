using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate the paus of the game.
    /// </summary>
    public class PauseGameMessage : Message
    {
        [JsonProperty]
        private int requestedByClientID;
        [JsonProperty]
        private bool pause;

        /// <summary>
        /// Constructor of the class PauseGameMessage
        /// </summary>
        /// <param name="requestedByClientID">the client id</param>
        /// <param name="pause">true if the game is paused.</param>
        public PauseGameMessage(int requestedByClientID, bool pause) : base("1.0", MessageType.PAUSE_GAME)
        {
            this.pause = pause;
            this.requestedByClientID = requestedByClientID;
        }
    }
}
