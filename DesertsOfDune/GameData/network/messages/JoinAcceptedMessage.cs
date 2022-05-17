using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate the acceptance of a client join.
    /// </summary>
    public class JoinAcceptedMessage : Message
    {
        private string clientSecret;

        /// <summary>
        /// Constructor of the class JoinAcceptedMessage
        /// </summary>
        /// <param name="clientSecret">the used to identify the client</param>
        public JoinAcceptedMessage(string clientSecret) : base("v1",ActionType.MessageType.JOINACCEPTED)
        {
            this.clientSecret = clientSecret;
        }
    }
}
