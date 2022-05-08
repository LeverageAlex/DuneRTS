using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeManager : MonoBehaviour
{

    public static NodeManager instance;

    private int _gridSizeX = 11;
    private int _gridSizeZ = 11;

    public GameObject[] enemiesOnBoard;

    public Node[] nodes;

    public GameObject cityNodePrefab, duneNodePrefab, FlatDuneNodePrefab, FlatRockNodePrefab, rockNodePrefab;

    public GameObject[,] spiceCrumbs;
    public GameObject spicePrefab;



    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManger in scene!");
            return;
        }
        instance = this;
        Debug.Log("Instance set");
        spiceCrumbs = new GameObject[_gridSizeZ, _gridSizeX];
        enemiesOnBoard = new GameObject[nodes.Length];

    }


    public Node getNodeRightFrom(Node node)
    {
        return getNodeFromPos((node.X) + 1, (node.Z));
    }

    public Node getNodeLeftFrom(Node node)
    {
        return getNodeFromPos((node.X) - 1, (node.Z));
    }

    public Node getNodeUpFrom(Node node)
    {
        return getNodeFromPos((node.X), (node.Z) + 1);
    }

    public Node getNodeDownFrom(Node node)
    {
        return getNodeFromPos((node.X), (node.Z) - 1);
    }

    //moore neighbourhood (8 nodes)
    public bool isNodeNeighbour(Node baseNode, Node neighbour)
    {
        if (Mathf.Abs(baseNode.X - neighbour.X) == 1)
        {
            return (Mathf.Abs(baseNode.Z - neighbour.Z) <= 1);
        }
        else if (Mathf.Abs(baseNode.X - neighbour.X) == 0)
        {
            return (Mathf.Abs(baseNode.Z - neighbour.Z) == 1);
        }

        return false;
    }

    public bool isNodeNeighbour(int firstX, int firstZ, int secondX, int secondZ)
    {
        if (Mathf.Abs(firstX - secondX) == 1)
        {
            return (Mathf.Abs(firstZ - secondZ) <= 1);
        }
        else if (Mathf.Abs(firstX - secondX) == 0)
        {
            return (Mathf.Abs(firstZ - secondZ) == 1);
        }

        return false;
    }

    //Getters
    public Node getNodeFromPos(int x, int z)
    {
        if (z + GridSizeZ * x < 0 || z + GridSizeZ * x >= nodes.Length) { return null; }
        return nodes[z + GridSizeZ * x];
    }

    public bool placeObjectOnNode(GameObject obj, int x, int z)
    {
        if (z + GridSizeZ * x < 0 || z + GridSizeZ * x >= nodes.Length)
        {
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
        foreach (Node node in nodes)
        {
            node.ResetColor();
        }
    }


    public void SpawnSpiceCrumOn(int x, float y, int z)
    {
        if (spiceCrumbs[z, x] != null) return;
        GameObject spice = Instantiate(spicePrefab, new Vector3(x, y, z), Quaternion.identity);
        spiceCrumbs[z, x] = spice;

    }

    public bool IsSpiceOn(int x, int z)
    {
        if (x >= 0 && x < _gridSizeX && z >= 0 && z < _gridSizeZ)
        {
            return spiceCrumbs[z, x] != null;
        }
        else return false;
    }

    public void CollectSpice(int x, int z)
    {
        if (spiceCrumbs[z, x] != null)
        {
            Destroy(spiceCrumbs[z, x]);
            spiceCrumbs[z, x] = null;
        }
    }

    public void SetMap(Node[] nodes)
    {
        this.nodes = nodes;
    }

    public void UpdateBoard(int x, int z, bool spiceOnNode, NodeTypeEnum nodeEnum)
    {
        Node currentNode = getNodeFromPos(x, z);
        if (currentNode == null || currentNode.nodeTypeEnum != nodeEnum)
        {
            if (currentNode != null)
            {
                Destroy(currentNode.gameObject);
            }
            GameObject nodePrefab = null;
            switch(nodeEnum)
            {
                case NodeTypeEnum.DUNE: nodePrefab = duneNodePrefab; break;
                case NodeTypeEnum.FLATDUNE: nodePrefab = FlatDuneNodePrefab; break;
                case NodeTypeEnum.ROCK: nodePrefab = rockNodePrefab; break;
                case NodeTypeEnum.FLATROCK: nodePrefab = FlatRockNodePrefab; break;
                case NodeTypeEnum.CITY: nodePrefab = cityNodePrefab; break;
            }
            currentNode = (Node)Instantiate(nodePrefab, new Vector3(x, 0, z), Quaternion.identity).GetComponent(typeof(Node));
            nodes[z + GridSizeZ * x] = currentNode;
        }
        if(spiceOnNode)
        {
            SpawnSpiceCrumOn(x, currentNode.charHeightOffset, z);
        }
        else
        {
            CollectSpice(x, z);
        }
    }

    public void setMapSize(int gridSizeX, int gridSizeZ)
    {
        _gridSizeX = gridSizeX;
        _gridSizeZ = gridSizeZ;
        spiceCrumbs = new GameObject[_gridSizeZ, _gridSizeX];
        nodes = new Node[gridSizeX * gridSizeZ];
        enemiesOnBoard = new GameObject[nodes.Length];
    }



    public int GridSizeX { get { return _gridSizeX; } }
    public int GridSizeZ { get { return _gridSizeZ; } }
}
