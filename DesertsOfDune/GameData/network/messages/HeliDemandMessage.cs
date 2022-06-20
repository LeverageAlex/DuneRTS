using GameData.network.util.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    public class HeliDemandMessage : TurnMessage
    {

        public Position target { get; }
        public bool crash { get; }
        public HeliDemandMessage(int clientID, int characterID, Position target, bool crash) : base(characterID, clientID, MessageType.HELI_DEMAND)
        {
            this.target = target;
            this.crash = crash;
        }
    }
}
