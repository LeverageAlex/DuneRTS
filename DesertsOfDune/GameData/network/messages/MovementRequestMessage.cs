using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate a movementrequest.
    /// </summary>
    public class MovementRequestMessage : TurnMessage
    {
        private Position[] path;

        /// <summary>
        /// Constructor of the class MovementRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="path">the path the character requests to take</param>
        public MovementRequestMessage(int clientID, int characterID, Position[] path) : base(characterID, clientID, Enums.MessageType.MOVEMENT_REQUEST)
        {
            this.path = path;
        }
    }
}
