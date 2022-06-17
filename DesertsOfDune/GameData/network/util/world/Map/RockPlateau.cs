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
        public RockPlateau(bool hasSpice, bool isInSandstorm) : base(enums.TileType.PLATEAU, enums.Elevation.low, hasSpice, isInSandstorm, true)
        {

        }
    }
}
