using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to request a spice transfer
    /// </summary>
    public class TransferRequestMessage : ClientServerMessage
    {
        [JsonProperty]
        private int characterID;
        [JsonProperty]
        private int targetID;
        [JsonProperty]
        private int amount;

        /// <summary>
        /// Constructor of the class TransferRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="targetID">the id of the target chracter</param>
        /// <param name="amount">the amount of spice to be transfered</param>
        public TransferRequestMessage(int clientID, int characterID, int targetID, int amount) : base(clientID, MessageType.TRANSFER_REQUEST)
        {
            this.clientID = clientID;
            this.characterID = characterID;
            this.amount = amount;
        }
    }
}
