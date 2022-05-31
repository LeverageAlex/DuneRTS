using System;
using System.Collections.Generic;
using GameData.network.util.world;

namespace GameData.Pathfinder
{
    public class SandwormGraph : Graph
    {
        public SandwormGraph(Map map) : base()
        {
            CreateGraph(map);
        }

        /// <summary>
        /// converts a map into a adjacency list containing all desert fields and their neighbor fields, which are also desert fields
        /// </summary>
        /// <param name="map">map to convert</param>
        private void CreateGraph(Map map)
        {
            for (int x = 0; x < map.MAP_WIDTH; x++)
            {
                for (int y = 0; y < map.MAP_HEIGHT; y++)
                {
                    MapField field = map.GetMapFieldAtPosition(x, y);
                    if (map.IsMapFieldADesertField(field))
                    {
                        List<MapField> desertFieldNeighbors = new List<MapField>();
                        foreach (MapField neighbor in map.GetNeighborFields(field))
                        {
                           if (map.IsMapFieldADesertField(neighbor))
                            {
                                desertFieldNeighbors.Add(neighbor);
                            }
                        }
                        base.adjacencyList.Add(field, desertFieldNeighbors);
                    }
                }
            }
        }
    }
}
