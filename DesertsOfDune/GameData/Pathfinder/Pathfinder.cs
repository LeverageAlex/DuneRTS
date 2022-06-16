using System;
using System.Collections.Generic;
using System.Linq;
using GameData.network.util.world;

namespace GameData.Pathfinder
{
    /// <summary>
    /// Represents the path finder for determining the shortest path in a graph
    /// </summary>
    public class AStarPathfinder
    {
        public AStarPathfinder()
        {
        }

        /// <summary>
        /// find the shortest path from start node to target node, given a certain graph (assumptiom: both nodes are in this graph)
        /// using the A* algorithm
        /// </summary>
        /// <param name="startNode">the node, from which the search starts</param>
        /// <param name="targetNode">the end node, where the path should end</param>
        /// <param name="graph">the graph, the algorithm is working on</param>
        /// <returns>the list of all nodes, that are on the shortest path from start node to target node</returns>
        /// <see href="https://en.wikipedia.org/wiki/A*_search_algorithm"/>
        public Queue<MapField> GetShortestPath(MapField startNode, MapField targetNode, Graph graph)
        {
            // initialize all variables
            HashSet<MapField> openSet = new HashSet<MapField>();
            openSet.Add(startNode);

            Dictionary<MapField, MapField> cameFrom = new Dictionary<MapField, MapField>();

            Dictionary<MapField, double> gScore = new Dictionary<MapField, double>();
            gScore.Add(startNode, 0);

            Dictionary<MapField, double> fScore = new Dictionary<MapField, double>();
            fScore.Add(startNode, H(startNode, targetNode));

            // as long, as there are nodes, that are not finished
            while (openSet.Count != 0)
            {
                MapField current = GetNodeWithLowestFScore(openSet, fScore);

                if (current.Equals(targetNode))
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);

                List<MapField> neighb = graph.GetNeighborsOfVertex(current);
                foreach (MapField neighbor in neighb)
                {
                    double tentativeGScore = gScore.GetValueOrDefault(current) + graph.GetWeigthOfEdge(current, neighbor);
                    if (tentativeGScore < gScore.GetValueOrDefault(current))
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + H(neighbor, targetNode);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return new Queue<MapField>();
        }

        /// <summary>
        /// represents the heuristic function for estimating the cost from a node to a target node
        /// </summary>
        /// <param name="startNode">first node</param>
        /// <param name="targetNode">second node</param>
        /// <returns>the estimated cost to move from start node to target node</returns>
        private double H(MapField startNode, MapField targetNode)
        {
            return Math.Sqrt((startNode.XCoordinate - targetNode.XCoordinate) ^ 2 + (startNode.ZCoordinate - targetNode.ZCoordinate) ^ 2);
        }

        /// <summary>
        /// retrieves the field with the lowest f score from all fields in the openset
        /// </summary>
        /// <param name="openSet">set of all unfinished fields</param>
        /// <param name="fScore">the dictionary, which map the fields and their f scores</param>
        /// <returns>the field with the lowest f score</returns>
        private MapField GetNodeWithLowestFScore(HashSet<MapField> openSet, Dictionary<MapField, double> fScore)
        {
            MapField fieldWithLowestScore = openSet.First();
            double bestScore = fScore.GetValueOrDefault(fieldWithLowestScore);

            foreach (MapField field in openSet)
            {
                if (fScore.GetValueOrDefault(field) < bestScore)
                {
                    // new map field with a better fscore found
                    fieldWithLowestScore = field;
                    bestScore = fScore.GetValueOrDefault(field);
                }
            }

            return fieldWithLowestScore;
        }

        /// <summary>
        /// reconstructes the path from the start node (= current) to the end node after the A* algorithm finished
        /// </summary>
        /// <param name="cameFrom">mapping of successor field for every field</param>
        /// <param name="current">the start node of the path</param>
        /// <returns></returns>
        private Queue<MapField> ReconstructPath(Dictionary<MapField, MapField> cameFrom, MapField current)
        {
            Queue<MapField> path = new Queue<MapField>();
            path.Enqueue(current);

            while (cameFrom.Keys.Contains(current))
            {
                current = cameFrom.GetValueOrDefault(current);
                path.Enqueue(current);
            }

            return path;
        }
    }
}
