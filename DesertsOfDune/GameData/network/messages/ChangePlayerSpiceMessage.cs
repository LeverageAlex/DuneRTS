using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate a change of the player spice
    /// </summary>
    public class ChangePlayerSpiceMessage : ClientServerMessage
    {
        private int newSpiceValue;

        /// <summary>
        /// Constructor of the class ChangePlayerSpiceMessage
        /// </summary>
        /// <param name="newSpiceValue">the new amount of spice the player has</param>
        /// <param name="clientID">the client id</param>
        public ChangePlayerSpiceMessage(int newSpiceValue, int clientID) : base(clientID, MessageType.CHANGE_PLAYER_SPICE)
        {
            this.newSpiceValue = newSpiceValue;
        }
    }
}
