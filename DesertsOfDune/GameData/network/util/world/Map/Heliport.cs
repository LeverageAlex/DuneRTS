using GameData.network.util.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.mapField
{
    /// <summary>
    /// This class represents the MapField Heliport
    /// </summary>
    public class Heliport : MapField
    {
        /// <summary>
        /// Constructor of the class Heliport
        /// </summary>
        /// <param name="hasSpice">tells weather there is spice on the MapField or not</param>
        /// <param name="isInSandstorm">tells weather the field is in a Sandstorm or not</param>
        public Heliport(bool hasSpice, bool isInSandstorm) : base(TileType.HELIPORT, Elevation.low, hasSpice, isInSandstorm, true)
        {
        }
    }
}
