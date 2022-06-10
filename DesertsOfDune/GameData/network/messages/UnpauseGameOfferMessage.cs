using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class UnpauseGameOfferMessage : Message
    {
        [JsonProperty]
        public int requestedByClientID { get; }

        /// <summary>
        /// Message to finish the pause and continue the game.
        /// </summary>
        /// <param name="requestedByClintID">ID from client, who requested unpause</param>
        public UnpauseGameOfferMessage(int requestedByClintID) : base("1.0", MessageType.UNPAUSE_GAME_OFFER)
        {
            this.requestedByClientID = requestedByClintID;
        }
    }
}
