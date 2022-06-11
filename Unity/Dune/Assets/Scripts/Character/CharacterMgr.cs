using GameData.network.controller;
using GameData.network.util.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * This class provides functionality to maintain characters on the map:
 * - Spawn Chars
 * - Update CharStats
 * - Spawn Sandworm
 * - Move Worm
 * 
 */
public class CharacterMgr : MonoBehaviour
{
    //Connects CharacterID to CharacterObject
    Dictionary<int, Character> characterDict = new Dictionary<int, Character>();

    public GameObject noblePrefab;
    public GameObject mentatPrefab;
    public GameObject beneGesseritPrefab;
    public GameObject fighterPrefab;

    public GameObject atomicPrefab;

    public GameObject sandwormPrefab;
    public GameObject helicopterPrefab;

    private MoveAbles sandwormMoveScript;

    public Sprite emblemRichese;
    public Sprite emblemVernius;
    public Sprite emblemAtreides;
    public Sprite emblemHarkonnen;
    public Sprite emblemOrdos;
    public Sprite emblemCorinno;



    private float charSpawnLowY = 0.35f;
    private float charSpawnHighY = 0.525f;

    private float wormHeightOffset = 0.35f;

    public int clientID;
    public int enemyClientID;
    public string clientSecret;

    private HouseEnum playerHouse;
    private HouseEnum enemyHouse;

    private int nobleMaxHP;
    private int fighterMaxHP;
    private int benneGesseritMaxHP;
    private int mentatMaxHP;

    public ClientConnectionHandler handler;

    public static CharacterMgr instance;






    // Start is called before the first frame update
    void Awake()
    {
        //Example to show Method
        //spawnCharacter(175, "FIGHTER", 15, 15, 4, 10, 4, 4, 3, 6, false, false);
        if(instance == null)
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        /*LinkedList<Vector3> path = new LinkedList<Vector3>();
        path.AddLast(new Vector3(1, 0.6f, 1));
        path.AddLast(new Vector3(8, 0.6f, 9));
        SpawnSandworm(5, 5);
        SandwormMove(path);*/
    }




    /*
     * To be filled after open question regarding standardDocument has ben resolved
  */
    public bool spawnCharacter(int clientId, int characterID, CharTypeEnum type,int x, int z,int HPcurrent, int MPcurrent, int APcurrent, int APMax, int inventorySize, bool killedBySandworm, bool loud)
    {
        if (characterDict.ContainsKey(characterID))
            return false;


        float charSpawnY = MapManager.instance.getNodeFromPos(x,z).heightLvl == Node.HeightLevel.high ? charSpawnHighY : charSpawnLowY;
        GameObject newChar = (GameObject) Instantiate(getCharTypeByEnum(type), new Vector3(x, charSpawnY, z), Quaternion.identity);
        characterDict.Add(characterID, ((Character)newChar.GetComponent(typeof(Character))));
        Character localChar = (Character) newChar.GetComponent(typeof(Character));
        localChar.UpdateCharStats(HPcurrent, MPcurrent, APcurrent, inventorySize, loud, killedBySandworm);
        if (clientId == this.clientID)
        {
            localChar.house = playerHouse;
        }
        else
        {
            localChar.house = enemyHouse;
        }
        localChar.setMaxAP(APMax);
        localChar.setMaxHP(HPcurrent);
        return true;
    }

    /**
     * Used to update data of a character (HP etc.)
     */
    public bool characterStatChange(int characterID, int HP, int MP, int AP, int AD, int spiceInv, bool isLoud, bool isSwallowed)
    {
        if (!characterDict.ContainsKey(characterID))
            return false;

        Character charScript = getCharScriptByID(characterID);
        charScript.UpdateCharStats(HP, MP, AP, spiceInv, isLoud, isSwallowed);
        return true;
    }

    public Character getCharScriptByID(int characterID)
    {
        return characterDict[characterID];
    }


    GameObject getCharTypeByEnum(CharTypeEnum type)
    {
        switch (type)
        {
            case CharTypeEnum.NOBLE:
                return noblePrefab;
            case CharTypeEnum.BENEGESSERIT:
                return beneGesseritPrefab;
            case CharTypeEnum.MENTANT:
                return mentatPrefab;
            case CharTypeEnum.FIGHTER:
                return fighterPrefab;
            default:
                Debug.Log("Error in CharacterMgr: CharType doesn't exist!");
                return null;
        }
    }


    /**
     * Inits a Sandworm at given pos
     */
    public void SpawnSandworm(int x, int z)
    {
        if (sandwormMoveScript == null)
        {
            sandwormMoveScript = ((MoveAbles)Instantiate(sandwormPrefab, new Vector3(x, wormHeightOffset + MapManager.instance.getNodeFromPos(x, z).charHeightOffset, z), Quaternion.identity).GetComponent(typeof(MoveAbles)));
        }
        else
        {
            Debug.Log("There is already a sandworm!");
        }
    }

    public void DespawnSandworm()
    {
        Destroy(sandwormMoveScript.gameObject);
        sandwormMoveScript = null;
    }

    /**
     * Makes the worm move along the given path
     */
    public void SandwormMove(List<Position> path)
    {
        Debug.Log("It's about to happen: " + sandwormMoveScript.name);
        LinkedList<Vector3> newPos = new LinkedList<Vector3>();

        Vector3 tmp;
        foreach (Position p in path)
        {
                tmp = new Vector3(p.x, 0.372f + MapManager.instance.getNodeFromPos(p.x, p.y).charHeightOffset, p.y);
                newPos.AddLast(tmp);
        }

        sandwormMoveScript.WalkAlongPath(newPos);
    }


    public int MaxHP(CharTypeEnum character)
    {
        switch(character)
        {
            case CharTypeEnum.NOBLE:
                return nobleMaxHP;
            case CharTypeEnum.MENTANT:
                return mentatMaxHP;
            case CharTypeEnum.BENEGESSERIT:
                return benneGesseritMaxHP;
            //case CharTypeEnum.FIGHTER:
            default:
                return fighterMaxHP;
        }
    }


    public void SetPlayerHouse(HouseEnum house)
    {
        playerHouse = house;
    }

    public void SetEnemyHouse(HouseEnum house)
    {
        enemyHouse = house;
    }



}
