using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class resambles one field on the playingfield.
    /// </summary>
    public class MapField
    {
        private Position position;
        /*TODO: implement tiletype 
         * private TileType tileType;*/
        private bool hasSpice;
        private bool isInSandstorm;

        /// <summary>
        /// Constructor of the class MapField
        /// </summary>
        /// <param name="pos">the position of the MapField</param>
        /// <param name="tt">the type of field</param>
        /// <param name="hasSpice">true, if it countains spice</param>
        /// <param name="isInSandstorm">true, if there is a sandstorm on the field.</param>
        public MapField(Position pos,/* TileType tt*/ bool hasSpice, bool isInSandstorm)
        {
            this.position = pos;
            //this.tileType = tt;
            this.hasSpice = hasSpice;
            this.isInSandstorm = isInSandstorm;
        }
    }
}
