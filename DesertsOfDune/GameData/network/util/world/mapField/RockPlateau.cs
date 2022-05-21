using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.mapField
{
    /// <summary>
    /// This class represents the MapField RockPlateau
    /// </summary>
    public class RockPlateau : MapField
    {
        /// <summary>
        /// Constructor of the class RockPlateau
        /// </summary>
        /// <param name="hasSpice">tells weather there is spice on the MapField or not</param>
        /// <param name="isInSandstorm">tells weather the field is in a Sandstorm or not</param>
        /// <param name="stormEye">tells weather the field is in the storm eye or not</param>
        public RockPlateau(bool hasSpice, bool isInSandstorm, Position stormEye) : base(enums.TileType.MOUNTAIN, enums.Elevation.low, hasSpice, isInSandstorm, stormEye)
        {

        }
    }
}
