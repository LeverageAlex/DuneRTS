using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    public abstract class Message
    {
        private int version;
        private Enums.MessageType type;

        public Message(int version, Enums.MessageType type)
        {
            this.version = version;
            this.type = type;
        }

        public Enums.MessageType getMessageType()
        {
            return this.type;
        }
    }
}
