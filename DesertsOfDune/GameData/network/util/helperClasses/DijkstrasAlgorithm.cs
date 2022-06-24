/**
using System;
using System.Collections.Generic;

namespace GameData.graph
{
    /// <summary>
    /// implements Dijkstras algorithm for finding the shortest path between two map fields
    /// </summary>
    public class DijkstrasAlgorithm
    {

        private static readonly int NO_PARENT = -1;

        // Function that implements Dijkstra's
        // single source shortest path
        // algorithm for a graph represented
        // using adjacency matrix
        // representation
        public static int[] Dijkstra(int[,] adjacencyMatrix, int startVertex)
        {
            int nVertices = adjacencyMatrix.GetLength(0);
            int[] shortestDistances = new int[nVertices];
            bool[] added = new bool[nVertices];
            for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
            {
                shortestDistances[vertexIndex] = int.MaxValue;
                added[vertexIndex] = false;
            }
            shortestDistances[startVertex] = 0;
            int[] parents = new int[nVertices];
            parents[startVertex] = NO_PARENT;
            for (int i = 1; i < nVertices; i++)
            {
                int nearestVertex = -1;
                int shortestDistance = int.MaxValue;
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++)
                {
                    if (!added[vertexIndex] &&
                        shortestDistances[vertexIndex] <
                        shortestDistance)
                    {
                        nearestVertex = vertexIndex;
                        shortestDistance = shortestDistances[vertexIndex];
                    }
                }
                added[nearestVertex] = true;
                for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
                {
                    int edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];

                    if (edgeDistance > 0 && ((shortestDistance + edgeDistance) < shortestDistances[vertexIndex]))
                    {
                        parents[vertexIndex] = nearestVertex;
                        shortestDistances[vertexIndex] = shortestDistance + edgeDistance;
                    }
                }
            }

            printSolution(startVertex, shortestDistances, parents);
            return parents;
        }

        public static int[] DijkstraDistances(int[,] adjacencyMatrix, int startVertex)
        {
            int nVertices = adjacencyMatrix.GetLength(0);
            int[] shortestDistances = new int[nVertices];
            bool[] added = new bool[nVertices];
            for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
            {
                shortestDistances[vertexIndex] = int.MaxValue;
                added[vertexIndex] = false;
            }
            shortestDistances[startVertex] = 0;
            int[] parents = new int[nVertices];
            parents[startVertex] = NO_PARENT;
            for (int i = 1; i < nVertices; i++)
            {
                int nearestVertex = -1;
                int shortestDistance = int.MaxValue;
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++)
                {
                    if (!added[vertexIndex] &&
                        shortestDistances[vertexIndex] <
                        shortestDistance)
                    {
                        nearestVertex = vertexIndex;
                        shortestDistance = shortestDistances[vertexIndex];
                    }
                }
                added[nearestVertex] = true;
                for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
                {
                    int edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];

                    if (edgeDistance > 0 && ((shortestDistance + edgeDistance) < shortestDistances[vertexIndex]))
                    {
                        parents[vertexIndex] = nearestVertex;
                        shortestDistances[vertexIndex] = shortestDistance + edgeDistance;
                    }
                }
            }

            printSolution(startVertex, shortestDistances, parents);
            return shortestDistances;
        }

        private static void printSolution(int startVertex, int[] distances, int[] parents)
        {
            int nVertices = distances.Length;
            Console.Write("Vertex\t Distance\tPath");

            for (int vertexIndex = 0;
                    vertexIndex < nVertices;
                    vertexIndex++)
            {
                if (vertexIndex != startVertex)
                {
                    // Console.Write("\n" + startVertex + " -> ");
                    //Console.Write(vertexIndex + " \t\t ");
                    //Console.Write(distances[vertexIndex] + "\t\t");
                    //printPath(vertexIndex, parents);
                }
            }
        }

        private static void printPath(int currentVertex, int[] parents)
        {
            if (currentVertex == NO_PARENT)
            {
                return;
            }
            printPath(parents[currentVertex], parents);
            Console.Write(currentVertex + " ");
        }

        /// <summary>
        /// This method determines the first vertex to move towards for the specified targetVertex
        /// </summary>
        /// <param name="parent">the parent vetexes</param>
        /// <param name="targetVertex">the target vertex</param>
        /// <returns></returns>
        public static int GetFirstStep(int[] parent, int targetVertex)
        {
            int next = parent[targetVertex];
            List<int> list = new List<int>();
            while (true)
            {
                list.Add(next);
                next = parent[next];
                if (next == -1)
                {
                    break;
                }
            }
            if (list.Count - 2 >= 0)
            {
                Console.WriteLine("next vertex: " + list[list.Count - 2]);
                return list[list.Count - 2];
            }
            Console.WriteLine("next vertex: " + list[list.Count - 1]);
            return list[list.Count - 1];
        }
    }
}
**/
