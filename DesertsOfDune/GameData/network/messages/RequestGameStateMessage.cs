using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate the request of the gamestate
    /// </summary>
    public class RequestGameStateMessage : ClientServerMessage
    {
        /// <summary>
        /// Constructor of the class RequestGameStateMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        public RequestGameStateMessage(int clientID) : base(clientID,MessageType.REQUEST_GAMESTATE)
        {

        }
    }
}
