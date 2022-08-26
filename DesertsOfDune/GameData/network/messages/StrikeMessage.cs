using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Message is Send to the client if it uses wrongly formated Messages
    /// </summary>
    public class StrikeMessage : ClientServerMessage
    {
        [JsonProperty]
        public string wrongMessage { get; }
        [JsonProperty]
        public int count { get; }

        /// <summary>
        /// Constructor of the class StrikeMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="wrongMessage">description of the mistake</param>
        /// <param name="count"> count of Strikes</param>
        public StrikeMessage(int clientID, string wrongMessage, int count) : base(clientID, MessageType.STRIKE)
        {
            this.wrongMessage = wrongMessage;
            this.count = count;
        }
    }
}
