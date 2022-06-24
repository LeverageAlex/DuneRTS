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

        public static Map instance;

        public int MAP_WIDTH { get; }
        public int MAP_HEIGHT { get; }

        public Position PositionOfEyeOfStorm { get; set; }

        public Map(int mapWidth, int mapHeight, List<List<string>> scenarioConfiguration)
        {
            this.MAP_WIDTH = mapWidth;
            this.MAP_HEIGHT = mapHeight;
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Console.WriteLine("Error. There is more than one map. This doesn't make any sense!");
            }

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
                    switch (scenarioConfiguration[x][y])
                    {
                        case "DUNE":
                            fields[y, x] = new Dune(false, false);
                            break;
                        case "FLAT_SAND":
                            fields[y, x] = new FlatSand(false, false);
                            break;
                        case "MOUNTAINS":
                            fields[y, x] = new Mountain(false, false);
                            break;
                        case "PLATEAU":
                            fields[y, x] = new RockPlateau(false, false);
                            break;
                        case "CITY":
                            fields[y, x] = new City(0, false, false);
                            break;
                        case "HELIPORT":
                            fields[y, x] = new Heliport(false, false);
                            break;
                    }
                    fields[y, x].XCoordinate = x;
                    fields[y, x].ZCoordinate = y;
                    //  fields[y, x] = new MapField(scenarioConfiguration[x][(MAP_HEIGHT - 1) - y], x, y);
                }
            }
        }

        public MapField[,] GetNewMapFromMessage(MapField[,] mapFields)
        {
            MapField[,] newMap = new MapField[MAP_HEIGHT, MAP_WIDTH];

            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    MapField mapField = mapFields[y, x];
                    // MapField mapField = GetMapFieldAtPosition(x, y);

                    switch (mapField.tileType)
                    {
                        case "DUNE":
                            newMap[y, x] = new Dune(mapField.hasSpice, mapField.isInSandstorm);
                            break;
                        case "FLAT_SAND":
                            newMap[y, x] = new FlatSand(mapField.hasSpice, mapField.isInSandstorm);
                            break;
                        case "MOUNTAINS":
                            newMap[y, x] = new Mountain(mapField.hasSpice, mapField.isInSandstorm);
                            break;
                        case "PLATEAU":
                            newMap[y, x] = new RockPlateau(mapField.hasSpice, mapField.isInSandstorm);
                            break;
                        case "CITY":
                            newMap[y, x] = new City(mapField.clientID, mapField.hasSpice, mapField.isInSandstorm);
                            break;
                    }
                    newMap[y, x].XCoordinate = x;
                    newMap[y, x].ZCoordinate = y;

                    if (GetMapFieldAtPosition(x, y).Character != null && !GetMapFieldAtPosition(x, y).Character.KilledBySandworm)
                    {
                        newMap[y, x].PlaceCharacter(GetMapFieldAtPosition(x, y).Character);
                    }

                }
            }
            return newMap;
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
            }
            else
            {
                int index = random.Next(amountOfNeighbors);
                return neighbors[index];
            }
        }

        /// <summary>
        /// retrieves a random, approachble neighbor field of a given map field, on which no character stand already
        /// </summary>
        /// <param name="field">the field, of which a random approachble, free neighbor field should be determined</param>
        /// <returns>the random neighbor field or null, if there doesn't exist such a field</returns>
        /// TODO: do not return null
        public MapField GetRandomFreeApproachableNeighborField(MapField field)
        {
            Random random = new Random();
            List<MapField> neighbors = GetNeighborFields(field);

            neighbors.RemoveAll(neighbor => !neighbor.IsApproachable || neighbor.IsCharacterStayingOnThisField);

            int amountOfNeighbors = neighbors.Count;

            if (amountOfNeighbors == 0)
            {
                return null;
            }
            else
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
        public bool IsFieldOnMap(int x, int y)
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
                return this.fields[y, x];
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
                this.fields[y, x] = newField;
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
                    if (fields[y, x].tileType.Equals(Enum.GetName(typeof(TileType), TileType.CITY)))
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
            return mapField.tileType.Equals(TileType.FLAT_SAND.ToString()) || mapField.tileType.Equals(TileType.DUNE.ToString());
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
        /// gets a random field from the map, where no character is standing on
        /// </summary>
        /// <remarks>
        /// Be cautious when using this method, because it uses a while (true)-loop, so theoretically it is possible, that this method never return,
        /// if there is no desert field on the map
        /// </remarks>
        /// <returns>a random map field, where no character is standing on</returns>
        public MapField GetRandomFieldWithoutCharacter()
        {
            // as long, as the chosen map field has a character standing on

            while (true)
            {
                // get a random map field on the map
                Random random = new Random();
                int randomX = random.Next(MAP_WIDTH);
                int randomY = random.Next(MAP_HEIGHT);

                MapField chosenField = GetMapFieldAtPosition(randomX, randomY);
                if (!chosenField.IsCharacterStayingOnThisField)
                {
                    return chosenField;
                }
            }
        }


        public MapField GetRandomApproachableField()
        {
            // as long, as the chosen map field is approachable, choose a random field

            while (true)
            {
                // get a random map field on the map
                Random random = new Random();
                int randomX = random.Next(MAP_WIDTH);
                int randomY = random.Next(MAP_HEIGHT);

                MapField chosenField = GetMapFieldAtPosition(randomX, randomY);
                if (chosenField.IsApproachable)
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
                    if (GetMapFieldAtPosition(x, y).IsCharacterStayingOnThisField)
                    {
                        builder.Append("  C  ");
                    }
                    else
                    {
                        builder.Append(GetMapFieldAtPosition(x, y).tileType.ToString());
                    }

                    if (GetMapFieldAtPosition(x, y).isInSandstorm)
                    {
                        builder.Append(" x ");
                    }

                    if (GetMapFieldAtPosition(x, y).hasSpice)
                    {
                        builder.Append(" s ");
                    }

                    builder.Append(", ");
                }
                builder.Append("\n");
            }

            builder.Append("\n");

            Log.Information(builder.ToString());
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
                    if (GetMapFieldAtPosition(x, y).hasSpice)
                    {
                        spiceAmount++;
                    }
                }
            }
            return spiceAmount;
        }

        /// <summary>
        /// retrieves all characters on the map
        /// </summary>
        /// <returns></returns>
        public List<Character> GetCharactersOnMap()
        {
            List<Character> characters = new List<Character>();

            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    MapField field = GetMapFieldAtPosition(x, y);
                    if (field.IsCharacterStayingOnThisField)
                    {
                        characters.Add(field.Character);
                    }
                }
            }

            return characters;
        }

        /// <summary>
        /// calculates wheter a sandstorm is on the pathLine between two Points
        /// </summary>
        /// <param name="startField"> point 1 </param>
        /// <param name="targetPosition"> point 2 </param>
        /// <returns></returns>
        public bool HasSandstormOnPath(MapField startField, Position targetPosition)
        {
            bool horicontal = startField.XCoordinate != targetPosition.x;
            bool vertical = startField.ZCoordinate != targetPosition.y;

            //first Point
            float a = startField.XCoordinate;
            float b = startField.ZCoordinate;
            //second Point
            float c = targetPosition.x;
            float d = targetPosition.y;

            //building f(x) = mx * x + tx and f(y) = my * y + ty
            float mx = 0, my = 0, tx = 0, ty = 0;
            if (horicontal)
            {
                mx = (a < b) ? (d - b) / (c - a) : (b - d) / (a - c);//slope of f(x)
                tx = b - mx * a;
            }

            if (vertical)
            {
                my = (b < d) ? (c - a) / (d - b) : (a - c) / (b - d);//slope of f(y)
                ty = a - my * b;
            }

            //check if sandstormFields get cut by the pathLine
            foreach (MapField m in GetSandstormFieldsOnMap())
            {
                if (IsFieldCutByLine(m.XCoordinate, m.ZCoordinate, mx, tx, my, ty, horicontal, vertical)) return true;
            }

            return false;
        }

        /// <summary>
        /// calculates wheter a 1x1-Square with ceter of (x,y) gets cut by a line given trough
        /// f(x) = mx * x + tx and f(y) = my * y + ty
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mx"></param>
        /// <param name="tx"></param>
        /// <param name="my"></param>
        /// <param name="ty"></param>
        /// <param name="horicontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        private bool IsFieldCutByLine(float x, float y, float mx, float tx, float my, float ty, bool horicontal, bool vertical)
        {
            int cuts = 0;

            //checking all 4 edges of the square
            if (horicontal)
            {
                //left edge
                float yleft = (x - 0.5f) * mx + tx;//f(x)
                if (yleft >= (y - 0.5f) && yleft < (y + 0.5f)) cuts++;//checking bounds

                //right edge
                float yright = (x + 0.5f) * mx + tx;//f(x)
                if (yright > (y - 0.5f) && yright <= (y + 0.5f)) cuts++;//checking bounds
            }

            if (vertical)
            {
                //bottom edge
                float xbottom = (y - 0.5f) * my + ty;//f(y)
                if (xbottom > (x - 0.5f) && xbottom <= (x + 0.5f)) cuts++;//checking bounds

                //top edge
                float xtop = (y + 0.5f) * my + ty;//f(y)
                if (xtop >= (x - 0.5f) && xtop < (x + 0.5f)) cuts++;//checking bounds
            }

            return cuts >= 2;
        }

        /// <summary>
        /// returns all MapFields with isInSandstorm = true
        /// </summary>
        /// <returns></returns>
        public List<MapField> GetSandstormFieldsOnMap()
        {
            List<MapField> sandstormFields = new List<MapField>();
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    if (fields[y, x].isInSandstorm)
                    {
                        sandstormFields.Add(fields[y, x]);
                    }
                }
            }
            return sandstormFields;
        }

        /// <summary>
        /// removes all characters from the map (used for tests!)
        /// </summary>
        public void RemoveCharactersFromMap()
        {
            for(int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    MapField field = GetMapFieldAtPosition(x, y);
                    if (field.IsCharacterStayingOnThisField)
                    {
                        field.DisplaceCharacter(field.Character);
                    }
                }
            }
        }
    }
}