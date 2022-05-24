using System;
using Server.Clients;

namespace Server
{
    /// <summary>
    /// Represents a ai player, which can interact with the game and must send its moves within a certain time interval
    /// </summary>
    public class AIPlayer : Player
    {
        /// <summary>
        /// creates a new ai player
        /// </summary>
        /// <param name="clientName">the name of the ai player</param>
        /// <param name="sessionID">the id of the session of this ai player (from websocket server)</param>
        public AIPlayer(string clientName, string sessionID) : base(clientName, sessionID)
        {
        }
    }
}
