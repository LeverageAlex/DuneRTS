using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the House acknowledgement
    /// </summary>
    public class HouseAcknowledgementMessage : ClientServerMessage
    {
        [JsonProperty]
        public string houseName { get; }

        /// <summary>
        /// Constructor of the class HouseAcknowledgementMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="houseName">the name of the acknowlaged house for the client</param>
        public HouseAcknowledgementMessage(int clientID, string houseName) : base(clientID,MessageType.HOUSE_ACKNOWLEDGEMENT)
        {
            this.houseName = houseName;
        }
    }
}
