using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to inform about Map changes.
    /// </summary>
    public class MapChangeDemandMessage : Message
    {
        [JsonProperty]
        public string changeReason { get; }
        [JsonProperty]
        public MapField[,] newMap { get; }
        [JsonProperty]
        public Position stormEye {get;}

        /// <summary>
        /// Constructor of the class MapChangeMessage
        /// </summary>
        /// <param name="changeReason">holds the reason for the Map change.</param>
        /// <param name="newMap">the new Map that is send.</param>
        public MapChangeDemandMessage(MapChangeReasons changeReason, MapField[,] newMap, Position stormEye) : base("1.1", MessageType.MAP_CHANGE_DEMAND)
        {
            this.changeReason = changeReason.ToString();
            this.newMap = newMap;
            this.stormEye = stormEye;
        }

    }
}
