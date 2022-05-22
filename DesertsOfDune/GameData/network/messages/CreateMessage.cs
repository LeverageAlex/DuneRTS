using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used as a message to create a game
    /// </summary>
    public class CreateMessage : Message
    {
        [JsonProperty]
        private string lobbyCode;
        [JsonProperty]
        private bool spectate;

        /// <summary>
        /// Constructor of the class CreateMessage
        /// </summary>
        /// <param name="lobbyCode">the lobbyCode of the game lobby</param>
        /// <param name="spectate">true, if the client wants to spectate the game</param>
        public CreateMessage(string lobbyCode,bool spectate) : base("0.1", MessageType.CREATE)
        {
            this.lobbyCode = lobbyCode;
            this.spectate = spectate;
        }

    }
}
