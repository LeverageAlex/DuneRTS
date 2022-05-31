using GameData.network.util.world;
using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.enums;
using GameData.network.util.world.mapField;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Handler for earthquakes
    /// </summary>
    public class EarthQuakeExecutor
    {
        private readonly Map _map;

        /// <summary>
        /// create a new earthquake handler
        /// </summary>
        /// <param name="map">the map, the earthquake is executed on</param>
        public EarthQuakeExecutor(Map map)
        {
            this._map = map;
        }

        /// <summary>
        /// transformes rock planes to dunes
        /// </summary>
        /// <returns>true, if transformation was possible</returns>
        public bool TransformRockPlanes()
        {
            for (int x = 0; x < this._map.MAP_WIDTH; x++)
            {
                for (int y = 0; y < this._map.MAP_HEIGHT; y++)
                {
                    MapField field = this._map.GetMapFieldAtPosition(x, y);
                    if (field.TileType.Equals(TileType.MOUNTAINS.ToString()) || field.TileType.Equals(TileType.PLATEAU.ToString())){
                        MapField newField = new Dune(field.HasSpice, field.isInSandstorm, field.stormEye);
                        newField.PlaceCharacter(field.Character);
                        this._map.SetMapFieldAtPosition(newField, x, y);
                    }
                }
            }
            return true;
        }
    }
}
