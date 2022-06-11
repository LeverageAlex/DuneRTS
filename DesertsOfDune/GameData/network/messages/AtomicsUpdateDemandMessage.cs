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
        public int clientID { get; }
        [JsonProperty]
        public bool shunned { get; }
        [JsonProperty]
        public int atomicsLeft { get; }

        /// <summary>
        /// Constructor of the class AtomicsUpdateDemandMessage.
        /// </summary>
        /// <param name="clientID">The ID of the client.</param>
        /// <param name="shunned">Determines, if player is outlawed after using Atomics action.</param>
        /// <param name="atomicsLeft">Determines, how much atomics actions the player can use.</param>
        public AtomicsUpdateDemandMessage(int clientID, bool shunned, int atomicsLeft) : base("1.0", MessageType.ATOMICS_UPDATE_DEMAND)
        {
            this.clientID = clientID;
            this.shunned = shunned;
            this.atomicsLeft = atomicsLeft;
        }
    }
}
