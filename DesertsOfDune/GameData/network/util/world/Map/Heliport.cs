using GameData.network.util.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.mapField
{
    public class Heliport : MapField
    {
        public Heliport(bool hasSpice, bool isInSandstorm) : base(TileType.HELIPORT, Elevation.low, hasSpice, isInSandstorm, true)
        {
        }
    }
}
