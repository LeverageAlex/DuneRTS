using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used as a base class for all TurnMessages withing a round of the game.
    /// </summary>
    public abstract class TurnMessage : ClientServerMessage
    {
        [JsonProperty(Order = -2)]
        private int characterID;

        /// <summary>
        /// Constructor of the class TurnMessage
        /// </summary>
        /// <param name="characterID">the id of the character</param>
        /// <param name="clientID">the id of the client</param>
        /// <param name="messageType">the type of message</param>
        public TurnMessage(int characterID, int clientID, MessageType messageType) : base(clientID, messageType)
        {
            this.characterID = characterID;
        }
    }
}
