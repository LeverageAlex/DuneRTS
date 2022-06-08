using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to notificate all players after one player used the atomics action.
    /// </summary>
    public class AtomicsUpdateDemandMessage : Message
    {
        [JsonProperty]
        public int ClientID { get; }
        [JsonProperty]
        public bool Shunned { get; }
        [JsonProperty]
        public int AtomicsLeft { get; }

        /// <summary>
        /// Constructor of the class AtomicsUpdateDemandMessage.
        /// </summary>
        /// <param name="clientID">The ID of the client.</param>
        /// <param name="shunned">Determines, if player is outlawed after using Atomics action.</param>
        /// <param name="atomicsLeft">Determines, how much atomics actions the player can use.</param>
        public AtomicsUpdateDemandMessage(int clientID, bool shunned, int atomicsLeft) : base("0.1", MessageType.ATOMICS_UPDATE_DEMAND)
        {
            this.ClientID = clientID;
            this.Shunned = shunned;
            this.AtomicsLeft = atomicsLeft;
        }
    }
}
