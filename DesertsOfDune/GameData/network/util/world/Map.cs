using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameData.network.util.enums;
using GameData.network.util.world.mapField;
using Serilog;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents the map of the game (based on the scenario, but with further information)
    /// </summary>
    public class Map
    {
        public MapField[,] fields { get; set; }

        public int MAP_WIDTH { get; }
        public int MAP_HEIGHT { get; }

        public Position PositionOfEyeOfStorm { get; set; }

        public Map(int mapWidth, int mapHeight, List<List<string>> scenarioConfiguration)
        {
            this.MAP_WIDTH = mapWidth;
            this.MAP_HEIGHT = mapHeight;

            // check, that the scenarionConfiguration has the correct sizes
            CreateMapFromScenario(scenarioConfiguration);
        }

        /// <summary>
        /// create a map with detailed information based on the given scenario (only field types)
        /// </summary>
        /// <param name="scenarioConfiguration">the "array" of the field types of the map / scenario</param>
        private void CreateMapFromScenario(List<List<string>> scenarioConfiguration)
        {
            fields = new MapField[MAP_HEIGHT, MAP_WIDTH];
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    fields[y, x] = new MapField(scenarioConfiguration[x][(MAP_HEIGHT - 1) - y], x, y);
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

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
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
        /// retrieves a random, approachble neighbor field of a given map field
        /// </summary>
        /// <param name="field">the field, of which a random approachble neighbor field should be determined</param>
        /// <returns>the random neighbor field or null, if there doesn't exist such a field</returns>
        /// TODO: do not return null
        public MapField GetRandomApproachableNeighborField(MapField field)
        {
            Random random = new Random();
            List<MapField> neighbors = GetNeighborFields(field);

            neighbors.RemoveAll(neighbor => !neighbor.IsApproachable);

            int amountOfNeighbors = neighbors.Count;

            if (amountOfNeighbors == 0)
            {
                return null;
            } else
            {
                int index = random.Next(amountOfNeighbors);
                return neighbors[index];
            }
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
                return this.fields[(MAP_HEIGHT - 1) - y, x];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// sets a new field at a given position in the map and override the existing map tile at this position
        /// </summary>
        /// <param name="newField">the new map field</param>
        /// <param name="x">x-coordinate of the new map field</param>
        /// <param name="y">y-coordinate of the new map field</param>
        /// <returns>true, if the given position was valid, else return false</returns>
        public bool SetMapFieldAtPosition(MapField newField, int x, int y)
        {
            if (IsFieldOnMap(x, y))
            {
                this.fields[(MAP_HEIGHT - 1) - y, x] = newField;
                newField.XCoordinate = x;
                newField.ZCoordinate = y;
                return true;
            }
            else
            {
                return false;
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

        /// <summary>
        /// checks, whether a given map field is a desert field, so of type "DUNE" or "FLAT_SAND"
        /// </summary>
        /// <param name="mapField">the map field, to check</param>
        /// <returns>true, if the given map field is a desert field</returns>
        public bool IsMapFieldADesertField(MapField mapField)
        {
            return mapField.TileType.Equals(TileType.FLAT_SAND.ToString()) || mapField.TileType.Equals(TileType.DUNE.ToString());
        }

        /// <summary>
        /// retrieves a random desert field from the map
        /// </summary>
        /// <remarks>
        /// Be cautious when using this method, because it uses a while (true)-loop, so theoretically it is possible, that this method never return,
        /// if there is no desert field on the map
        /// </remarks>
        /// <returns>the random desert field on the map</returns>
        public MapField GetRandomDesertField()
        {
            // as long, as the chosen map field is not a desert field, choose a random field

            while (true)
            {
                // get a random map field on the map
                Random random = new Random();
                int randomX = random.Next(MAP_WIDTH);
                int randomY = random.Next(MAP_HEIGHT);

                MapField chosenField = GetMapFieldAtPosition(randomX, randomY);
                if (IsMapFieldADesertField(chosenField))
                {
                    return chosenField;
                }
            }
        }

        /// <summary>
        /// prints a map to the console for debugging purpose
        /// </summary>
        public void DrawMapToConsole()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("------------------------------------- \n");

            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    builder.Append(GetMapFieldAtPosition(x, y).TileType.ToString());

                    if (GetMapFieldAtPosition(x, y).isInSandstorm)
                    {
                        builder.Append(" x ");
                    }
                    builder.Append(", ");
                }
                builder.Append("\n");
            }

            builder.Append("\n");

            Log.Debug(builder.ToString());
        }

        /// <summary>
        /// determines the amount of spice, which is on the map by counting the map fields, which has spice on them
        /// </summary>
        /// <returns>the amount of spice on the map</returns>
        public int GetAmountOfSpiceOnMap()
        {
            int spiceAmount = 0;
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    if (GetMapFieldAtPosition(x, y).HasSpice)
                    {
                        spiceAmount++;
                    }
                }
            }
            return spiceAmount;
        }


    }
}