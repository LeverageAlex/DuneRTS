using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate the acceptance of a client join.
    /// </summary>
    public class JoinAcceptedMessage : Message
    {
        [JsonProperty]
        private string ClientSecret;
        [JsonProperty]
        private int ClientID;

        /// <summary>
        /// Constructor of the class JoinAcceptedMessage
        /// </summary>
        /// <param name="clientSecret">used to identify the client</param>
        /// <param name="clientID">ID of the client</param>
        public JoinAcceptedMessage(string clientSecret, int clientID) : base("v1",MessageType.JOINACCEPTED)
        {
            this.ClientSecret = clientSecret;
            this.ClientID = clientID;
        }
    }
}
