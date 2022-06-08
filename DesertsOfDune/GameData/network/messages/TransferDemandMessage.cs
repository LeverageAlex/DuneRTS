using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is send by the server to demand a Transfer
    /// </summary>
    public class TransferDemandMessage : ClientServerMessage
    {
        [JsonProperty]
        private int characterID;
        [JsonProperty]
        private int targetID;

        /// <summary>
        /// Constructor of the class TransferDemandMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="targetID">the id of the target character</param>
        public TransferDemandMessage(int clientID, int characterID, int targetID) : base(clientID, MessageType.TRANSFER_DEMAND)
        {
            this.characterID = characterID;
            this.targetID = targetID;
        }
    }
}
