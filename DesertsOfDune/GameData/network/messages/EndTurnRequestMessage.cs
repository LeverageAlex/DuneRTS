using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate a request to end the current turn.
    /// </summary>
    public class EndTurnRequestMessage : TurnMessage
    {
        /// <summary>
        /// Constructor of the class EndTurnRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        public EndTurnRequestMessage(int clientID, int characterID) : base(characterID, clientID,ActionType.MessageType.END_TURN_REQUEST)
        {

        }
    }
}
