using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.network.messages
{
     /// <summary>
    /// This class is used to inform about Map changes.
    /// </summary>
    public class MapChangeMessage : Message
    {
        private ActionType.MapChangeReasons mapChangeReason;
        private MapField[][] newMap;
        private Position stormEye;

        /// <summary>
        /// Constructor of the class MapChangeMessage
        /// </summary>
        /// <param name="reason">holds the reason for the Map change.</param>
        /// <param name="newMap">the new Map that is send.</param>
        /// <param name="stormEye">the position of the stormEye</param>
        public MapChangeMessage(ActionType.MapChangeReasons reason, MapField[][] newMap, Position stormEye) : base("v1", ActionType.MessageType.MAP_CHANGE)
        {
            this.mapChangeReason = reason;
            this.newMap = newMap;
            this.stormEye = stormEye;
        }

    }
}
