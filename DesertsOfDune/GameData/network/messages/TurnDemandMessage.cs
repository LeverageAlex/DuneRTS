using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used by the client to demand a specific Turn
    /// </summary>
    public class TurnDemandMessage : TurnMessage
    {
        /// <summary>
        /// Constructor of the class TurnDemandMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        public TurnDemandMessage(int clientID, int characterID) : base(characterID, clientID, MessageType.TURN_DEMAND)
        {
            
        }
    }
}
