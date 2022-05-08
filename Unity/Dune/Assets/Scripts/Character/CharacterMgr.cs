using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMgr : MonoBehaviour
{
    //Connects CharacterID to CharacterObject
    Dictionary<int, GameObject> characterDict = new Dictionary<int, GameObject>();

    public GameObject noblePrefab;
    public GameObject mentatPrefab;
    public GameObject beneGesseritPrefab;
    public GameObject fighterPrefab;

    public GameObject atomicPrefab;

    public GameObject sandwormPrefab;

    private MoveAbles sandwormMoveScript;

    private float charSpawnLowY = 0.35f;
    private float charSpawnHighY = 0.525f;

    private float wormHeightOffset = 0.35f;

    public int clientID;

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
    public bool spawnCharacter(int characterID, CharTypeEnum type,int x, int z,int HPcurrent, int healingHP, int MPcurrent, int APcurrent, int attackDamage, int inventoryLeft, bool killedBySandworm, bool loud)
    {
        if (characterDict.ContainsKey(characterID))
            return false;


        float charSpawnY = MapManager.instance.getNodeFromPos(x,z).heightLvl == Node.HeightLevel.high ? charSpawnHighY : charSpawnLowY;
        GameObject newChar = (GameObject) Instantiate(getCharTypeByEnum(type), new Vector3(x, charSpawnY, z), Quaternion.identity);
        characterDict.Add(characterID, newChar);
        Character localChar = (Character) newChar.GetComponent(typeof(Character));
        localChar.UpdateCharStats(HPcurrent, healingHP, MPcurrent, APcurrent, attackDamage, inventoryLeft, loud, killedBySandworm);
        return true;
    }

    public bool characterStatChange(int characterID, int HP, int HealHP, int MP, int AP, int AD, int spiceInv, bool isLoud, bool isSwallowed)
    {
        if (!characterDict.ContainsKey(characterID))
            return false;

        Character charScript = getCharScriptByID(characterID);
        charScript.UpdateCharStats(HP, HealHP, MP, AP, AD, spiceInv, isLoud, isSwallowed);
        return true;
    }

    public Character getCharScriptByID(int characterID)
    {
        return (Character)characterDict[characterID].GetComponent(typeof(Character));
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


    public void SpawnSandworm(int x, int z)
    {
        if (sandwormMoveScript == null)
        {
            sandwormMoveScript = ((MoveAbles)Instantiate(sandwormPrefab, new Vector3(x, wormHeightOffset + MapManager.instance.getNodeFromPos(x, z).charHeightOffset, z), Quaternion.identity).GetComponent(typeof(MoveAbles)));
        }
        else Debug.Log("There is already a sandworm!");
    }

    public void SandwormMove(LinkedList<Vector3> path)
    {
        Debug.Log("It's about to happen: " + sandwormMoveScript.name);
        sandwormMoveScript.WalkAlongPath(path);
    }

}
