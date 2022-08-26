using System;
using GameData.ClientManagement.Clients;

namespace GameData
{
    /// <summary>
    /// Represents a spectator, who watch the game live and only gets informed about the game but cannot play the game activly
    /// </summary>
    public class Spectator : Client
    {
        /// <summary>
        /// creates a new spectator (live watching)
        /// </summary>
        /// <param name="clientName">the name of the spectator</param>
        /// <param name="clientSecret">a secret identifier between the spectator and the server for reconnect</param>
        public Spectator(string clientName, string sessionID) : base(clientName, false, sessionID, false)
        {
        }
    }
}
