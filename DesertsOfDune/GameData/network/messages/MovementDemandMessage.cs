using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to communicate movement of characters.
    /// </summary>
    public class MovementDemandMessage : TurnMessage
    {
        [JsonProperty]
        public Specs specs { get; }

        /// <summary>
        /// Constructor of the class MovementMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="path">the path the character takes</param>
        public MovementDemandMessage(int clientID, int characterID, List<Position> path) : base(characterID,clientID,MessageType.MOVEMENT_DEMAND)
        {
         //   specs = new Specs(path, null);
          //  specs.path = path;
        }
    }
}
