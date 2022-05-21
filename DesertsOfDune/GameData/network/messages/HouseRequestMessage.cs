using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to Request a House.
    /// </summary>
    public class HouseRequestMessage : Message
    {
        [JsonProperty]
        private string HouseName;

        /// <summary>
        /// Constructor of the class HouseRequestMessage
        /// </summary>
        /// <param name="houseName">the House the client wants to request.</param>
        public HouseRequestMessage(string houseName) : base("v1", MessageType.HOUSE_REQUEST)
        {
            this.HouseName = houseName;
        }
    }
}
