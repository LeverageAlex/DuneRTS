using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate the acceptance of a client join.
    /// </summary>
    public class JoinAcceptedMessage : ClientServerMessage
    {
        [JsonProperty(Order = -3)]
        public string clientSecret { get; }

        /// <summary>
        /// Constructor of the class JoinAcceptedMessage
        /// </summary>
        /// <param name="clientSecret">the used to identify the client</param>
        public JoinAcceptedMessage(string clientSecret, int clientID) : base(clientID,MessageType.JOINACCEPTED)
        {
            this.clientSecret = clientSecret;
        }
    }
}
