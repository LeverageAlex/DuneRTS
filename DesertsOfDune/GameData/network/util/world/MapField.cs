using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.enums;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class resambles one field on the playingfield.
    /// </summary>
    public class MapField
    {
        [JsonProperty]
        private string tileType;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int clientID;
        [JsonProperty]
        private bool hasSpice;
        [JsonProperty]
        private bool isInSandstorm;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private Position stormEye;

        /// <summary>
        /// Constructor of the class MapField for a City MapField
        /// </summary>
        /// <param name="tt">the type of field</param>
        /// <param name="hasSpice">true, if it countains spice</param>
        /// <param name="isInSandstorm">true, if there is a sandstorm on the field.</param>
        /// <param name="clientID">the id of the client (only to be set for city tiles)</param>
        /// <param name="stormEye">the center position of the storm can be null</param>
        public MapField(bool hasSpice, bool isInSandstorm, int clientID, Position stormEye)
        {
            this.tileType = Enum.GetName(typeof(TileType),TileType.CITY);
            this.hasSpice = hasSpice;
            this.isInSandstorm = isInSandstorm;
            this.clientID = clientID;
            this.stormEye = stormEye;
        }

        /// <summary>
        /// Constructof of the Class MapField for Fields that are not the city
        /// </summary>
        /// <param name="tt">the type of the MapField</param>
        /// <param name="hasSpice">true, if the MapField has spice on it</param>
        /// <param name="isInSandstorm">true, if there is a sandstorm on the MapField</param>
        /// <param name="stormEye">the Position of the Sandstorm can also be null</param>
        public MapField(TileType tt, bool hasSpice, bool isInSandstorm, Position stormEye)
        {
            this.tileType = Enum.GetName(typeof(TileType), tt);
            this.hasSpice = hasSpice;
            this.isInSandstorm = isInSandstorm;
            this.stormEye = stormEye;
        }
    }
}
