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
        public string ClientName { get; }
        [JsonProperty]
        public string ConnectionCode { get; }
        [JsonProperty]
        public bool Active { get; }
        [JsonProperty]
        public bool IsCpu { get; }

        /// <summary>
        /// Constuctor of the class JoinMessage
        /// </summary>
        /// <param name="clientName">the name of the client</param>
        /// <param name="connectionCode">the code of the connection</param>
        /// <param name="active">notification if the client is player or participate</param>
        /// <param name="isCpu">notification if the client is humanPlayer or AIPlayer</param>
        public JoinMessage (string clientName, string connectionCode, bool active, bool isCpu) : base("v1", MessageType.JOIN)
        {
            this.ClientName = clientName;
            this.ConnectionCode = connectionCode;
            this.Active = active;
            this.IsCpu = isCpu;
        }
    }
}
