using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class is used to store a Position on the Map
    /// </summary>
    public class Position
    {
        [JsonProperty]
        private float x;
        [JsonProperty]
        private float y;

        /// <summary>
        /// Constructor of the class Position
        /// </summary>
        /// <param name="x">the x coordinate on the map</param>
        /// <param name="y">the y coordinate on the map</param>
        public Position(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// empty constructor of the class Position
        /// </summary>
        public Position()
        {

        }

        /// <summary>
        /// this method is used to do a movement
        /// </summary>
        /// <param name="dx">the x coordinate to move to</param>
        /// <param name="dy">the y coordinate to move to</param>
        /// <returns>true, if the move was possible</returns>
        public bool move(int dx, int dy)
        {
            return false;
        }
    }
}
