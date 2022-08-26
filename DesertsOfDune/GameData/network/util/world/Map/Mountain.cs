using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.mapField
{
    /// <summary>
    /// This class represents the MapField Mountain
    /// </summary>
    public class Mountain : MapField
    {
        /// <summary>
        /// Constructor of the class Mountain
        /// </summary>
        /// <param name="hasSpice">tells weather there is spice on the MapField or not</param>
        /// <param name="isInSandstorm">tells weather the field is in a Sandstorm or not</param>
        public Mountain(bool hasSpice, bool isInSandstorm) : base(enums.TileType.MOUNTAINS, enums.Elevation.high, hasSpice, isInSandstorm, false)
        {

        }
    }
}
