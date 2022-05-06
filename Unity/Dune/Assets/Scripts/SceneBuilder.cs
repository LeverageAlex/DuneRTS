using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{

    public GameObject cityNodePrefab, duneNodePrefab, FlatDuneNodePrefab, FlatRockNodePrefab, rockNodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
        initMap();
        createEnemys();

    }


    public void initMap()
    {
        int gridSizeX = 11;
        int gridSizeZ = 11;
        Node[] nodes = new Node[gridSizeZ * gridSizeX];
        for (int x = 0; x < gridSizeZ; x++)
        {
            for (int z = 0; z < gridSizeX; z++)
            {
                if(z + x * gridSizeZ == 7 || x*gridSizeZ + z == gridSizeZ * gridSizeX - 7)
                {
                    nodes[z + x * gridSizeZ] = (Node)(Instantiate(cityNodePrefab, new Vector3(x, 0, z), Quaternion.identity)).GetComponent(typeof(Node));
                }
                else if((z+x* gridSizeZ) % 5 == 0)
                {
                    nodes[z + x * gridSizeZ] = (Node)(Instantiate(rockNodePrefab, new Vector3(x, 0, z), Quaternion.identity)).GetComponent(typeof(Node));
                }
                else if((z + x * gridSizeZ) % 7 == 0)
                {
                    nodes[z + x * gridSizeZ] = (Node)(Instantiate(duneNodePrefab, new Vector3(x, 0, z), Quaternion.identity)).GetComponent(typeof(Node));
                }
                else if((z + x * gridSizeZ) % 3 == 0)
                {
                    nodes[z + x * gridSizeZ] = (Node)(Instantiate(FlatRockNodePrefab, new Vector3(x, 0, z), Quaternion.identity)).GetComponent(typeof(Node));
                }

                else
                {
                    nodes[z + x * gridSizeZ] = (Node)(Instantiate(FlatDuneNodePrefab, new Vector3(x, 0, z), Quaternion.identity)).GetComponent(typeof(Node));
                }


            }



        }

        NodeManager.instance.SetMap(nodes);


    }

    public void createEnemys()
    {

        CharacterMgr.instance.spawnCharacter(1, CharTypeEnum.NOBLE, 0, 0, 100, 30, 10, 10, 10, 10, false, false);
        CharacterMgr.instance.spawnCharacter(2, CharTypeEnum.BENEGESSERIT, 0, 2, 100, 30, 10, 10, 10, 10, false, false);
        CharacterMgr.instance.spawnCharacter(3, CharTypeEnum.FIGHTER, 0, 4, 100, 30, 10, 10, 10, 10, false, false);
        CharacterMgr.instance.spawnCharacter(4, CharTypeEnum.MENTANT, 0, 6, 100, 30, 10, 10, 10, 10, false, false);
    }

 
}
