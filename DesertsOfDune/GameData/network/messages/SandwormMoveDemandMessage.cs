using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate Sandworm movement.
    /// </summary>
    public class SandwormMoveDemandMessage : Message
    {
        [JsonProperty]
        public List<Position> path { get; }

        /// <summary>
        /// Constructor of the class SandwormMoveMessage
        /// </summary>
        /// <param name="path">takes a List of Position to represent the sandworm movement.</param>
        public SandwormMoveDemandMessage(List<Position> path) : base("1.1", MessageType.SANDWORM_MOVE_DEMAND)
        {
            this.path = path;
        }
     }
}
