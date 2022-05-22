using System;
using Server.Clients;

namespace Server
{
    /// <summary>
    /// Represents a human player, which can interact with the game and pause it.
    /// </summary>
    public class HumanPlayer : Player
    {
        /// <summary>
        /// creates a new human player
        /// </summary>
        /// <param name="clientName">the name of the human player</param>
        /// <param name="sessionID">the id of the session of this human player (from websocket server)</param>
        public HumanPlayer(string clientName, string sessionID) : base(clientName, sessionID)
        {
        }
    }
}
