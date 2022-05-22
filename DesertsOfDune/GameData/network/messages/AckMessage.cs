using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate actions.
    /// </summary>
    public class AckMessage : Message
    {
        public AckMessage() : base("0.1", MessageType.ACK)
        {

        }
    }
}
