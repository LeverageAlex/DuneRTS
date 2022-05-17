using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    public class CreateMessage : Message
    {
        private string lobbyCode;
        private int CPUCount;

        public CreateMessage(string lobbyCode,int CPUCount) : base("v1", MessageType.CREATE)
        {
            this.lobbyCode = lobbyCode;
            this.CPUCount = CPUCount;
        }

        public CreateMessage(string lobbyCode) : base("v1", MessageType.CREATE)
        {
            this.lobbyCode = lobbyCode;
        }

    }
}
