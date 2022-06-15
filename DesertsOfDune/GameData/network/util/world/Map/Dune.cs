using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.mapField
{
    /// <summary>
    /// This class represents the MapField dune
    /// </summary>
    public class Dune : MapField
    {
        /// <summary>
        /// Constructor of the class Dune
        /// </summary>
        /// <param name="hasSpice">tells weather there is spice on the MapField or not</param>
        /// <param name="isInSandstorm">tells weather the field is in a Sandstorm or not</param>
        /// <param name="stormEye">tells weather the field is in the storm eye or not</param>
        public Dune(bool hasSpice, bool isInSandstorm, Position stormEye) : base(enums.TileType.DUNE, enums.Elevation.low, hasSpice, isInSandstorm, true,stormEye)
        {

        }
    }
}
