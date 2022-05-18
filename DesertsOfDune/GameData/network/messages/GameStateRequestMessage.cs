using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This message class is used to request the gamestate
    /// </summary>
    public class GameStateRequestMessage : ClientServerMessage
    {
        /// <summary>
        /// Constructor of the class GameStateRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        public GameStateRequestMessage(int clientID) : base(clientID,MessageType.REQUEST_GAMESTATE)
        {
            
        }
    }
}
