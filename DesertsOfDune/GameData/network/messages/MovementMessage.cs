using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to communicate movement of characters.
    /// </summary>
    public class MovementMessage : TurnMessage
    {
        private Position[] path;

        /// <summary>
        /// Constructor of the class MovementMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="path">the path the character takes</param>
        public MovementMessage(int clientID, int characterID, Position[] path) : base(characterID,clientID,Enums.MessageType.MOVEMENT)
        {
            this.path = path;
        }
    }
}
