using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Message is used to Rejoin to a Game.
    /// </summary>
    public class RejoinMessage : Message
    {
        [JsonProperty]
        public string ClientSecret { get; }

        /// <summary>
        /// Contructor of the class RejoinMessage
        /// </summary>
        /// <param name="clientSecret">The client secret to identificate if it's the same client which was already connected.</param>
        public RejoinMessage(string clientSecret) : base("1.0", MessageType.REJOIN)
        {
            this.ClientSecret = clientSecret;
        }
    }
}
