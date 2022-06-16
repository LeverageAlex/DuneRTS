using System;
using System.Collections.Generic;
using GameData.network.util.world;

namespace GameData.Pathfinder
{
    public class Graph
    {
        internal Dictionary<MapField, List<MapField>> adjacencyList;

        /// <summary>
        /// creates a graph
        /// </summary>
        public Graph()
        {
            this.adjacencyList = new Dictionary<MapField, List<MapField>>();
        }

        /// <summary>
        /// retrieves a list of all neighbors of a vertex
        /// </summary>
        /// <param name="field">vertex, whose neighbors are requested</param>
        /// <returns>a list of all neighbors or an empty list, if the vertex has no neighbors</returns>
        public List<MapField> GetNeighborsOfVertex(MapField field)
        {
            return adjacencyList.GetValueOrDefault(field, new List<MapField>());
        }

        /// <summary>
        /// get the weight of an edge between verticies field1 and field2 
        /// </summary>
        /// <remarks>
        /// because the is build up from cubes, the weigth / distance between to neighbor fields is
        /// constantly 1
        /// </remarks>
        /// <param name="field1">vertex 1</param>
        /// <param name="field2">vertex 2</param>
        /// <returns>1 or Double.MAX_VALUE, if there is no edge between them</returns>
        public double GetWeigthOfEdge(MapField field1, MapField field2)
        {
            List<MapField> neigborsOfField1 = adjacencyList.GetValueOrDefault(field1, new List<MapField>());

            if (neigborsOfField1.Count == 0 | !neigborsOfField1.Contains(field2))
            {
                // there are no connection between these verticies
                return double.MaxValue;
            } else
            {
                return 1;
            }
        }
    }
}
