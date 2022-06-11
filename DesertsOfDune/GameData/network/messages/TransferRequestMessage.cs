using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class TransferRequestMessage : Message
    {
        [JsonProperty]
        public int clientID { get; set; }
        [JsonProperty]
        public int characterID { get; set; }
        [JsonProperty]
        public int targetID { get; set; }
        [JsonProperty]
        public int amount { get; set; }

        public TransferRequestMessage(int clientID, int characterID, int targetID, int amount) : base("1.0", MessageType.TRANSFER_REQUEST)
        {
            this.clientID = clientID;
            this.characterID = characterID;
            this.targetID = targetID;
            this.amount = amount;
        }
    }
}
