using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class UnpauseGameOfferMessage : Message
    {
        [JsonProperty]
        public int RequestedByClientID { get; }

        /// <summary>
        /// Message to finish the pause and continue the game.
        /// </summary>
        /// <param name="requestedByClintID">ID from client, who requested unpause</param>
        public UnpauseGameOfferMessage(int requestedByClintID) : base("v1", MessageType.UNPAUSE_GAME_OFFER)
        {
            this.RequestedByClientID = requestedByClintID;
        }
    }
}
