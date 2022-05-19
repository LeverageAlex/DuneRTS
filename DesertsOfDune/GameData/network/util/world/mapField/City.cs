using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.mapField
{
    /// <summary>
    /// This class represents the MapField City
    /// </summary>
    public class City : MapField
    {

        private int spiceCount;

        /// <summary>
        /// Constructor of the class City
        /// </summary>
        /// <param name="hasSpice">tells weather there is spice on the MapField or not</param>
        /// <param name="isInSandstorm">tells weather the field is in a Sandstorm or not</param>
        /// <param name="stormEye">tells weather the field is in the storm eye or not</param>
        public City(bool hasSpice, bool isInSandstorm, Position stormEye) : base(enums.TileType.CITY, enums.Elevation.low, hasSpice, isInSandstorm, stormEye)
        {

        }

        /// <summary>
        /// this method is used to add spice to the spice of the city
        /// </summary>
        /// <param name="amount">the amount of spice to be added</param>
        /// <returns>true, if action was succesfull</returns>
        public bool AddSpice(int amount)
        {
            this.spiceCount = spiceCount + amount;
            return true;
        }
    }
}
