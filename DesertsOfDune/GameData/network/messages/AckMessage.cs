using System;
namespace GameData.network.messages
{
    public class AckMessage : Message
    {
        /// <summary>
        /// Acknowledgement message, if a lobby was succesful created by a client.
        /// </summary>
        public AckMessage() : base("v1", MessageType.ACK)
        {
            
        }
    }
}
