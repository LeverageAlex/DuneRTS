using GameData.network.controller;
using GameData.network.util.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Serilog;
using System.Linq;

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

    public GameObject CharacterSpawnEffect;

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

    private HouseEnum playerHouse;
    private HouseEnum enemyHouse;

    private int nobleMaxHP;
    private int fighterMaxHP;
    private int benneGesseritMaxHP;
    private int mentatMaxHP;

    public static ClientConnectionHandler handler;
    public static CharacterMgr instance;


    // Start is called before the first frame update
    void Awake()
    {
        //Example to show Method
        if(instance == null)
        {
            instance = this;
        }
        
    }




    /*
     * To be filled after open question regarding standardDocument has ben resolved
  */
    public bool spawnCharacter(int clientId, int characterID, CharTypeEnum type,int x, int z,int HPcurrent, int healthMax, int MPcurrent, int APcurrent, int APMax, int inventorySize, bool killedBySandworm, bool loud)
    {
        if (characterDict.ContainsKey(characterID))
            return false;


        float charSpawnY = MapManager.instance.getNodeFromPos(x,z).heightLvl == Node.HeightLevel.high ? charSpawnHighY : charSpawnLowY;
        GameObject newChar = (GameObject) Instantiate(getCharTypeByEnum(type), new Vector3(x, charSpawnY, z), Quaternion.identity);
        PlayCharacterSpawnAnimation(newChar);
        characterDict.Add(characterID, ((Character)newChar.GetComponent(typeof(Character))));
        Log.Debug("Added Character to list: " + characterID);
        Log.Debug(characterDict[characterID].charName);
        Log.Debug("Successful print");
        Character localChar = (Character) newChar.GetComponent(typeof(Character));
        localChar.characterId = characterID;
        localChar.UpdateCharStats(HPcurrent, MPcurrent, APcurrent, inventorySize, loud, killedBySandworm);
        if (clientId == SessionHandler.clientId)
        {
            localChar.house = playerHouse;
        }
        else
        {
            localChar.house = enemyHouse;
        }
        localChar.SetMatColorToHouse();
        localChar.setMaxAP(APMax);
        localChar.setMaxHP(healthMax);
        return true;
    }

    public void PlayCharacterSpawnAnimation(GameObject newChar)
    {
        GameObject spawnEffect = Instantiate(CharacterSpawnEffect, newChar.transform.position, Quaternion.identity);
        
        Destroy(spawnEffect, 2);
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

    public void ClearCharDictionary()
    {
        characterDict.Clear();
    }


    /**
     * Inits a Sandworm at given pos
     */
    public void SpawnSandworm(int x, int z)
    {
        if (sandwormMoveScript != null)
        {
            Destroy(sandwormMoveScript.gameObject);
        }
            sandwormMoveScript = ((MoveAbles)Instantiate(sandwormPrefab, new Vector3(x, wormHeightOffset + MapManager.instance.getNodeFromPos(x, z).charHeightOffset, z), Quaternion.identity).GetComponent(typeof(MoveAbles)));
        
    }

    public void DespawnSandworm()
    {
        if (sandwormMoveScript != null)
        {
            Destroy(sandwormMoveScript.gameObject);
            sandwormMoveScript = null;
        }
    }

    /**
     * Makes the worm move along the given path
     */
    public void SandwormMove(List<Position> path)
    {
        if (sandwormMoveScript != null)
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

    public Character randomChar()
    {
        List<Character> values = Enumerable.ToList(characterDict.Values);
        return values.ElementAt(0);
    }


    public void SetPlayerHouse(HouseEnum house)
    {
        playerHouse = house;
    }

    public void SetEnemyHouse(HouseEnum house)
    {
        enemyHouse = house;
    }

    public void removeCharacter(int charID)
    {
        characterDict.Remove(charID);
    }
    public Helicopter spawnHelicopter(Character charToTransport, Position target, bool crash)
    {
        GameObject obj = Instantiate(helicopterPrefab, new Vector3(charToTransport.X, 0f, charToTransport.Z), Quaternion.identity);
        Helicopter copterScript = (Helicopter)obj.GetComponent(typeof(Helicopter));
        copterScript.InitHelicopter(charToTransport, new Vector3(target.x, 0f, target.y), crash);
        return copterScript;
    }

}
