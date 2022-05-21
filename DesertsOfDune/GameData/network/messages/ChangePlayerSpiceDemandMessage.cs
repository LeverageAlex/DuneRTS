using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate a change of the player spice
    /// </summary>
    public class ChangePlayerSpiceDemandMessage : ClientServerMessage
    {
        [JsonProperty]
        private int newSpiceValue;

        /// <summary>
        /// Constructor of the class ChangePlayerSpiceMessage
        /// </summary>
        /// <param name="newSpiceValue">the new amount of spice the player has</param>
        /// <param name="clientID">the client id</param>
        public ChangePlayerSpiceDemandMessage(int newSpiceValue, int clientID) : base(clientID, MessageType.CHANGE_PLAYER_SPICE_DEMAND)
        {
            this.newSpiceValue = newSpiceValue;
        }
    }
}
