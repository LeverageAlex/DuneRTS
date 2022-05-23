using System;
using GameData.network.util.world;
/// <summary>
/// This class represents a adjacency matrix of the playingField
/// </summary>
public class Graph
{
	private readonly int[,] node;
	public int[,] Node { get { return node; } }
	private readonly int size;

	/// <summary>
	/// private Constructor of the class Grap
	/// </summary>
	/// <param name="size">the amounts of fields in the adjacency matrix</param>
	private Graph(int size)
	{
		if (size <= 0)
		{
			return;
		}
		this.node = new int[size, size];
		this.size = size;
	}

	/// <summary>
	/// This method converts mapfield Indizes to a vertex
	/// </summary>
	/// <param name="xCoordinate"></param>
	/// <param name="zCoordinate"></param>
	/// <param name="mapFields"></param>
	/// <returns></returns>
	public static int ConvertArrayIndexToVertex(int xCoordinate, int zCoordinate, MapField[,] mapFields)
    {
		return xCoordinate + zCoordinate*mapFields.GetLength(1);
    }

	public static int ConvertVertexToXArrayIndex(int vertex, MapField[,] mapFields)
    {
		return vertex % mapFields.GetLength(1);
    }

	public static int ConvertVertexToZArrayIndex(int vertex, MapField[,] mapFields)
	{
		return vertex / mapFields.GetLength(1);
	}


	/// <summary>
	/// This method determines a adjacency matrix for the Sandworm to orientate on the map
	/// </summary>
	/// <param name="mapFields">the map</param>
	/// <returns></returns>
	public static Graph DetermineSandWormGraph(MapField[,] mapFields)
    {
		Graph graph = new Graph(mapFields.GetLength(0) * mapFields.GetLength(1));
		Console.Write(graph.size);
		int index = 0;
			for (int i = 0; i < mapFields.GetLength(0); i++)
			{
				for (int j = 0; j < mapFields.GetLength(1); j++)
				{
					if (i + 1 < mapFields.GetLength(0) && mapFields[i + 1, j].TileType == "DUNE" || mapFields[i, j].TileType == "FLAT")
					{
						if (index + 1 < mapFields.GetLength(0) * mapFields.GetLength(1))
						{
							graph.addEdge(index, index + 1);
						}
					}
					if (i + j * i - 1 >= 0 && mapFields[i - 1, j].TileType == "DUNE" || mapFields[i, j].TileType == "FLAT")
					{
						if(index - 1 >= 0)
						{
							graph.addEdge(index, index-1);
						}
					}
					if (j + 1 < mapFields.GetLength(1) && mapFields[i, j + 1].TileType == "DUNE" || mapFields[i, j].TileType == "FLAT")
					{
						if (index + mapFields.GetLength(1) < 100)
						{
							graph.addEdge(index, index + mapFields.GetLength(1));
						}

					}
					if (j - 1 >= 0 && mapFields[i, j - 1].TileType == "DUNE" || mapFields[i, j].TileType == "FLAT")
					{
						if (index - mapFields.GetLength(1) >= 0)
						{
							graph.addEdge(index, index - mapFields.GetLength(1));
						}
					}
					index++;
				}
			}
		return graph;
    }
	/// <summary>
	/// This method adds a entry to the adjacency matrix
	/// </summary>
	/// <param name="start"></param>
	/// <param name="end"></param>
	public void addEdge(int start, int end)
	{
		if (this.size > start && this.size > end)
		{
			this.node[start, end] = 1;
		}
	}
	/// <summary>
	/// This method represents the adjacency matrix as String
	/// </summary>
	public void adjacencyNode()
	{
		if (this.size > 0)
		{
			for (var row = 0; row < this.size; row++)
			{
				Console.Write("Adjacency Matrix of vertex " + row.ToString() + " :");
				for (var col = 0; col < this.size; col++)
				{
					if (this.node[row, col] == 1)
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