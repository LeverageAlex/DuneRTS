using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Class is used as a message to request a pause of the game
    /// </summary>
    public class PauseGameRequestMessage : ClientServerMessage
    {
        [JsonProperty]
        private bool pause;

        /// <summary>
        /// Constructor of the class PauseGameRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        public PauseGameRequestMessage(int clientID) : base(clientID, Enums.MessageType.PAUSE_REQUEST)
        {
            this.pause = true;
        }
    }
}
