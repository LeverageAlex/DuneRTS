using System;
using GameData.network.util.enums;
using GameData.network.util.world;
using Serilog;

namespace GameData.graph
{
    /// <summary>
    /// Represents a adjacency matrix of the map
    /// </summary>
    public class Graph
    {
        public int[,] Nodes { get; }
        private readonly int _size;

        /// <summary>
        /// private Constructor of the class Graph
        /// </summary>
        /// <param name="size">the amounts of fields in the adjacency matrix</param>
        private Graph(int size)
        {
            if (size <= 0)
            {
                Log.Error("A graph must have a size (= number of vertices) greater than zero.");
                return;
            }
            else
            {
                this._size = size;
                this.Nodes = new int[size, size];
            }
        }

        /// <summary>
        /// converts mapfield indizes to a vertex
        /// </summary>
        /// <param name="xCoordinate">the x-coordinate</param>
        /// <param name="zCoordinate">the z-coordinate</param>
        /// <param name="mapFields">the whole map</param>
        /// <returns>the position represented as a vertex</returns>
        public static int ConvertArrayIndexToVertex(int xCoordinate, int zCoordinate, Map map)
        {
            return xCoordinate * map.MAP_WIDTH + zCoordinate;
        }

        /// <summary>
        /// determines the fitting x-coordinate to the vertex
        /// </summary>
        /// <param name="vertex">the vertex to determine the coordinate to</param>
        /// <param name="map">the whole map</param>
        /// <returns>the determined x-coordinate</returns>
        public static int ConvertVertexToXArrayIndex(int vertex, Map map)
        {
            return vertex / map.MAP_WIDTH;
        }

        /// <summary>
        /// determines the fitting z-coordinate to the vertex
        /// </summary>
        /// <param name="vertex">the vertex to determine the coordinate to</param>
        /// <param name="map">the whole map</param>
        /// <returns>the determined z-coordinate</returns>
        public static int ConvertVertexToZArrayIndex(int vertex, Map map)
        {
            return vertex % map.MAP_WIDTH;
        }


        /// <summary>
        /// determines a adjacency matrix for the Sandworm to orientate on the map
        /// </summary>
        /// <param name="map">the map to convert to graph</param>
        /// <returns>adjacency matrix of the map</returns>
        public static Graph DetermineSandWormGraph(Map map)
        {
            Graph graph = new Graph(map.MAP_HEIGHT * map.MAP_WIDTH);

            int index = 0;
            for (int i = 0; i < map.MAP_HEIGHT; i++)
            {
                for (int j = 0; j < map.MAP_WIDTH; j++)
                {
                    if (i + 1 < map.MAP_HEIGHT && map.GetMapFieldAtPosition(i+1,j).tileType == TileType.DUNE.ToString() || map.GetMapFieldAtPosition(i, j).tileType == TileType.FLAT_SAND.ToString())
                    {
                        if (index + 1 < map.MAP_HEIGHT * map.MAP_WIDTH)
                        {
                            graph.AddEdge(index, index + 1);
                        }
                    }
                    if (i + j * i - 1 >= 0 && map.GetMapFieldAtPosition(i - 1, j).tileType == TileType.DUNE.ToString() || map.GetMapFieldAtPosition(i, j).tileType == TileType.FLAT_SAND.ToString())
                    {
                        if (index - 1 >= 0)
                        {
                            graph.AddEdge(index, index - 1);
                        }
                    }
                    if (j + 1 < map.MAP_WIDTH && map.GetMapFieldAtPosition(i, j+1).tileType == TileType.DUNE.ToString() || map.GetMapFieldAtPosition(i, j).tileType == TileType.FLAT_SAND.ToString())
                    {
                        if (index + map.MAP_WIDTH < 100)
                        {
                            graph.AddEdge(index, index + map.MAP_WIDTH);
                        }

                    }
                    if (j - 1 >= 0 && map.GetMapFieldAtPosition(i, j-1).tileType == TileType.DUNE.ToString() || map.GetMapFieldAtPosition(i, j).tileType == TileType.FLAT_SAND.ToString())
                    {
                        if (index - map.MAP_WIDTH >= 0)
                        {
                            graph.AddEdge(index, index - map.MAP_WIDTH);
                        }
                    }
                    index++;
                }
            }
            return graph;
        }

        /// <summary>
        /// adds a entry to the adjacency matrix
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void AddEdge(int start, int end)
        {
            if (this._size > start && this._size > end)
            {
                this.Nodes[start, end] = 1;
            }
        }
        /// <summary>
        /// represents the adjacency matrix as String
        /// </summary>
        /// TODO: remove
        public void adjacencyNode()
        {
            if (this._size > 0)
            {
                for (var row = 0; row < this._size; row++)
                {
                    Console.Write("Adjacency Matrix of vertex " + row.ToString() + " :");
                    for (var col = 0; col < this._size; col++)
                    {
                        if (this.Nodes[row, col] == 1)
                        {
                            Console.Write(" " + col.ToString());
                        }
                    }
                    Console.Write("\n");
                }
            }
            else
            {
                Console.WriteLine("Empty Graph");
            }
        }
    }
}
