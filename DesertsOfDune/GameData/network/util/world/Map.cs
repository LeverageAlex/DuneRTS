using System;
using System.Collections.Generic;
using System.Linq;
using GameData.network.util.enums;
using GameData.network.util.world.mapField;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents the map of the game (based on the scenario, but with further information)
    /// </summary>
    public class Map
    {
        private MapField[,] fields;

        private readonly int MAP_WIDTH;
        private readonly int MAP_HEIGHT;

        public Map(int mapWidth, int mapHeight, List<List<string>> scenarioConfiguration)
        {
            this.MAP_WIDTH = mapWidth;
            this.MAP_HEIGHT = mapHeight;

            // check, that the scenarionConfiguration has the correct sizes
            CreateMapFromScenario(scenarioConfiguration);
        }

        private void CreateMapFromScenario(List<List<string>> scenarioConfiguration)
        {
            fields = new MapField[MAP_HEIGHT, MAP_WIDTH];
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    fields[y, x] = new MapField(scenarioConfiguration[y][x]);
                }
            }
        }

        /// <summary>
        /// return a list of all neighbor map fields of this field
        /// </summary>
        /// <remarks>
        /// A neighbor map field is a field on the map, whose distance d = sqrt((x1 - x2)^2 + (y1-y2)^2) to the field is d <= 1.
        /// </remarks>
        /// <param name="field">the map field, whose neighbors will be determined</param>
        /// <returns>a list of all map fields, that are neighbors of the field</returns>
        public List<MapField> GetNeighborFields(MapField field)
        {
            List<MapField> neighbors = new List<MapField>();

            for (int dx = -1; dx <= -1; dx++)
            {
                for (int dy = -1; dy <= -1; dy++)
                {
                    // if dx = dy = 0, the field is the field itself and not a neighbor
                    if (dx != 0 || dy != 0)
                    {
                        int newX = field.XCoordinate + dx;
                        int newY = field.ZCoordinate + dy;
                        // check, if the neighbor field is on the map
                        if (IsFieldOnMap(newX, newY))
                        {
                            // add neighbor field to the list of neighbors
                            neighbors.Add(GetMapFieldAtPosition(newX, newY));
                        }
                    }
                }
            }

            return neighbors;
        }

        /// <summary>
        /// checks, wether a field specified through its x- and y-coordinate on the map, so whether the coordinates are x,y > 0 and x,y < maxX, maxY
        /// </summary>
        /// <param name="x"><the x-coordinate of the mapfield/param>
        /// <param name="y">the y-coordinate of the mapfield</param>
        /// <returns></returns>
        private bool IsFieldOnMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < MAP_WIDTH && y < MAP_HEIGHT;
        }

        /// <summary>
        /// return the reference to a map field at a given position in the map
        /// </summary>
        /// <param name="x">the x-coordinate of the requested map field</param>
        /// <param name="y">the y-coordinate of the requested map field</param>
        /// <returns>the map field at the given position or null, if the position in not valid</returns>
        /// TODO: do not return null
        public MapField GetMapFieldAtPosition(int x, int y)
        {
            if (IsFieldOnMap(x, y))
            {
                return this.fields[y,x];
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// gets a list of all cities on the map
        /// </summary>
        /// <returns>a list of all cities on the map</returns>
        public List<City> GetCitiesOnMap()
        {
            List<City> cities = new List<City>();
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    if (fields[y, x].TileType == TileType.CITY.ToString())
                    {
                        cities.Add((City)fields[y, x]);
                    }
                }
            }

            return cities;
        }
    }
}
