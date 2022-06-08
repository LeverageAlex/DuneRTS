using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class TransferRequestMessage : Message
    {
        [JsonProperty]
        public int ClientID { get; set; }
        [JsonProperty]
        public int CharacterID { get; set; }
        [JsonProperty]
        public int TargetID { get; set; }
        [JsonProperty]
        public int Amount { get; set; }

        public TransferRequestMessage(int clientID, int characterID, int targetID, int amount) : base("v1", MessageType.TRANSFER_REQUEST)
        {
            this.ClientID = clientID;
            this.CharacterID = characterID;
            this.TargetID = targetID;
            this.Amount = amount;
        }
    }
}
