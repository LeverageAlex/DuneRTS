using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Class is used as a message to request a pause of or to request continuation of the game
    /// </summary>
    public class PauseGameRequestMessage : Message
    {
        [JsonProperty]
        private bool pause;

        /// <summary>
        /// Constructor of the class PauseGameRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="pause">true, if client wishes to pause the game if he wishes to continue false</param>
        public PauseGameRequestMessage(bool pause) : base("v1",MessageType.PAUSE_REQUEST)
        {
            this.pause = pause;
        }
    }
}
