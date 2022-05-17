using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    public class StrikeMessage : Message
    {
        private string wrongMessage;
        private int count;

        public StrikeMessage(string wrongMessage, int count) : base("v1", MessageType.STRIKE)
        {
            this.wrongMessage = wrongMessage;
            this.count = count;
        }
    }
}
