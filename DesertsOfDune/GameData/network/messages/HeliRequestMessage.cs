using GameData.network.util.world;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This message class is used to comunicate the request of a helicopter
    /// </summary>
    public class HeliRequestMessage : TurnMessage
    {

        public Position target { get; }

        /// <summary>
        /// Constructor of the class HeliRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="target">the target position of the heli</param>
        public HeliRequestMessage(int clientID, int characterID, Position target) : base(characterID, clientID, MessageType.HELI_REQUEST)
        {
            this.target = target;
        }
    }
}
