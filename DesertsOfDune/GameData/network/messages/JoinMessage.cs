using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Message is used to Join to a Game.
    /// </summary>
    public class JoinMessage : Message
    {
        [JsonProperty]
        public string clientName { get; }
        [JsonProperty]
        public string connectionCode { get; }
        [JsonProperty]
        public bool active { get; }
        [JsonProperty]
        public bool isCpu { get; }

        /// <summary>
        /// Constuctor of the class JoinMessage
        /// </summary>
        /// <param name="clientName">the name of the client</param>
        /// <param name="connectionCode">the code of the connection</param>
        /// <param name="active">weather of not the connection is active</param>
        /// <param name="isCpu">true, if the client is a cpu</param>
        public JoinMessage (string clientName, string connectionCode, bool active, bool isCpu) : base("0.1", MessageType.JOIN)
        {
            this.clientName = clientName;
            this.connectionCode = connectionCode;
            this.active = active;
            this.isCpu = isCpu;
        }
    }
}
