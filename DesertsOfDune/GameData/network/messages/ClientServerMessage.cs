using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to model clientserver Messages
    /// </summary>
    public abstract class ClientServerMessage : Message
    {
        [JsonProperty(Order = -3)]
        public int clientID { get; }

        /// <summary>
        /// Constructor of the class ClientServerMessage
        /// </summary>
        /// <param name="clientID">the id of the client.</param>
        public ClientServerMessage(int clientID, MessageType type) : base("1.0", type)
        {
            this.clientID = clientID;
        }
    }
}
