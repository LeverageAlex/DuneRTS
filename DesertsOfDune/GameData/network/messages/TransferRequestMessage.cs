using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class TransferRequestMessage : Message
    {
        [JsonProperty]
        private int ClientID;
        [JsonProperty]
        private int CharacterID;
        [JsonProperty]
        private int TargetID;
        [JsonProperty]
        private int Amount;

        public TransferRequestMessage(int clientID, int characterID, int targetID, int amount) : base("v1", MessageType.TRANSFER_REQUEST)
        {
            this.ClientID = clientID;
            this.CharacterID = characterID;
            this.TargetID = targetID;
            this.Amount = amount;
        }
    }
}
