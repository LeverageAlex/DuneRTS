using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int _gridSizeX = 11;
    private static int _gridSizeZ = 11;

    public static Node[] field;

    //Getters


    public Node[] nodes;
    // Start is called before the first frame update
    void Awake()
    {
        field = nodes;
    }

  
    public static Node getNodeRightFrom(Node node)
    {
        return getNodeFromPos((int)(node.transform.position.x) + 1, (int)(node.transform.position.z));
    }

    public static Node getNodeLeftFrom(Node node)
    {
        return getNodeFromPos((int) (node.transform.position.x)-1, (int)(node.transform.position.z));
    }

    public static Node getNodeUpFrom(Node node)
    {
        return getNodeFromPos((int)(node.transform.position.x), (int)Mathf.Round(node.transform.position.z)+1);
    }

    public static Node getNodeDownFrom(Node node)
    {
        return getNodeFromPos((int)(node.transform.position.x), (int)(node.transform.position.z) - 1);
    }


    public static Node getNodeFromPos(int x, int z)
    {
        if(z + GridSizeZ * x < 0 || z + GridSizeZ * x >= field.Length) { return null; }
        return field[z + GridSizeZ * x];
    }



    public static int GridSizeX { get { return _gridSizeX; } }
    public static int GridSizeZ { get { return _gridSizeZ; } }
}
