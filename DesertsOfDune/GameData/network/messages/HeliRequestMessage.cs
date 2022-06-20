using GameData.network.util.world;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    public class HeliRequestMessage : TurnMessage
    {
        //This has to be done, because the HELIPORT message doesn't contain a version (don't ask why)
        public Position target { get; }
        public HeliRequestMessage(int clientID, int characterID, Position target) : base(characterID, clientID, MessageType.HELI_REQUEST)
        {
            this.target = target;
        }
    }
}
