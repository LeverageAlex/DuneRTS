using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class TransferDemandMessage : Message
    {
        [JsonProperty]
        private int ClientID;
        [JsonProperty]
        private int CharacterID;
        [JsonProperty]
        private int TargetID;

        public TransferDemandMessage(int clientID, int characterID, int targetID) : base("v1", MessageType.TRANSFER_DEMAND)
        {
            this.ClientID = clientID;
            this.CharacterID = characterID;
            this.TargetID = targetID;
        }
    }
}
