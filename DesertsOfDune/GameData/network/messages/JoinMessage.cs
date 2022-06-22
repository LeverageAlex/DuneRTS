using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Message is used to Join to a Game.
    /// </summary>
    public class JoinMessage : Message
    {
        [JsonProperty]
        public string clientName { get; }
        [JsonProperty]
        public bool isActive { get; }
        [JsonProperty]
        public bool isCpu { get; }

        /// <summary>
        /// Constuctor of the class JoinMessage
        /// </summary>
        /// <param name="clientName">the name of the client</param>
        /// <param name="isActive">whether the client who connects is active player or spectator</param>
        /// <param name="isCpu">true, if the client is a cpu</param>
        public JoinMessage(string clientName, bool isActive, bool isCpu) : base("1.1", MessageType.JOIN)
        {
            this.clientName = clientName;
            this.isActive = isActive;
            this.isCpu = isCpu;
        }
    }
}
