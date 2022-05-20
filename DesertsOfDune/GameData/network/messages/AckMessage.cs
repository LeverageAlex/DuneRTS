using System;
namespace GameData.network.messages
{
    public class AckMessage : Message
    {
        public AckMessage() : base("v1", MessageType.ACK)
        {
            
        }
    }
}
