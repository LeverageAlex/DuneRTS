using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{

    public static NodeManager instance;

    private static int _gridSizeX = 11;
    private static int _gridSizeZ = 11;

    public GameObject[] enemiesOnBoard;




    public Node[] nodes;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManger in scene!");
            return;
        }
        instance = this;
        enemiesOnBoard = new GameObject[nodes.Length];
        
    }

  
    public Node getNodeRightFrom(Node node)
    {
        return getNodeFromPos((node.X) + 1, (node.Z));
    }

    public Node getNodeLeftFrom(Node node)
    {
        return getNodeFromPos( (node.X)-1, (node.Z));
    }

    public Node getNodeUpFrom(Node node)
    {
        return getNodeFromPos((node.X), (node.Z)+1);
    }

    public Node getNodeDownFrom(Node node)
    {
        return getNodeFromPos((node.X), (node.Z) - 1);
    }

    public bool isNodeNeighbour(Node baseNode, Node neighbour)
    {
        return Mathf.Abs(baseNode.X - neighbour.X) + Mathf.Abs(baseNode.Z - neighbour.Z) <= 1;
    }

    //Getters
    public Node getNodeFromPos(int x, int z)
    {
        if(z + GridSizeZ * x < 0 || z + GridSizeZ * x >= nodes.Length) { return null; }
        return nodes[z + GridSizeZ * x];
    }

    public bool placeObjectOnNode(GameObject obj, int x, int z)
    {
        if(z + GridSizeZ * x < 0 || z + GridSizeZ * x >= nodes.Length) {
            return false;
        }

        enemiesOnBoard[z + GridSizeZ * x] = obj;
        return true;

    }

    public bool RemoveObjectOnNode(int x, int z)
    {
        if (z + GridSizeZ * x < 0 || z + GridSizeZ * x >= nodes.Length)
        {
            return false;
        }

        enemiesOnBoard[z + GridSizeZ * x] = null;
        getNodeFromPos(x, z).ResetColor();
        return true;

    }

    public bool placeObjectOnNode(GameObject obj, Node node)
    {
        if (node.Z + GridSizeZ * node.X < 0 || node.Z + GridSizeZ * node.X >= nodes.Length)
        {
            return false;
        }

        enemiesOnBoard[node.Z + GridSizeZ * node.X] = obj;
        return true;

    }

    public GameObject getObjectOnNode(Node node)
    {
        return enemiesOnBoard[node.Z + node.X * GridSizeZ];
    }

    public void ResetNodeColors()
    {
        foreach(Node node in nodes)
        {
            node.ResetColor();
        }
    }


    public int GridSizeX { get { return _gridSizeX; } }
    public int GridSizeZ { get { return _gridSizeZ; } }
}
