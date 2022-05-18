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
        private int cpuCount;

        /// <summary>
        /// Constructor of the class CreateMessage
        /// </summary>
        /// <param name="lobbyCode">the lobbyCode of the game lobby</param>
        /// <param name="cpuCount">the amount of cpu's</param>
        public CreateMessage(string lobbyCode,int cpuCount) : base("v1", MessageType.CREATE)
        {
            this.lobbyCode = lobbyCode;
            this.cpuCount = cpuCount;
        }

    }
}
