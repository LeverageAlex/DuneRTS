using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class resambles one field on the playingfield.
    /// </summary>
    public class MapField
    {
        // [JsonProperty]
        /*TODO: implement tiletype 
         * private TileType tileType;*/
        [JsonProperty]
        private bool hasSpice;
        [JsonProperty]
        private bool isInSandstorm;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private int clientID;
        [JsonProperty]
        private Position stormEye;

        /// <summary>
        /// Constructor of the class MapField
        /// </summary>
        /// <param name="tt">the type of field</param>
        /// <param name="hasSpice">true, if it countains spice</param>
        /// <param name="isInSandstorm">true, if there is a sandstorm on the field.</param>
        /// <param name="clientID">the id of the client (only to be set for city tiles)</param>
        /// <param name="stormEye">the center position of the storm</param>
        public MapField(/* TileType tt*/ bool hasSpice, bool isInSandstorm, int clientID, Position stormEye)
        {
            //this.tileType = tt;
            this.hasSpice = hasSpice;
            this.isInSandstorm = isInSandstorm;
            this.clientID = clientID;
            this.stormEye = stormEye;
        }
    }
}
