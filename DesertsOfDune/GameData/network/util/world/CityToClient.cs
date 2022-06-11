using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class is used for the GAMECFG message
    /// </summary>
    public class CityToClient
    {
        [JsonProperty]
        public int clientID { get; }
        [JsonProperty]
        public int x { get; }
        [JsonProperty]
        public int y { get; }

        /// <summary>
        /// Constructor of the class CityToClient
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="x">the x coordinate of its city</param>
        /// <param name="y">the y coordinate of its city</param>
        public CityToClient(int clientID, int x, int y)
        {
            this.clientID = clientID;
            this.x = x;
            this.y = y;
        }
    }
}
