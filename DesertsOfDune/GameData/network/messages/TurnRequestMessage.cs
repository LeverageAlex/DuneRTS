using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate a turn request.
    /// </summary>
    public class TurnRequestMessage : TurnMessage
    {
        /// <summary>
        /// Constructor of the class TurnRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        public TurnRequestMessage(int clientID, int characterID) : base(characterID, clientID,ActionType.MessageType.TURN_REQUEST) 
        {

        }
    }
}
