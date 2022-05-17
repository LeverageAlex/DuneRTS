using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This Message is used to Join to a Game.
    /// </summary>
    public class JoinMessage : Message
    {
        private string clientName;
        private string connectionCode;
        private bool active;

        /// <summary>
        /// Constuctor of the class JoinMessage
        /// </summary>
        /// <param name="clientName">the name of the client</param>
        /// <param name="connectionCode">the code of the connection</param>
        /// <param name="active">weather of not the connection is active</param>
        public JoinMessage (string clientName, string connectionCode, bool active) : base("v1", MessageType.JOIN)
        {
            this.clientName = clientName;
            this.connectionCode = connectionCode;
            this.active = active;
        }
    }
}
