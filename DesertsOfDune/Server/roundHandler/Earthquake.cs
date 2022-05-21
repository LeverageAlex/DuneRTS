using GameData.network.util.world;
using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.enums;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles earthQuakes
    /// </summary>
    public class EarthQuake
    {
        private MapField[,] fields;

        /// <summary>
        /// Constructor of the class EarthQuake
        /// </summary>
        /// <param name="fields">the playingfield as a collection of fields</param>
        public EarthQuake(MapField[,] fields)
        {
            this.fields = fields;
        }

        /// <summary>
        /// This method transformes Tock planes
        /// </summary>
        /// <returns>true, if transformation was possible</returns>
        public bool TransformRockPlanes()
        {
            RemoveSandworm();
            foreach (MapField field in fields)
            {
                if (field.TileType == Enum.GetName(typeof(TileType), TileType.MOUNTAIN) || field.TileType == Enum.GetName(typeof(TileType), TileType.PLATEAU))
                {
                    field.TileType = Enum.GetName(typeof(TileType), TileType.DUNE);
                }
            }
            return false;
        }

        /// <summary>
        /// This method remoes the sandworm if it exists
        /// </summary>
        /// <returns>true, if the Sandworm was removed</returns>
        public bool RemoveSandworm()
        {
            // TODO: implement logic
            return false;
        }
    }
}
