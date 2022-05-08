using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{

    //public GameObject cityNodePrefab, duneNodePrefab, FlatDuneNodePrefab, FlatRockNodePrefab, rockNodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
        initMap();
        createEnemys();

    }


    public void initMap()
    {
        NodeManager nodeManager = NodeManager.instance;
        int gridSizeX = 11;
        int gridSizeZ = 11;
        nodeManager.setMapSize(gridSizeX, gridSizeZ);
        for (int x = 0; x < gridSizeZ; x++)
        {
            for (int z = 0; z < gridSizeX; z++)
            {
                if(z + x * gridSizeZ == 7 || x*gridSizeZ + z == gridSizeZ * gridSizeX - 7)
                {
                    nodeManager.UpdateBoard(x, z, false, NodeTypeEnum.CITY);
                   
                }
                else if((z+x* gridSizeZ) % 5 == 0)
                {
                    nodeManager.UpdateBoard(x, z, false, NodeTypeEnum.ROCK);
                }
                else if((z + x * gridSizeZ) % 7 == 0)
                {
                    nodeManager.UpdateBoard(x, z, false, NodeTypeEnum.DUNE);
                }
                else if((z + x * gridSizeZ) % 3 == 0)
                {
                    nodeManager.UpdateBoard(x, z, false, NodeTypeEnum.FLATROCK);
                }

                else
                {
                    nodeManager.UpdateBoard(x, z, false, NodeTypeEnum.FLATDUNE);
                }


            }



        }


    }

    public void createEnemys()
    {

        CharacterMgr.instance.spawnCharacter(1, CharTypeEnum.NOBLE, 0, 0, 100, 30, 10, 10, 10, 10, false, false);
        CharacterMgr.instance.spawnCharacter(2, CharTypeEnum.BENEGESSERIT, 0, 2, 100, 30, 10, 10, 10, 10, false, false);
        CharacterMgr.instance.spawnCharacter(3, CharTypeEnum.FIGHTER, 0, 4, 100, 30, 10, 10, 10, 10, false, false);
        CharacterMgr.instance.spawnCharacter(4, CharTypeEnum.MENTANT, 0, 6, 100, 30, 10, 10, 10, 10, false, false);
    }

 
}
