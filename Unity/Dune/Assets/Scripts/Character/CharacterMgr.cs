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

    private float charSpawnLowY = 0.35f;
    private float charSpawnHighY = 0.525f;

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





    /*
     * To be filled after open question regarding standardDocument has ben resolved
  */  public bool spawnCharacter(int characterID, CharTypeEnum type,int x, int z,int HPcurrent, int healingHP, int MPcurrent, int APcurrent, int attackDamage, int inventoryLeft, bool killedBySandworm, bool loud)
    {
        if (characterDict.ContainsKey(characterID))
            return false;


        float charSpawnY = NodeManager.instance.getNodeFromPos(x,z).heightLvl == Node.HeightLevel.high ? charSpawnHighY : charSpawnLowY;
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


}
